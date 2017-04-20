namespace OmniXaml.Tests.Rework2
{
    using System;

    public class FuncStringConverterExtended : IStringSourceValueConverter
    {
        private readonly Func<string, Type, (bool, object)> func;

        public FuncStringConverterExtended(Func<string, Type, (bool, object)> func)
        {
            this.func = func;
        }

        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            return func(strValue, desiredTargetType);
        }
    }
}