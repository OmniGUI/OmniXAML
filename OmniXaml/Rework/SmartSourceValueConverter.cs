namespace OmniXaml.Rework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SmartSourceValueConverter : ISmartSourceValueConverter
    {
        private IEnumerable<ITaggedSmartSourceValueConverter> converters = new List<ITaggedSmartSourceValueConverter>() { new BuiltInConverter() };

        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            var results = from c in converters
                let r = c.TryConvert(strValue, desiredTargetType)
                where r.Item1
                select new {r.Item2, r.Item3};

            if (results.Any())
            {
                return (true, results.First().Item2);
            }

            return (false, null);
        }
    }
}
