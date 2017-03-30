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

        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            var results = from c in converters
                let r = c.TryConvert(strValue, desiredTargetType)
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