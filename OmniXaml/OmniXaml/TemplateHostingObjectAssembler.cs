namespace OmniXaml.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using Assembler;
    using Glass;
    using Typing;

    public class TemplateHostingObjectAssembler : IObjectAssembler
    {
        private readonly IObjectAssembler objectAssembler;
        private bool recording;
        private IList<XamlNode> nodeList;
        private int depth;

        private readonly IDictionary<PropertyInfo, IDeferredObjectAssembler> deferredObjectAssemblers = new Dictionary<PropertyInfo, IDeferredObjectAssembler>();
        private IDeferredObjectAssembler assembler;

        public TemplateHostingObjectAssembler(IObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
        }

        public WiringContext WiringContext
        {
            get { return objectAssembler.WiringContext; }
        }

        public void WriteNode(XamlNode node)
        {
            if (recording)
            {
                if (node.NodeType == XamlNodeType.StartMember)
                {
                    depth++;
                }

                if (node.NodeType == XamlNodeType.EndMember)
                {
                    depth--;
                    if (depth == 0)
                    {
                        recording = false;
                        var loaded = assembler.Load(new ReadOnlyCollection<XamlNode>(nodeList), WiringContext);
                        objectAssembler.OverrideInstance(loaded);
                        objectAssembler.WriteNode(node);
                    }
                }

                if (depth > 0)
                {
                    nodeList.Add(node);
                }
            }
            else
            {
                if (node.NodeType == XamlNodeType.StartMember && !node.Member.IsDirective)
                {                    
                    var hasAssembler = TryGetDeferredAssembler(node.Member, out assembler);
                    if (hasAssembler)
                    {
                        recording = true;
                        nodeList = new Collection<XamlNode>();
                        depth++;
                        objectAssembler.WriteNode(node);
                    }
                }

                if (!recording)
                {
                    objectAssembler.WriteNode(node);
                }                
            }
        }

        private bool TryGetDeferredAssembler(XamlMember xamlMember, out IDeferredObjectAssembler assembler)
        {
            Guard.ThrowIfNull(xamlMember, nameof(xamlMember));

            var propInfo = xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name);
            var success = deferredObjectAssemblers.TryGetValue(propInfo, out assembler);
            return success;
        }

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler
        {
            get { return objectAssembler.XamlSetValueHandler; }
            set { objectAssembler.XamlSetValueHandler = value; }
        }

        public void OverrideInstance(object instance)
        {            
        }

        public void PushScope()
        {
            throw new NotImplementedException();
        }

        public IList NodeList => new ReadOnlyCollection<XamlNode>(nodeList);

        public void DeferredAssembler<TItem>(Expression<Func<TItem, object>> selector, IDeferredObjectAssembler assembler)
        {
            var propInfo = typeof(TItem).GetRuntimeProperty(selector.GetFullPropertyName());
            deferredObjectAssemblers.Add(propInfo, assembler);
        }
    }

}