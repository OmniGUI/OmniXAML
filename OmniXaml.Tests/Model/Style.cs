namespace OmniXaml.Tests.Model
{
    using DefaultLoader;

    public class Style
    {
        private string value;

        [DependsOn("Value")]
        public string TargetType { get; set; }

        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (TargetType != null)
                {
                    RightOrder = true;
                }
            }
        }

        public bool RightOrder { get; set; }
    }
}