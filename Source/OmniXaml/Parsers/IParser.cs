namespace OmniXaml.Parsers
{
    public interface IParser<in TInput, out TOutput>
    {
        TOutput Parse(TInput input);
    }
}