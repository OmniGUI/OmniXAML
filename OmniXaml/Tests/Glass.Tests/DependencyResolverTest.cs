namespace Glass.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyResolverTest
    {
        [TestMethod]
        public void SortTest()
        {
            var a = new Node("A");
            var b = new Node("B");
            var c = new Node("C");
            var d = new Node("D");
            var e = new Node("E");

            a.Dependencies = new Collection<Node> { b, d };
            b.Dependencies = new Collection<Node> { c, e };
            c.Dependencies = new Collection<Node> { d, e };

            var expected = new List<Node> { d, e, c, b, a };

            var actual = DependencySorter.Sort(a).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CircularDependency()
        {
            var a = new Node("A");
            var b = new Node("B");
            var c = new Node("C");
            var d = new Node("D");
            var e = new Node("E");

            a.Dependencies = new Collection<Node> { b, d };
            b.Dependencies = new Collection<Node> { c, e };
            c.Dependencies = new Collection<Node> { d, e };
            e.Dependencies = new Collection<Node> { a };

            DependencySorter.Sort(a);            
        }
    }
}
