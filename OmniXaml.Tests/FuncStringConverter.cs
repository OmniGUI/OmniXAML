using System;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Tests
{
    internal class FuncStringConverter : IStringSourceValueConverter
    {
        private readonly Func<string, (bool success, object converter)> func;

        public FuncStringConverter(Func<string, (bool success, object converter)> func)
        {
            this.func = func;
        }

        public (bool, object) TryConvert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            return func(strValue);
        }
    }
}