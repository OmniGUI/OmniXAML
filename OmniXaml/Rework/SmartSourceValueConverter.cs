using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniXaml.Rework
{
    public class SmartSourceValueConverter : IStringSourceValueConverter
    {
        private readonly IEnumerable<IStringSourceValueConverter> converters =
            new List<IStringSourceValueConverter> {new TypeConverterSourceValueConverter()};

        public (bool, object) TryConvert(string strValue, Type desiredTargetType, ConvertContext context = null)
        {
            var results = from c in converters
                let r = c.TryConvert(strValue, desiredTargetType)
                where r.Item1
                select new {r.Item1, r.Item2};

            if (results.Any())
            {
                return (true, results.First().Item2);                
            }
            return (false, null);
        }
    }
}