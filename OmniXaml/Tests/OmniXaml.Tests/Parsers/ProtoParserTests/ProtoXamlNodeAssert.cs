namespace OmniXaml.Tests.Parsers.ProtoParserTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal static class ProtoXamlNodeAssert
    {
        public static void AreEqualWithLooseXamlTypeComparison(ICollection<ProtoXamlNode> expectedStates, ICollection<ProtoXamlNode> actualStates)
        {
            CollectionAssert.AreEqual(
                expectedStates.ToList(),
                actualStates.ToList(),
                new ProtoXamlNodeComparer { CheckNameOnlyForXamlTypes = true });
        }

        private static void AreEqual(ICollection<ProtoXamlNode> expectedStates, ICollection<ProtoXamlNode> actualStates)
        {
            CollectionAssert.AreEqual(
                expectedStates.ToList(),
                actualStates.ToList(),
                new ProtoXamlNodeComparer { CheckNameOnlyForXamlTypes = false });
        }

        private class ProtoXamlNodeComparer : IComparer
        {
            private bool checkNameOnlyForXamlTypes = false;

            public bool CheckNameOnlyForXamlTypes
            {
                get
                {
                    return checkNameOnlyForXamlTypes;
                }
                set
                {
                    checkNameOnlyForXamlTypes = value;
                }
            }

            public int Compare(object x, object y)
            {
                var stateA = x as ProtoXamlNode;
                var stateB = y as ProtoXamlNode;

                if (stateA == null || stateB == null)
                {
                    return -1;
                }

                var typesAreEqual = stateA.XamlType == stateB.XamlType;
                if (stateA.XamlType != null && stateB.XamlType != null)
                {
                    typesAreEqual = checkNameOnlyForXamlTypes
                                        ? stateA.XamlType.Name == stateB.XamlType.Name
                                        : stateA.XamlType == stateB.XamlType;
                }

                var equal = stateA.Namespace == stateB.Namespace && stateA.NodeType == stateB.NodeType && typesAreEqual;
                return equal ? 0 : -1;
            }
        }
    }
}