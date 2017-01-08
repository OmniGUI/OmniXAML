namespace OmniXaml.Tests.Model
{
    using System.Collections.Generic;
    using Glass.Core;

    public class ResourceDictionary : Dictionary<object, object>
    {
        public override bool Equals(object obj)
        {
            var rd = obj as ResourceDictionary;
            return this.ContentEquals(rd);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }    
}