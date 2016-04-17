namespace XamlViewer.ViewModels
{
    using System.Collections.Generic;

    public class XmlSnippetProvider : IXamlSnippetProvider
    {
        public IEnumerable<Snippet> Snippets { get; }
    }
}