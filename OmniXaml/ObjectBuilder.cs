namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Ambient;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly IConverterContextFactory contextFactory;
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;


        public ObjectBuilder(IInstanceCreator creator, ObjectBuilderContext objectBuilderContext, IConverterContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            ObjectBuilderContext = objectBuilderContext;
            this.creator = creator;
            sourceValueConverter = objectBuilderContext.SourceValueConverter;
        }

        protected ObjectBuilderContext ObjectBuilderContext { get; }

        public object Inflate(ConstructionNode node, BuildContext buildContext, object instance = null)
        {
            if (buildContext.Root == null)
            {
                buildContext.Root = node;
                buildContext.PrefixedTypeResolver.Root = node;
            }
            
            return InflateCore(node, buildContext, instance);
        }

        private object InflateCore(ConstructionNode node, BuildContext buildContext, object instance = null)
        {
            EnsureValidRoot(node, buildContext.Root);

            buildContext.CurrentNode = node;

            if (instance == null)
            {
                instance = CreateInstance(node, buildContext);
                buildContext.InstanceLifecycleSignaler.OnBegin(instance);
            }
            else
            {
                NotifyNewInstance(buildContext, instance);
            }
            
            ApplyAssignments(instance, node.Assignments, buildContext);
            InflateChildren(node.Children, instance, buildContext);
            buildContext.InstanceLifecycleSignaler.OnEnd(instance);
            return instance;                                                       
        }

        private void EnsureValidRoot(ConstructionNode node, ConstructionNode root)
        {
            if (!Equals(node, root) && node.InstantiateAs != null)
            {
                throw new InvalidOperationException($"'InstantiateAs' has been set for the node {node}, but this feature is only valid for the root node.");
            }
        }

        private static void NotifyNewInstance(BuildContext buildContext, object instance)
        {
            buildContext.NamescopeAnnotator.TrackNewInstance(instance);
            buildContext.AmbientRegistrator.RegisterInstance(instance);
        }

        private object CreateInstance(ConstructionNode node, BuildContext buildContext)
        {
            EnsureValidNode(node);

            var instanceType = node.InstantiateAs ?? node.InstanceType;

            var instance = creator.Create(instanceType, buildContext, node.InjectableArguments.Select(s => new InjectableMember(s)));
            NotifyNewInstance(buildContext, instance);

            if (node.Name != null)
                buildContext.NamescopeAnnotator.RegisterName(node.Name, instance);

            return instance;
        }

        private void EnsureValidNode(ConstructionNode node)
        {
            if (node.InstantiateAs != null)
            {
                Type instanceType = node.InstanceType;
                Type instantiateAsType = node.InstantiateAs;
                if (!instanceType.GetTypeInfo().IsAssignableFrom(instantiateAsType.GetTypeInfo()))
                {                
                    throw new InvalidOperationException($"A node of type {instanceType} cannot be instantiated as {instantiateAsType}. The node of type {instanceType} has been tried to inflate as {instantiateAsType}, but this is not possible since the {instantiateAsType} is not derived from {instanceType}");
                }
            }
        }

        private void InflateChildren(IEnumerable<ConstructionNode> children, object parent, BuildContext buildContext)
        {
            foreach (var constructionNode in children)
            {
                var child = InflateCore(constructionNode, buildContext);
                var association = new ChildAssociation(parent, new KeyedInstance(child, constructionNode.Key));

                Associate(association, buildContext);
            }
        }

        protected void Associate(ChildAssociation childAssociation, BuildContext buildContext)
        {
            var childInstance = childAssociation.Child.Instance;
            var childKey = childAssociation.Child.Key;
            var parent = childAssociation.Parent;

            if (childKey == null)
            {
                Collection.UniversalAdd(parent, childInstance);
            }
            else
            {
                Collection.UniversalAddToDictionary(parent, childInstance, childKey);
            }

            OnInstanceAssociated(buildContext, childInstance);
        }

        protected void OnInstanceAssociated(BuildContext buildContext, object childInstance)
        {
            buildContext.InstanceLifecycleSignaler.AfterAssociatedToParent(childInstance);
        }

        private void ApplyAssignment(MemberAssignment assignment, object target, BuildContext buildContext)
        {
            EnsureValidAssigment(assignment);
            var property = assignment.Member;

            if ((assignment.Children.Count() == 1) || (assignment.SourceValue != null))
                ApplySingleAssignment(assignment, target, buildContext, property);
            else
                ApplyMultiAssignment(assignment, target, buildContext, property);
        }

        private void ApplyMultiAssignment(MemberAssignment assignment, object instance, BuildContext buildContext, Member property)
        {
            foreach (var constructionNode in assignment.Children)
            {
                var originalValue = InflateCore(constructionNode, buildContext);
                var child = MakeCompatible(instance, new ConversionRequest(property, originalValue), buildContext);

                var parent = property.GetValue(instance);
                var pendingAdd = new ChildAssociation(parent, new KeyedInstance(child, constructionNode.Key));

                Associate(pendingAdd, buildContext);
            }
        }

        private void ApplySingleAssignment(MemberAssignment assignment, object instance, BuildContext buildContext, Member member)
        {
            object value;
            string key = null;
            if (assignment.SourceValue == null)
            {
                var first = assignment.Children.First();
                key = first.Key;
                value = CreateChildProperty(instance, member, first, buildContext);
            }
            else
            {
                value = assignment.SourceValue;
            }

            var compatibleValue = MakeCompatible(instance, new ConversionRequest(member, value), buildContext);
            PerformAssigment(new Assignment(new KeyedInstance(instance, key), member, compatibleValue), buildContext);
        }

        private static void OnAssigmentExecuted(Assignment assignment, BuildContext buildContext)
        {
            var ambientPropertyAssignment = new AmbientMemberAssignment
            {
                Property = assignment.Member,
                Value = assignment.Value
            };

            buildContext.AmbientRegistrator.RegisterAssignment(ambientPropertyAssignment);
        }

        protected virtual object MakeCompatible(object instance, ConversionRequest conversionRequest, BuildContext buildContext)
        {
            var value = conversionRequest.Value;

            if (value is string)
            {
                var valueContext = contextFactory.CreateConverterContext(conversionRequest.Member.MemberType, value, buildContext);
                var compatibleValue = sourceValueConverter.GetCompatibleValue(valueContext);
                return compatibleValue;
            }

            return value;
        }

        protected virtual void PerformAssigment(Assignment assignment, BuildContext buildContext)
        {
            if (assignment.Member.MemberType.IsCollection())
            {
                var parent = assignment.Member.GetValue(assignment.Target.Instance);
                var child = assignment.Value;
                var pendingAdd = new ChildAssociation(parent, new KeyedInstance(child, assignment.Target.Key));
                Associate(pendingAdd, buildContext);
            }
            else
            {
                assignment.ExecuteAssignment();
                OnAssigmentExecuted(assignment, buildContext);
            }
        }

        protected virtual object CreateChildProperty(object parent, Member member, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            return InflateCore(nodeToBeCreated, buildContext);
        }

        protected virtual void ApplyAssignments(object instance, IEnumerable<MemberAssignment> assigments, BuildContext buildContext)
        {
            foreach (var propertyAssignment in assigments)
                ApplyAssignment(propertyAssignment, instance, buildContext);
        }

        private static void EnsureValidAssigment(MemberAssignment assignment)
        {
            if ((assignment.SourceValue != null) && (assignment.Children != null) && assignment.Children.Any())
                throw new InvalidOperationException("You cannot specify a Source Value and Children at the same time.");

            if ((assignment.SourceValue == null) && !assignment.Children.Any())
                throw new InvalidOperationException("Children is empty.");
        }
    }
}