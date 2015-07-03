namespace OmniXaml.NewAssembler
{
    using System.Collections;
    using Typing;

    public class Level
    {
        public Level()
        {
        }

        private Level(Level level)
        {
            Instance = level.Instance;
            XamlType = level.XamlType;
            XamlMember = level.XamlMember;
            IsCollectionHolderObject = level.IsCollectionHolderObject;
            IsMemberHostingChildren = level.IsMemberHostingChildren;
            IsObjectFromMember = level.IsObjectFromMember;
        }

        public object Instance { get; set; }
        public XamlType XamlType { get; set; }
        public XamlMember XamlMember { get; set; }
        public bool IsCollectionHolderObject { get; set; }
        public bool IsMemberHostingChildren { get; set; }
        public bool IsObjectFromMember { get; set; }
        public ICollection Collection { get; set; }

        public void MaterializeType()
        {
            var instance = XamlType.CreateInstance(null);
            Instance = instance;
        }

        public Level Clone()
        {
            return new Level(this);
        }
    }
}