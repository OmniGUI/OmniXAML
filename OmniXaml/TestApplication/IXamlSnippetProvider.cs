namespace TestApplication
{
    using System.Collections;

    public interface IXamlSnippetProvider
    {
        IList Snippets { get; }
    }
}