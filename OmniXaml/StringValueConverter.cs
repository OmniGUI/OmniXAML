using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniXaml
{
    public class StringValueConverter : IStringSourceValueConverter
    {
        private readonly IEnumerable<IStringSourceValueConverter> converters =
            new List<IStringSourceValueConverter> {new ComponentModelTypeConverterBasedSourceValueConverter()};

        public (bool, object) Convert(string strValue, Type desiredTargetType, ConvertContext context = null)
        {
            var results = from c in converters
                let r = c.Convert(strValue, desiredTargetType)
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