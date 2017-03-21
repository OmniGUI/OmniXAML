using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace OmniXaml.Tests.Rework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Model;
    using OmniXaml.Rework;
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
            fixture.Creator.SetObjectFactory((type, hints) =>
            {
                var argument = (string)hints.Members.First().Values.First();
                var myImmutable = new MyImmutable(argument);
                return new CreationResult(myImmutable, new CreationHints(new[] {hints.Members.First()}, new List<PositionalParameter>(), new List<object>()));
            });
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
            fixture.Converter.SetConvertFunc((str, type) => (true, double.Parse(str, CultureInfo.InvariantCulture)));

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

        [Fact]
        public void CreateSimpleType_String()
        {
            var fixture = new ObjectBuildFixture();
            var str = "salutations";

            fixture.Creator.SetObjectFactory((type, hints) => new CreationResult(str, new CreationHints(new NewInjectableMember[0], new[] { hints.Positionals.First() }, new List<object>())));

            var ctn = new ConstructionNode(typeof(string))
            {
                PositionalParameter = new[] { str },
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(str, instance);
        }

        [Fact]
        public void Collection()
        {
            var fixture = new ObjectBuildFixture();
            fixture.Creator.SetObjectFactory((type, hints) =>
            {
                if (type == typeof(string))
                {
                    return new CreationResult(hints.Positionals.First().Instance, new CreationHints(new NewInjectableMember[0], new[] {hints.Positionals.First()}, new List<object>()));
                }
                else
                {
                    var i = Activator.CreateInstance(type);
                    return new CreationResult(i, new CreationHints());
                }
            });


            var ctn = new ConstructionNode(typeof(Collection))
            {
                Children = new[]
                {
                    new ConstructionNode(typeof(string))
                    {
                        PositionalParameter = new []{ "hola"},
                    },
                }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(new Collection() { "hola" }, instance);
        }

        [Fact]
        public async Task ConstructionNotification()
        {
            var fixture = new ObjectBuildFixture();
            fixture.ObjectBuilder.Inflate(new ConstructionNode(typeof(Window)));
            var inflatedNode = await fixture.ObjectBuilder.NodeInflated.FirstAsync();
            Assert.IsType<NodeInflation>(inflatedNode);
        }
    }
}