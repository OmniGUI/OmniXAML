namespace OmniXaml.NewAssembler
{
    using System;
    using Assembler;

    public class SuperObjectAssembler : IObjectAssembler
    {
        public SuperObjectAssembler(WiringContext wiringContext)
        {
            WiringContext = wiringContext;
            State.Push(new Level());
        }

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public WiringContext WiringContext { get; }

        public AssemblerState State { get; } = new AssemblerState();

        public void Process(XamlNode node)
        {
            Command command;

            switch (node.NodeType)
            {
                case XamlNodeType.NamespaceDeclaration:
                    command = new NamespaceDeclarationCommand(this, node.NamespaceDeclaration);
                    break;
                case XamlNodeType.StartObject:
                    command = new StartObjectCommand(this, node.XamlType);
                    break;
                case XamlNodeType.StartMember:
                    command = new StartMemberCommand(this, node.Member);
                    break;
                case XamlNodeType.Value:
                    command = new ValueCommand(this, node.Value);
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
        }
    }

    public class GetObjectCommand : Command
    {
        public GetObjectCommand(SuperObjectAssembler superObjectAssembler) : base(superObjectAssembler)
        {            
        }

        public override void Execute()
        {
            State.Push(new Level());

            PromoteMemberInstanceToCurrentInstance();

            State.CurrentValue.IsCollectionHolderObject = true;
        }

        private void PromoteMemberInstanceToCurrentInstance()
        {
            var member = State.PreviousValue.XamlMember;
            var instance = State.PreviousValue.Instance;
            var value = member.GetValue(instance);
            State.CurrentValue.Instance = value;
            State.CurrentValue.XamlType = member.Type;
        }
    }
}