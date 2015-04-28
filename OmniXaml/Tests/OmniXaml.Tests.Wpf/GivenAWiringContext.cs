namespace OmniXaml.Tests.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class GivenAWiringContext
    {
        public WiringContext WiringContext
        {
            get
            {
                var windowType = typeof(Window);
                var textBlockType = typeof(TextBlock);
                var toggleButtonType = typeof(ToggleButton);

                var context = new WiringContextBuilder().WithNsPrefix("", "root")
                    .WithXamlNs("root", windowType.Assembly, windowType.Namespace)
                    .WithXamlNs("root", textBlockType.Assembly, textBlockType.Namespace)
                    .WithXamlNs("root", toggleButtonType.Assembly, toggleButtonType.Namespace)
                    .Build();

                return context;
            }
        }
    }
}