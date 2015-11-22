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

        public ObjectAssembler(IWiringContext wiringContext, ITopDownValueContext topDownValueContext, ObjectAssemblerSettings settings = null)
            : this(new StackingLinkedList<Level>(), wiringContext, topDownValueContext)
        {
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownValueContext, nameof(topDownValueContext));

            this.topDownValueContext = topDownValueContext;
            StateCommuter.RaiseLevel();

            rootInstance = settings?.RootInstance;
            var rootInstanceType = rootInstance?.GetType();
            rootInstanceXamlType = rootInstanceType != null ? wiringContext.TypeContext.TypeRepository.GetXamlType(rootInstanceType) : null;
        }

        public ObjectAssembler(StackingLinkedList<Level> state, IWiringContext wiringContext, ITopDownValueContext topDownValueContext)
        {
            WiringContext = wiringContext;
            StateCommuter = new StateCommuter(this, state, wiringContext, topDownValueContext);
        }

        public StateCommuter StateCommuter { get; }

        private bool IsLevelOneAndThereIsRootInstance => StateCommuter.Level == 1 && rootInstance != null;

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public IWiringContext WiringContext { get; }

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
                    command = new StartMemberCommand(this, GetMember(instruction.Member));
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

        private XamlMemberBase GetMember(XamlMemberBase member)
        {
            if (IsLevelOneAndThereIsRootInstance && !member.IsDirective)
            {
                return rootInstanceXamlType == null ? member : rootInstanceXamlType.GetMember(member.Name);
            }

            return member;
        }
    }
}