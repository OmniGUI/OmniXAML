namespace Glass.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

            var actual = DependencySorter.SortDependencies(new Collection<Node> { a, b, c, d, e }).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleTest()
        {
            var prop = new Node("Property");
            var val = new Node("Value");

            val.Dependencies = new Collection<Node> { prop };

            var expected = new List<Node> { prop, val };

            var actual = DependencySorter.SortDependencies(new Collection<Node> { prop, val }).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AlreadySorted()
        {
            var a = new Node("A");
            var b = new Node("B");
            var c = new Node("C");
            var d = new Node("D");
            var e = new Node("E");

            a.Dependencies = new Collection<Node> { };
            b.Dependencies = new Collection<Node> { a };
            c.Dependencies = new Collection<Node> { b };
            d.Dependencies = new Collection<Node> { c };
            e.Dependencies = new Collection<Node> { d };

            var expected = new List<Node> { a, b, c, d, e };

            var actual = DependencySorter.SortDependencies(new Collection<Node> { a, b, c, d, e }).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PartiallySorted()
        {
            var a = new Node("A");
            var b = new Node("B");
            var c = new Node("C");
            var d = new Node("D");
            var e = new Node("E");

            a.Dependencies = new Collection<Node> { };
            b.Dependencies = new Collection<Node> { a };
            c.Dependencies = new Collection<Node> { b };
            d.Dependencies = new Collection<Node> { e };
            e.Dependencies = new Collection<Node> {  };

            var expected = new List<Node> { a, b, c, e, d };

            var actual = DependencySorter.SortDependencies(new Collection<Node> { a, b, c, d, e }).ToList();
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

            DependencySorter.SortDependencies(new Collection<Node> { a, b, c, d, e });
        }
    }
}
