namespace OmniXaml.NewAssembler
{
    using System;
    using Assembler;
    using Commands;
    using Glass;

    public class SuperObjectAssembler : IObjectAssembler
    {
        private readonly ObjectAssemblerSettings settings;

        public SuperObjectAssembler(WiringContext wiringContext, ObjectAssemblerSettings settings = null) : this(new StackingLinkedList<Level>(), wiringContext)
        {
            this.settings = settings;
            StateCommuter.RaiseLevel();
        }

        public SuperObjectAssembler(StackingLinkedList<Level> state, WiringContext wiringContext)
        {
            WiringContext = wiringContext;          
            StateCommuter = new StateCommuter(state, wiringContext);
        }

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public WiringContext WiringContext { get; }

        public StateCommuter StateCommuter { get; }

        public void Process(XamlNode node)
        {
            Command command;

            switch (node.NodeType)
            {
                case XamlNodeType.NamespaceDeclaration:
                    command = new NamespaceDeclarationCommand(this, node.NamespaceDeclaration);
                    break;
                case XamlNodeType.StartObject:
                    command = new StartObjectCommand(this, node.XamlType, settings?.RootInstance);
                    break;
                case XamlNodeType.StartMember:
                    command = new StartMemberCommand(this, node.Member);
                    break;
                case XamlNodeType.Value:
                    command = new ValueCommand(this, (string)node.Value);
                    break;
                case XamlNodeType.EndObject:
                    command = new EndObjectCommand(this);
                    break;
                case XamlNodeType.EndMember:
                    command = new EndMemberCommand(this);
                    break;
                case XamlNodeType.GetObject:
                    command = new GetObjectCommand(this);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            command.Execute();
        }

        public void OverrideInstance(object instance)
        {
            StateCommuter.RaiseLevel();
            StateCommuter.Instance = instance;
        }
    }
}