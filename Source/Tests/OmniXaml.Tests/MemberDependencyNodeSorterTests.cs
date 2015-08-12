namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MemberDependencyNodeSorterTests : GivenAWiringContextWithNodeBuilders
    {
        private readonly MemberDependencyNodeSorter memberDependencyNodeSorter = new MemberDependencyNodeSorter();

        [TestMethod]
        public void Sort()
        {
            var input = new List<XamlInstruction>
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

            var actualNodes = memberDependencyNodeSorter.Sort(input.GetEnumerator()).ToList();
            var expectedInstructions = new List<XamlInstruction>
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

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }       
    }
}
