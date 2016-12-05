namespace OmniXaml.Tests.Model
{
    using System;

    internal class TestWindow : Window
    {
        internal bool ButtonClicked { get; private set; } = false;
        internal bool WindowLoaded { get; private set; } = false;

        internal void OnClick(object sender, EventArgs args)
        {
            ButtonClicked = true;
        }

        internal void OnLoad(EventArgs args)
        {
            WindowLoaded = true;
        }

        public event EventHandler Clicked;
    }
}