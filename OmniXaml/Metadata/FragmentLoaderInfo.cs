namespace OmniXaml.Metadata
{
    using System;

    public class FragmentLoaderInfo
    {
        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public IConstructionFragmentLoader Loader { get; set; }
    }
}