namespace OmniXaml.Wpf.Tests
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Xaml.Tests.Resources;
    using Xunit;

    public class LoadingTests : GivenAXamlXmlLoader
    {
        [StaFact]
        public void Window()
        {
            var windowType = typeof(Window);           

            var actualInstance = LoadXaml(File.LoadAsString(@"Xaml\Wpf\Window.xaml"));
            Assert.IsType(windowType, actualInstance);
        }

        [StaFact]
        public void WindowWithContent()
        {
            var windowType = typeof(Window);
            var textBlockType = typeof(TextBlock);

            var actualInstance = LoadXaml(File.LoadAsString(@"Xaml\Wpf\WindowWithContent.xaml"));
            Assert.IsType(windowType, actualInstance);
            var window = (Window)actualInstance;
            Assert.IsType(textBlockType, window.Content);
            var textBlock = (TextBlock)window.Content;
            Assert.Equal("Saludos cordiales!", textBlock.Text);
        }

        [StaFact]
        public void DataTemplate()
        {
            var visualTree = LoadXaml(File.LoadAsString(@"Xaml\Wpf\DataTemplate.xaml"));
        }

        [StaFact]
        public void ShowCase()
        {
            var visualTree = LoadXaml(File.LoadAsString(@"Xaml\Wpf\ShowCase.xaml"));
        }

        [StaFact]
        public void MicroShowCase()
        {
            var visualTree = LoadXaml(File.LoadAsString(@"Xaml\Wpf\MicroShowCase.xaml"));
        }

        [StaFact]
        public void Stage1()
        {
            var visualTree = LoadXaml(File.LoadAsString(@"Xaml\Wpf\Stage1.xaml"));
        }

        [StaFact]
        public void Stage2()
        {
            var visualTree = LoadXaml(File.LoadAsString(@"Xaml\Wpf\Stage2.xaml"));
        }

        [StaFact]
        public void Stage3()
        {
            var visualTree = LoadXaml(File.LoadAsString(@"Xaml\Wpf\Stage3.xaml"));
        }

        [StaFact]
        public void ColorResource()
        {
            var visualTree = (Window)LoadXaml(File.LoadAsString(@"Xaml\Wpf\ColorResource.xaml"));
            var color = (Color)visualTree.FindResource("color");
            Assert.Equal(0xff, color.A);
            Assert.Equal(0x80, color.R);
            Assert.Equal(0x80, color.G);
            Assert.Equal(0x80, color.B);
        }

        [StaFact]
        public void SolidColorBrushResource()
        {
            var visualTree = (Window)LoadXaml(File.LoadAsString(@"Xaml\Wpf\SolidColorBrushResource.xaml"));
            var brush = (SolidColorBrush)visualTree.FindResource("brush");
            var color = brush.Color;
            Assert.Equal(0xff, color.A);
            Assert.Equal(0x80, color.R);
            Assert.Equal(0x80, color.G);
            Assert.Equal(0x80, color.B);
        }
    }
}
