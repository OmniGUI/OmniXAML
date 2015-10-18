namespace OmniXaml.Tests.Classes
{
    public class SpyingParent
    {
        private ChildClass child;

        public ChildClass Child
        {
            get { return child; }
            set
            {
                child = value;
                if (child.Name != null)
                {
                    ChildHadNamePriorToBeingAssigned = true;
                }
            }
        }

        public bool ChildHadNamePriorToBeingAssigned { get; private set; }
    }
}