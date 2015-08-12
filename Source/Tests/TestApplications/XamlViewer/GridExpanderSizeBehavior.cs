namespace XamlViewer
{
    using System.Windows;
    using System.Windows.Controls;

    public class GridExpanderSizeBehavior
    {
        public static readonly DependencyProperty SizeRowsToExpanderStateProperty =
            DependencyProperty.RegisterAttached(
                "SizeRowsToExpanderState",
                typeof(bool),
                typeof(GridExpanderSizeBehavior),
                new FrameworkPropertyMetadata(false, SizeRowsToExpanderStateChanged));

        public static void SetSizeRowsToExpanderState(Grid grid, bool value)
        {
            grid.SetValue(SizeRowsToExpanderStateProperty, value);
        }

        private static void SizeRowsToExpanderStateChanged(object target, DependencyPropertyChangedEventArgs e)
        {
            Grid grid = target as Grid;
            if (grid != null)
            {
                if ((bool)e.NewValue == true)
                {
                    grid.AddHandler(Expander.ExpandedEvent, new RoutedEventHandler(Expander_Expanded));
                    grid.AddHandler(Expander.CollapsedEvent, new RoutedEventHandler(Expander_Collapsed));
                }
                else if ((bool)e.OldValue == true)
                {
                    grid.RemoveHandler(Expander.ExpandedEvent, new RoutedEventHandler(Expander_Expanded));
                    grid.RemoveHandler(Expander.CollapsedEvent, new RoutedEventHandler(Expander_Collapsed));
                }
            }
        }

        private static void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            Expander expander = e.OriginalSource as Expander;
            int row = Grid.GetRow(expander);
            if (row <= grid.RowDefinitions.Count)
            {
                grid.RowDefinitions[row].Height = new GridLength(1.0, GridUnitType.Star);
            }
        }

        private static void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            Expander expander = e.OriginalSource as Expander;
            int row = Grid.GetRow(expander);
            if (row <= grid.RowDefinitions.Count)
            {
                grid.RowDefinitions[row].Height = new GridLength(1.0, GridUnitType.Auto);
            }
        }
    }
}