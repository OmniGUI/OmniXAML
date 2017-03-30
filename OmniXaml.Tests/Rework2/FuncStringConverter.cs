namespace OmniXaml.Tests.Rework2
{
    using System;

    internal class FuncStringConverter : IStringSourceValueConverter
    {
        private readonly Func<string, (bool success, object converter)> func;

        public FuncStringConverter(Func<string, (bool success, object converter)> func)
        {
            this.func = func;
        }

        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            return func(strValue);
        }
    }
}