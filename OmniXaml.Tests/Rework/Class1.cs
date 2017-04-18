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
}