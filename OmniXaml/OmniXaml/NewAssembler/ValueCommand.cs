namespace OmniXaml.NewAssembler
{
    public class ValueCommand : Command
    {
        private readonly object value;

        public ValueCommand(SuperObjectAssembler superObjectAssembler, object value) : base(superObjectAssembler)
        {
            this.value = value;
        }

        public override void Execute()
        {            
            Current.XamlMember.SetValue(Current.Instance, value);
        }
    }
}