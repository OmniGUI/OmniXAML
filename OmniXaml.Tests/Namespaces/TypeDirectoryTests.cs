using System;

namespace OmniXaml.Tests.Namespaces
{
    using System.Reflection;
    using Model;
    using TypeLocation;
    using Xunit;
    
    public class TypeDirectoryTests
    {
        [Fact]
        public void GetTypeFromXamlNamespace()
        {
            var sut = CreateSut();
            var type = sut.GetTypeByFullAddress(new Address("root", "TextBlock"));
            Assert.NotNull(type);
        }

        [Fact]
        public void GetTypeFromClrNamespace()
        {
            var sut = CreateSut();
            var type = sut.GetTypeByFullAddress(new Address("using:OmniXaml.Tests.Model;assembly=OmniXaml.Tests", "TextBlock"));
            Assert.NotNull(type);
        }

        private ITypeDirectory CreateSut()
        {
            var type = typeof(TextBlock);
            var assembly = type.GetTypeInfo().Assembly;
            var xamlNamespaces = XamlNamespace.Map("root").With(Route.Assembly(assembly).WithNamespaces(type.Namespace));
            return new TypeDirectory(new[] { xamlNamespaces });
        }
    }
}
