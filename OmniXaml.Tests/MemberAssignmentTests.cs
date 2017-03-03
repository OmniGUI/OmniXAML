namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using Model;
    using Xunit;

    public class MemberAssignmentTests
    {
        [Fact]
        public void AttachedPropertyAssignment()
        {
            ICollection<VisualStateGroup> groups = new List<VisualStateGroup>();
            var instance = new Window();
            var sut = new Assignment(new KeyedInstance(instance), Member.FromAttached<VisualStateManager>("VisualStateGroups"), groups);
            sut.ExecuteAssignment();

            Assert.IsAssignableFrom<ICollection<VisualStateGroup>>(VisualStateManager.GetVisualStateGroups(instance));
        }

        [Fact]
        public void StandardPropertyAssignment()
        {
            var instance = new TextBlock();
            var sut = new Assignment(new KeyedInstance(instance), Member.FromStandard<TextBlock>(tb => tb.Text), "Some text");
            sut.ExecuteAssignment();

            Assert.Equal("Some text", instance.Text);
        }
    }
}