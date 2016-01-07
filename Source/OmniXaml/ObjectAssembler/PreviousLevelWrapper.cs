namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using Typing;

    public class PreviousLevelWrapper
    {
        private readonly Level level;

        public PreviousLevelWrapper(Level level)
        {
            this.level = level;
        }

        public MemberBase Member
        {
            get { return level.Member; }
            set { level.Member = value; }
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

        public bool XamlMemberIsOneToMany => Member.XamlType.IsCollection;
        public bool IsDictionary => Collection is IDictionary;

        public bool CanHostChildren => XamlMemberIsOneToMany && Collection != null;
        public bool IsEmpty => level.IsEmpty;
    }
}