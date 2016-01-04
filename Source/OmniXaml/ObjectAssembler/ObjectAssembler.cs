namespace OmniXaml.ObjectAssembler
{
    using System;
    using System.Collections;
    using Commands;
    using Glass;
    using Typing;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly object rootInstance;
        private readonly XamlType rootInstanceXamlType;
        private readonly ITopDownValueContext topDownValueContext;

        public ObjectAssembler(ITypeContext typeContext, ITopDownValueContext topDownValueContext, ObjectAssemblerSettings settings = null)
            : this(new StackingLinkedList<Level>(), typeContext, topDownValueContext)
        {
            Guard.ThrowIfNull(typeContext, nameof(typeContext));
            Guard.ThrowIfNull(topDownValueContext, nameof(topDownValueContext));

            this.TypeContext = typeContext;
            this.topDownValueContext = topDownValueContext;
            StateCommuter.RaiseLevel();

            rootInstance = settings?.RootInstance;
            var rootInstanceType = rootInstance?.GetType();
            rootInstanceXamlType = rootInstanceType != null ? TypeContext.GetXamlType(rootInstanceType) : null;
        }

        public ObjectAssembler(StackingLinkedList<Level> state, ITypeContext typeContext, ITopDownValueContext topDownValueContext)
        {
            StateCommuter = new StateCommuter(this, state, typeContext, topDownValueContext);
        }

        public StateCommuter StateCommuter { get; }

        private bool IsLevelOneAndThereIsRootInstance => StateCommuter.Level == 1 && rootInstance != null;

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public ITypeContext TypeContext { get; }

        public InstanceLifeCycleHandler InstanceLifeCycleHandler { get; set; } = new InstanceLifeCycleHandler();

        public void Process(XamlInstruction instruction)
        {
            Command command;

            switch (instruction.InstructionType)
            {
                case XamlInstructionType.NamespaceDeclaration:
                    command = new NamespaceDeclarationCommand(this, instruction.NamespaceDeclaration);
                    break;
                case XamlInstructionType.StartObject:
                    command = new StartObjectCommand(this, instruction.XamlType, rootInstance);
                    break;
                case XamlInstructionType.StartMember:
                    command = new StartMemberCommand(this, GetActualMemberFromMemberSpecifiedInInstruction(instruction.Member));
                    break;
                case XamlInstructionType.Value:
                    command = new ValueCommand(this, topDownValueContext, (string) instruction.Value);
                    break;
                case XamlInstructionType.EndObject:
                    command = new EndObjectCommand(this);
                    break;
                case XamlInstructionType.EndMember:
                    command = new EndMemberCommand(this, topDownValueContext);
                    break;
                case XamlInstructionType.GetObject:
                    command = new GetObjectCommand(this);
                    break;
                default:
                    throw new XamlParseException($"The XamlInstructionType {instruction.InstructionType} has an unexpected value");
            }

            command.Execute();
        }

        public void OverrideInstance(object instance)
        {
            StateCommuter.RaiseLevel();
            var tempQualifier = StateCommuter;
            tempQualifier.Current.Instance = instance;

            var collection = instance as ICollection;
            if (collection != null)
            {
                tempQualifier.Current.Collection = collection;
            }
        }

        private XamlMemberBase GetActualMemberFromMemberSpecifiedInInstruction(XamlMemberBase specifiedMember)
        {
            if (IsLevelOneAndThereIsRootInstance && !specifiedMember.IsDirective && rootInstanceXamlType != null)
            {
                var attachable = specifiedMember as AttachableXamlMember;

                XamlMemberBase actualMember;

                if (attachable != null)
                {
                    actualMember = attachable.DeclaringType.GetAttachableMember(specifiedMember.Name);
                }
                else
                {
                    actualMember = rootInstanceXamlType.GetMember(specifiedMember.Name);
                }

                return actualMember;
            }

            return specifiedMember;
        }
    }
}