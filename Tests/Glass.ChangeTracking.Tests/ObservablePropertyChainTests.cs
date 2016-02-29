namespace Glass.ChangeTracking.Tests
{
    using Xunit;
    using System;

    public class ObservablePropertyChainTests
    {
        [Fact]
        public void SubscriptionToOneLevelPath()
        {
            var changeSource = new DummyViewModel();
            var sut = new ObservablePropertyChain(changeSource, "Text");
            object actualText = null;

            sut.Subscribe(o => actualText = o);
            changeSource.Text = "Hello world";

            Assert.Equal("Hello world", actualText);
        }

        [Fact]
        public void SubscriptionToTwoLevelPath()
        {
            var changeSource = new DummyViewModel { Child = new DummyViewModel() };
            var sut = new ObservablePropertyChain(changeSource, "Child.Text");
            object actualText = null;

            sut.Subscribe(o => actualText = o);
            changeSource.Child.Text = "Hello world";

            Assert.Equal("Hello world", actualText);
        }

        [Fact]
        public void GivenSubscriptionToTwoLevelPath_WhenRootChanges_NotificationsShouldArrive()
        {
            var changeSource = new DummyViewModel { Child = new DummyViewModel { Text = "Old text" } };
            var sut = new ObservablePropertyChain(changeSource, "Child.Text");
            object actualText = null;

            sut.Subscribe(o => actualText = o);
            changeSource.Child = new DummyViewModel { Text = "This is the real thing" };

            Assert.Equal("This is the real thing", actualText);
        }

        [Fact]
        public void GivenSubscriptionToTwoLevelPath_WhenRootChangesButValuesAreSame_NoNotificationAreReceived()
        {
            var changeSource = new DummyViewModel { Child = new DummyViewModel { Text = "Same" } };
            var sut = new ObservablePropertyChain(changeSource, "Child.Text");

            var hit = false;
            sut.Subscribe(o => hit = true);
            changeSource.Child = new DummyViewModel { Text = "Same" };

            Assert.False(hit);
        }

        [Fact]
        public void GivenSubscriptionToTwoLevelPath_WhenRootChangesInDifferentSteps_NotificationsShouldArrive()
        {
            var changeSource = new DummyViewModel { Child = new DummyViewModel { Text = "Old text" } };
            var sut = new ObservablePropertyChain(changeSource, "Child.Text");
            object actualText = null;

            sut.Subscribe(o => actualText = o);
            changeSource.Child = new DummyViewModel();
            changeSource.Child.Text = "This is the real thing";

            Assert.Equal("This is the real thing", actualText);
        }

        [Fact]
        public void GivenSubscriptionToTwoLevelPathToStruct_RetrievesTheCorrectValue()
        {
            var changeSource = new DummyViewModel { Child = new DummyViewModel { MyStruct = new MyStruct { Text = "My text" } } };
            var sut = new ObservablePropertyChain(changeSource, "Child.MyStruct.Text");

            Assert.Equal("My text", sut.Value);
        }

        [Fact]
        public void ValueTypeChain_WithOneLevel_ValueIsAccessed()
        {
            var root = new MyStruct { Text = "Hello" };
            var sut = new ValueTypePropertyChain(root, "Text");

            Assert.Equal("Hello", sut.Value);
        }

        [Fact]
        public void ValueTypeChain_WithTwoLevels_ValueIsAccessed()
        {
            var root = new MyStruct { Child = new ChildStruct { Text = "Some text" } };
            var sut = new ValueTypePropertyChain(root, "Child.Text");

            Assert.Equal("Some text", sut.Value);
        }
    }
}
