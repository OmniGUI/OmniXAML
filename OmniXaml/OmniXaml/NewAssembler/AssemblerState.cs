namespace OmniXaml.NewAssembler
{
    using Glass;

    public class AssemblerState : StackingLinkedList<Level>
    {
        public void AssignChildToParent()
        {
            var child = CurrentValue.Instance;
            var parent = PreviousValue.Instance;
            var parentProperty = PreviousValue.XamlMember;
            parentProperty.SetValue(parent, child);
        }

        public bool HasCurrentInstance => CurrentValue.Instance == null;
        public bool IsProcessingCollection { get; set; }
    }
}