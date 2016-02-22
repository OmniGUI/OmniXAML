namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using TypeConversion;
    using Typing;

    public class CurrentLevelWrapper
    {
        private readonly Level level;
        private readonly IValueContext valueContext;

        public CurrentLevelWrapper(Level level, IValueContext valueContext)
        {
            this.level = level;
            this.valueContext = valueContext;
        }

        public string InstanceName
        {
            get
            {
                var runtimeNameMember = XamlType.RuntimeNamePropertyMember;
                return (string) runtimeNameMember?.GetValue(Instance);            
            }
            set
            {
                var runtimeNameMember = XamlType.RuntimeNamePropertyMember;
                runtimeNameMember?.SetValue(Instance, value, valueContext);
            }
        }

        public XamlType InstanceXamlType => valueContext.TypeRepository.GetByType(Instance.GetType());

        public MemberBase Member
        {
            get { return level.Member; }
            set { level.Member = value; }
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
            set
            {
                level.Instance = value;

                if (value!=null)
                {
                    var type = value.GetType();
                    var xamlType = valueContext.TypeRepository.GetByType(type);
                    valueContext.TopDownValueContext.Add(value, xamlType);
                }
            }
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

        public InstanceProperties InstanceProperties => level.InstanceProperties;

        public bool WasAssociatedRightAfterCreation
        {
            get { return level.WasInstanceAssignedRightAfterBeingCreated; }
            set { level.WasInstanceAssignedRightAfterBeingCreated = value; }
        }
    }
}