namespace OmniXaml.Rework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SmartSourceValueConverter : ISmartSourceValueConverter
    {
        private readonly IEnumerable<ISmartSourceValueConverter> converters = new List<ISmartSourceValueConverter>() { new BuiltInConverter() };

        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
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
