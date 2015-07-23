namespace XamlViewer
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using OmniXaml.Visualization;

    public class NodeTypeToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            var relativePath = parameter == null ? "" : RemoveEndingSlash((string)parameter);

            var nodeType = (NodeType)value;
            switch (nodeType)
            {
                case NodeType.Root:
                    return new Uri(CombineRelative(relativePath, "root.png"), UriKind.Relative);
                case NodeType.Value:
                    return new Uri(CombineRelative(relativePath, "value.png"), UriKind.Relative);
                case NodeType.NamespaceDeclaration:
                    return new Uri(CombineRelative(relativePath, "namespace.png"), UriKind.Relative);
                case NodeType.GetObject:
                    return new Uri(CombineRelative(relativePath, "collection.png"), UriKind.Relative);
                case NodeType.Object:
                    return new Uri(CombineRelative(relativePath, "object.png"), UriKind.Relative);
                case NodeType.Member:
                    return new Uri(CombineRelative(relativePath, "member.png"), UriKind.Relative);
            }

            throw new InvalidOperationException($"Cannot find a matching Uri for the type {nodeType}");
        }

        private static string RemoveEndingSlash(string str)
        {
            return str.EndsWith("/") || str.EndsWith("\\") ? str.Remove(str.Length-1, 1) : str;
        }

        private static string CombineRelative(string relativePath, string fileName)
        {
            return relativePath + "/" + fileName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}