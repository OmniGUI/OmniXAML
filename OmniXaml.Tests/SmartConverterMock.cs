using System;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Tests
{
    internal class SmartConverterMock : IStringSourceValueConverter
    {
        private Func<string, Type, (bool, object)> convertFunc = (str, type) =>  (true, System.Convert.ChangeType(str, type));

        public (bool, object) TryConvert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            return convertFunc(strValue, desiredTargetType);
        }

        public void SetConvertFunc(Func<string, Type, (bool, object)> func)
        {
            convertFunc = func;
        }
    }
}