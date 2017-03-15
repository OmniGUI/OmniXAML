namespace OmniXaml.Tests.Rework
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Model;
    using Xunit;

    public class NewObjectBuilderTests
    {
        [Fact]
        public void SingleInstance()
        {
            var fixture = new ObjectBuildFixture();
            var instance = fixture.ObjectBuilder.Inflate(new ConstructionNode(typeof(Window)));

            Assert.IsType<Window>(instance);
        }

        [Fact]
        public void SingleInstanceWithMemberInside()
        {
            var fixture = new ObjectBuildFixture();
            var ctn = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(TextBlock)),
                        },
                    }
                }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(new Window() { Content = new TextBlock() }, instance);
        }

        [Fact]
        public void ImmutableFromProperty()
        {
            var fixture = new ObjectBuildFixture();
            fixture.Creator.SetObjectFactory((type, inject) => new CreationResult(new MyImmutable((string)inject.First().Value), new[] { inject.First() }));
            var ctn = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<MyImmutable>(immutable => immutable.Argument),
                        SourceValue = "Saludos",
                    }
                }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(new MyImmutable("Saludos"), instance);
        }

        [Fact]
        public void WhenValueIsNotCompatible_ConverterIsUsed()
        {
            var fixture = new ObjectBuildFixture();
            fixture.Converter.SetConvertFunc((str, type) => double.Parse(str, CultureInfo.InvariantCulture));

            var ctn = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(w => w.Height),
                        SourceValue = "12.5",
                    }
                }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(new Window() { Height = 12.5 }, instance);
        }
    }
}