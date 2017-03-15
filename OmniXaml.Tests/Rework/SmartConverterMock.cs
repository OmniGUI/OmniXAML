namespace OmniXaml.Tests.Rework
{
    using System;

    internal class SmartConverterMock : ISmartSourceValueConverter
    {
        private Func<string, Type, object> convertFunc = (str, type) =>  System.Convert.ChangeType(str, type);

        public object Convert(string strValue, Type desiredTargetType)
        {
            return convertFunc(strValue, desiredTargetType);
        }

        public void SetConvertFunc(Func<string, Type, object> func)
        {
            convertFunc = func;
        }
    }
}