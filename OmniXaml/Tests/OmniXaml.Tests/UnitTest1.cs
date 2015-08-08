namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;
    using Visualization;

    [TestClass]
    public class OrderAwareXamlNodesPullParserTests : GivenAWiringContextWithNodeBuilders
    {
        private readonly Skimmer skimmer = new Skimmer();

        [TestMethod]
        public void FilterOutput()
        {
            var input = new List<XamlNode>
            {
                X.StartObject<Style>(),
                    X.StartMember<Style>(c => c.Setter),
                        X.StartObject<Setter>(),
                            X.StartMember<Setter>(c => c.Value),
                                X.Value("Value"),
                            X.EndMember(),
                            X.StartMember<Setter>(c => c.Property),
                                X.Value("Property"),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject()
            };

            var actualNodes = skimmer.FilterInput(input.GetEnumerator()).ToList();
            var expectedNodes = new List<XamlNode>
            {
                X.StartObject<Style>(),
                    X.StartMember<Style>(c => c.Setter),
                        X.StartObject<Setter>(),
                            X.StartMember<Setter>(c => c.Property),
                                X.Value("Property"),
                            X.EndMember(),
                            X.StartMember<Setter>(c => c.Value),
                                X.Value("Value"),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject()
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void LookAheadTest()
        {
            var look = new List<XamlNode>
            {
                X.StartObject<Style>(),
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
                X.EndObject(),
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = Skimmer.LookAhead(enumerator).Count();
            Assert.AreEqual(8, count);
        }

        [TestMethod]
        public void LookAheadTestStartZero()
        {
            var look = new List<XamlNode>();

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = Skimmer.LookAhead(enumerator).Count();
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void LookAheadTestStartMiniumLength()
        {
            var look = new List<XamlNode>
            {
                X.StartObject<Style>(),                
                X.EndObject()
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = Skimmer.LookAhead(enumerator).Count();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void LookAheadTest10()
        {
            var look = new List<XamlNode>
            {
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = Skimmer.LookAhead(enumerator).Count();
            Assert.AreEqual(10, count);
        }
    }
}
