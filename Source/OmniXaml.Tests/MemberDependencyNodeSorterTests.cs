namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common.DotNetFx;
    using Xunit;
    using Resources;

    public class MemberDependencyNodeSorterTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly MemberDependencyNodeSorter memberDependencyNodeSorter = new MemberDependencyNodeSorter();
        private readonly InstructionResources resources;

        public MemberDependencyNodeSorterTests()
        {
            resources = new InstructionResources(this);
        }

        [Fact]
        public void SortWholeStyle()
        {
            var input = resources.StyleUnsorted;

            var enumerator = new EnumeratorDebugWrapper<Instruction>(input.GetEnumerator());
            var actualNodes = memberDependencyNodeSorter.Sort(enumerator).ToList();
            var expectedInstructions = resources.StyleSorted;

            Assert.Equal(expectedInstructions, actualNodes);
        }


        [Fact]
        public void SortSetter()
        {
            var input = resources.SetterUnsorted;

            var enumerator = new EnumeratorDebugWrapper<Instruction>(input.GetEnumerator());
            var actualNodes = memberDependencyNodeSorter.Sort(enumerator).ToList();
            var expectedInstructions = resources.SetterSorted;

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void SortComboBox()
        {
            var input = resources.ComboBoxUnsorted;

            var enumerator = new EnumeratorDebugWrapper<Instruction>(input.GetEnumerator());
            var actualNodes = memberDependencyNodeSorter.Sort(enumerator).ToList();
            var expectedInstructions = resources.ComboBoxSorted;

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void SortTwoComboBoxes()
        {
            var input = resources.TwoComboBoxesUnsorted;

            var enumerator = new EnumeratorDebugWrapper<Instruction>(input.GetEnumerator());
            var actualNodes = memberDependencyNodeSorter.Sort(enumerator).ToList();
            var expectedInstructions = resources.TwoComboBoxesSorted;

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void SortMarkupExtension()
        {
            var input = resources.ListBoxSortedWithExtension;

            var enumerator = new EnumeratorDebugWrapper<Instruction>(input.GetEnumerator());
            var actualNodes = memberDependencyNodeSorter.Sort(enumerator).ToList();
            var expectedInstructions = resources.ListBoxSortedWithExtension;

            Assert.Equal(expectedInstructions, actualNodes);
        }
    }

    public class EnumeratorDebugWrapper<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> getEnumerator;

        public EnumeratorDebugWrapper(IEnumerator<T> getEnumerator)
        {
            this.getEnumerator = getEnumerator;
        }

        public void Dispose()
        {
            getEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            return getEnumerator.MoveNext();
        }

        public void Reset()
        {
            getEnumerator.Reset();
        }

        public T Current => getEnumerator.Current;

        object IEnumerator.Current => Current;
    }
}
