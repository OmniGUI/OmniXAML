using System;
using Moq;

namespace AberratioReader.Tests
{
    public class XamlReaderBuilder
    {
        private readonly ITypeFactory typeFactory;
        private readonly ITypeProvider typeProvider;

        public XamlReaderBuilder()
        {
            var tpm = new Mock<ITypeFactory>();
            tpm
                .Setup(factory => factory.Create(It.IsAny<Type>()))
                .Returns((Type t) => Activator.CreateInstance(t));

            typeFactory = tpm.Object;

            var tp = new Mock<ITypeProvider>();
            tp.Setup(provider => provider.GetType(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => typeof (DummyClass));
                
            typeProvider = tp.Object;
        }

        public XamlReader Build()
        {
            return new XamlReader(typeFactory, typeProvider);
        }
    }
}