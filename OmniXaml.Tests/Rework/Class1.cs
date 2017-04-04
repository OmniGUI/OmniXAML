using System;
using System.ComponentModel;
using OmniXaml.Rework;
using OmniXaml.Services;
using OmniXaml.Tests.Model;

namespace OmniXaml.Tests.Rework
{
    public class Class1
    {
        public void NamescopeTest()
        {
            var sut = new LaPolla();
            var node = sut.Inflate(new ConstructionNode(typeof(Window)));
        }
    }

    public class LaPolla
    {

        public LaPolla()
        {

        }

        public object Inflate(ConstructionNode constructionNode)
        {
            INewObjectBuilder innerBestia = new NewObjectBuilder(new SimpleInstanceCreator(), new SimpleValueConverter(), new NoActionValuePipeline());
            innerBestia.NodeInflated.Subscribe();
            return innerBestia.Inflate(constructionNode);
        }
    }



    internal class SimpleValueConverter : IStringSourceValueConverter
    {
        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            var convertFrom = TypeDescriptor.GetConverter(desiredTargetType).ConvertFrom(strValue);
            return (true, convertFrom);
        }
    }
}