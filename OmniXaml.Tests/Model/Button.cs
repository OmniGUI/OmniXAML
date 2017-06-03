using System;

namespace OmniXaml.Tests.Model
{
    public class Button : ModelObject
    {
        public Button()
        {
            
        }

        public event EventHandler Click;

        public void ClickButton()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public object Content { get; set; }
    }
}
