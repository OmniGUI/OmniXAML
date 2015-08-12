namespace XamlViewer
{
    using System.Collections;

    public interface IXamlSnippetProvider
    {
        IList Snippets { get; }
    }
}