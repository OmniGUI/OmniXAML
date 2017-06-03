namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SuperSmartSourceValueConverter : IStringSourceValueConverter
    {
        private readonly IEnumerable<IStringSourceValueConverter> converters;

        public SuperSmartSourceValueConverter(IEnumerable<IStringSourceValueConverter> converters)
        {
            this.converters = converters;
        }

        public (bool, object) Convert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            var results = from c in converters
                let r = c.Convert(strValue, desiredTargetType, context)
                where r.Item1
                select new { r.Item1, r.Item2 };

            if (Enumerable.Any(results))
            {
                return (true, Enumerable.First(results).Item2);
            }

            return (false, null);
        }
    }
}