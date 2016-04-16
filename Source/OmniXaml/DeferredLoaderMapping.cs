namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Glass.Core;

    public class DeferredLoaderMapping 
    {
        private readonly IDictionary<PropertyInfo, IDeferredLoader> deferredObjectAssemblers = new Dictionary<PropertyInfo, IDeferredLoader>();

        public void Map<TItem>(Expression<Func<TItem, object>> selector, IDeferredLoader assembler)
        {
            var propInfo = typeof(TItem).GetRuntimeProperty(selector.GetFullPropertyName());
            deferredObjectAssemblers.Add(propInfo, assembler);
        }

        public bool TryGetMapping(PropertyInfo info, out IDeferredLoader loader)
        {
            return deferredObjectAssemblers.TryGetValue(info, out loader);
        }
    }
}