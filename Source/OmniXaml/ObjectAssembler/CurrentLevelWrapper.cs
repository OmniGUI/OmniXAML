namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using Typing;

    public class CurrentLevelWrapper
    {
        private readonly Level level;

        public CurrentLevelWrapper(Level level)
        {
            this.level = level;
        }

        public XamlMemberBase XamlMember
        {
            get { return level.XamlMember; }
            set { level.XamlMember = value; }
        }

        public bool IsGetObject
        {
            get { return level.IsGetObject; }
            set { level.IsGetObject = value; }
        }

        public ICollection Collection
        {
            get { return level.Collection; }
            set { level.Collection = value; }
        }

        public object Instance
        {
            get { return level.Instance; }
            set { level.Instance = value; }
        }

        public bool WasInstanceAssignedRightAfterBeingCreated
        {
            get { return level.WasInstanceAssignedRightAfterBeingCreated; }
            set { level.WasInstanceAssignedRightAfterBeingCreated = value; }
        }

        public XamlType XamlType
        {
            get { return level.XamlType; }
            set { level.XamlType = value; }
        }

        public Collection<ConstructionArgument> CtorArguments
        {
            get { return level.CtorArguments; }
            set { level.CtorArguments = value; }
        }

        public bool HasInstance => Instance != null;
        public bool IsMarkupExtension => Instance is IMarkupExtension;
    }
}