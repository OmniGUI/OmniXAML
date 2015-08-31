namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal static class XamlNodesAssert
    {
        public static void AreEssentiallyTheSame(ICollection<XamlInstruction> expectedInstructions, ICollection<XamlInstruction> actualNodes)
        {
            CollectionAssert.AreEqual(
                expectedInstructions.ToList(),
                actualNodes.ToList(),
                new XamlNodeComparer { CheckNameOnlyForXamlTypes = true });
        }

        private class XamlNodeComparer : IComparer
        {
            private bool checkNameOnlyForXamlTypes;

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
                var nodeA = (XamlInstruction)x;
                var nodeB = (XamlInstruction)y;

                var areXamlTypesConsideredEqual = nodeA.XamlType == nodeB.XamlType;
                if (nodeA.XamlType != null && nodeB.XamlType != null)
                {
                    areXamlTypesConsideredEqual = checkNameOnlyForXamlTypes
                        ? nodeA.XamlType.Name == nodeB.XamlType.Name
                        : nodeA.XamlType == nodeB.XamlType;
                }

                var areMembersConsideredEqual = nodeA.Member == nodeB.Member;
                if (nodeA.Member != null && nodeB.Member != null)
                {
                    areMembersConsideredEqual = checkNameOnlyForXamlTypes
                        ? nodeA.Member.Name == nodeB.Member.Name
                        : nodeA.Member == nodeB.Member;
                }

                var areNamespacesEqual = nodeA.NamespaceDeclaration == nodeB.NamespaceDeclaration;
                if (nodeA.NamespaceDeclaration != null && nodeB.NamespaceDeclaration != null)
                {
                    areNamespacesEqual = nodeA.NamespaceDeclaration.Namespace == nodeB.NamespaceDeclaration.Namespace
                                         && nodeA.NamespaceDeclaration.Prefix == nodeB.NamespaceDeclaration.Prefix;
                }

                var equal = nodeA.InstructionType == nodeB.InstructionType &&
                        areMembersConsideredEqual && areNamespacesEqual &&
                        areXamlTypesConsideredEqual;

                return equal ? 0 : -1;
            }
        }
    }
}