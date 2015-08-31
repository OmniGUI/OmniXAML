namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using Typing;

    internal class PreviousLevelWrapper
    {
        private readonly Level level;

        public PreviousLevelWrapper(Level level)
        {
            this.level = level;
        }

        public XamlMemberBase XamlMember
        {
            get { return level.XamlMember; }
            set { level.XamlMember = value; }
        }

        public object Instance
        {
            get { return level.Instance; }
            set { level.Instance = value; }
        }

        public ICollection Collection
        {
            get { return level.Collection; }
            set { level.Collection = value; }
        }

        public bool IsOneToMany => Collection != null;
        public bool IsDictionary => Collection is IDictionary;
    }
}