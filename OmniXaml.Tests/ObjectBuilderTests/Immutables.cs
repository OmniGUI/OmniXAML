namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Model;
    using Xunit;

    public class Immutables : ObjectBuilderTestsBase
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static IEnumerable<object[]> SimpleTypesData => new Collection<object[]>
        {
            new object[] {typeof(string), "Some Text", "Some Text"},
            new object[] {typeof(int), "1234", 1234},
            new object[] {typeof(double), "123.45", 123.45D},
            new object[] {typeof(float), "123.45", 123.45F},
            new object[] {typeof(uint), "123", 123U},
            new object[] {typeof(long), "1234", 1234L},
            new object[] {typeof(ulong), "1234", 1234UL},
            new object[] {typeof(bool), "true", true},
            new object[] {typeof(bool), "false", false},
            new object[] {typeof(byte), "123", (byte)123},
            new object[] {typeof(decimal), "1234.56", (decimal) 1234.56},
        };
     
        [Theory]
        [MemberData(nameof(SimpleTypesData))]
        public void Simple_Types(Type type, string textInput, object result)
        {
            var node = new ConstructionNode(type)
            {
                PositionalParameter = new[] { textInput }
            };

            var fixture = Create(node);
            Assert.Equal(result, fixture.Result);
        }

        [Fact]
        public void ImmutableFromContent()
        {
            var node = new ConstructionNode(typeof(MyImmutable)) { PositionalParameter = new[] { "Hola" } };
            var myImmutable = new MyImmutable("Hola");
            var fixture = Create(node);

            Assert.Equal(myImmutable, fixture.Result);
        }
    }
}