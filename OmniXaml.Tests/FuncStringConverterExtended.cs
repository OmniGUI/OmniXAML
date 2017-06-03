using System;

namespace OmniXaml.Tests
{
    public class FuncStringConverterExtended : IStringSourceValueConverter
    {
        private readonly Func<string, Type, (bool, object)> func;

        public FuncStringConverterExtended(Func<string, Type, (bool, object)> func)
        {
            this.func = func;
        }

        public (bool, object) Convert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            return func(strValue, desiredTargetType);
        }
    }
}