namespace OmniXaml.Tests.Model
{
    using DefaultLoader;

    public class Setter
    {
        private string value;

        
        public string Property { get; set; }

        [DependsOn(nameof(Property))]
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (Property != null)
                {
                    RightOrder = true;
                }
            }
        }

        public bool RightOrder { get; set; }
    }
}