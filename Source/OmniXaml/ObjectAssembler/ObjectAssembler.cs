namespace OmniXaml.ObjectAssembler
{
    using System;
    using System.Collections;
    using Commands;
    using Glass;
    using Typing;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly ITopDownMemberValueContext topDownMemberValueContext;
        private readonly Type rootInstanceType;
        private readonly object rootInstance;

        public ObjectAssembler(IWiringContext wiringContext, ITopDownMemberValueContext topDownMemberValueContext, ObjectAssemblerSettings settings = null)
            : this(new StackingLinkedList<Level>(), wiringContext, topDownMemberValueContext)
        {
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownMemberValueContext, nameof(topDownMemberValueContext));

            this.topDownMemberValueContext = topDownMemberValueContext;
            StateCommuter.RaiseLevel();
            rootInstance = settings?.RootInstance;
            rootInstanceType = settings?.RootInstance?.GetType();
        }

        public ObjectAssembler(StackingLinkedList<Level> state, IWiringContext wiringContext, ITopDownMemberValueContext topDownMemberValueContext)
        {
            WiringContext = wiringContext;          
            StateCommuter = new StateCommuter(state, wiringContext, topDownMemberValueContext);
        }

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public IWiringContext WiringContext { get; }

        public StateCommuter StateCommuter { get; }

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
                    command = new ValueCommand(this, (string)instruction.Value);
                    break;
                case XamlInstructionType.EndObject:
                    command = new EndObjectCommand(this);
                    break;
                case XamlInstructionType.EndMember:
                    command = new EndMemberCommand(this, topDownMemberValueContext);
                    break;
                case XamlInstructionType.GetObject:
                    command = new GetObjectCommand(this);
                    break;
                default:
                    throw new XamlParseException($"The XamlInstructionType {instruction.InstructionType} has an unexpected value");
            }

            command.Execute();
        }

        private XamlMemberBase GetMember(XamlMemberBase member)
        {
            if (IsLevelOneAndThereIsRootInstance)
            {
                var xamlMember = WiringContext.TypeContext.GetXamlType(rootInstanceType).GetMember(member.Name);
                return rootInstanceType == null ? member : xamlMember;
            }

            return member;
        }

        private bool IsLevelOneAndThereIsRootInstance => StateCommuter.Level == 1 && rootInstance != null;

        public void OverrideInstance(object instance)
        {
            StateCommuter.RaiseLevel();
            StateCommuter tempQualifier = StateCommuter;
            tempQualifier.Current.Instance = instance;

            var collection = instance as ICollection;
            if (collection != null)
            {
                tempQualifier.Current.Collection = collection;
            }
        }
    }
}