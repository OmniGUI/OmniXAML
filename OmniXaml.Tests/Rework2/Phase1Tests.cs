namespace OmniXaml.Tests.Rework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Model;
    using OmniXaml.Rework;
    using Rework2;
    using ReworkPhases;
    using Xunit;

    public class Phase1Tests
    {
        private static Phase1Builder CreateSut()
        {
            return new Phase1Builder(new SimpleInstanceCreator(), null);
        }

        [Fact]
        public void Collection()
        {
            var creator = new SmartInstanceCreatorMock();
            creator.SetObjectFactory((type, hints) =>
            {
                if (type == typeof(string))
                {
                    return new CreationResult(hints.Positionals.First().Instance,
                        new CreationHints(new NewInjectableMember[0], new[] { hints.Positionals.First() },
                            new List<object>()));
                }
                var i = Activator.CreateInstance(type);
                return new CreationResult(i, new CreationHints());
            });


            var ctn = new ConstructionNode(typeof(Collection))
            {
                Children = new[]
                {
                    new ConstructionNode(typeof(string))
                    {
                        PositionalParameter = new[] {"hola"}
                    }
                }
            };

            var actual = new Phase1Builder(creator, null).Inflate(ctn);
            var expected = new InflatedNode()
            {
                Instance = new Collection(),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "hola",
                        IsResolved = true,
                    }
                },
                IsResolved = true,
            };

            Assert.Equal(expected, actual, new InflatedNodeComparer());
        }

        [Fact(Skip = "Nada")]
        public void CreateSimpleType_String()
        {
            var fixture = new ObjectBuildFixture();
            var str = "salutations";

            fixture.Creator.SetObjectFactory((type, hints) => new CreationResult(str,
                new CreationHints(new NewInjectableMember[0], new[] { hints.Positionals.First() }, new List<object>())));

            var ctn = new ConstructionNode(typeof(string))
            {
                PositionalParameter = new[] { str }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(str, instance);
        }

        [Fact(Skip = "Nada")]
        public void ImmutableFromProperty()
        {
            var fixture = new ObjectBuildFixture();
            fixture.Creator.SetObjectFactory((type, hints) =>
            {
                var argument = (string)hints.Members.First().Values.First();
                var myImmutable = new MyImmutable(argument);
                return new CreationResult(myImmutable,
                    new CreationHints(new[] { hints.Members.First() }, new List<PositionalParameter>(),
                        new List<object>()));
            });
            var ctn = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<MyImmutable>(immutable => immutable.Argument),
                        SourceValue = "Saludos"
                    }
                }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(new MyImmutable("Saludos"), instance);
        }

        [Fact]
        public void SingleInstance()
        {
            var ctn = new ConstructionNode(typeof(Window));

            var inflatedNode = new InflatedNode
            {
                Instance = new Window(),
            };

            Assert.Equal(inflatedNode, CreateSut().Inflate(ctn), new InflatedNodeComparer());
        }

        [Fact]
        public void SingleInstanceWithMemberInside()
        {
            var ctn = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(TextBlock))
                        }
                    }
                }
            };

            var inflatedNode = new InflatedNode
            {
                Instance = new Window(),
                Assigments = new List<InflatedMemberAssignment>
                {
                    new InflatedMemberAssignment
                    {
                        Member = Member.FromStandard<Window>(w => w.Content),
                        Children = new List<InflatedNode>
                        {
                            new InflatedNode
                            {
                                Instance = new TextBlock()
                            }
                        }
                    }
                }
            };

            Assert.Equal(inflatedNode, CreateSut().Inflate(ctn), new InflatedNodeComparer());
        }

        [Fact(Skip = "Nada")]
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
                        SourceValue = "12.5"
                    }
                }
            };

            var instance = fixture.ObjectBuilder.Inflate(ctn);

            Assert.Equal(new Window { Height = 12.5 }, instance);
        }
    }
}