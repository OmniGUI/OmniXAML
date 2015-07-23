namespace XamlViewer
{
    using System;
    using System.Windows;

    public static class Execute
    {
        public static Action Safely(Action action)
        {
            return () =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    ShowErrorMessage(e);
                }
            };
        }

        private static void ShowErrorMessage(Exception e)
        {
            MessageBox.Show($"Something went wrong!\n\nException: {e}");
        }

        public static Action<object> Safely(Action<object> action)
        {
            return a =>
            {
                try
                {
                    action(a);
                }
                catch (Exception e)
                {
                    ShowErrorMessage(e);
                }
            };
        }
    }
}