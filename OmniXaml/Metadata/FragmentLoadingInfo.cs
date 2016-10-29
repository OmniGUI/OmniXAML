namespace OmniXaml.Metadata
{
    using System;

    public class FragmentLoadingInfo
    {
        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public IConstructionFragmentLoader Loader { get; set; }
    }
}