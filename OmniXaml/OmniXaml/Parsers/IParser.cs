namespace OmniXaml.Parsers.ProtoParser
{
    public interface IParser<in TInput, out TOutput>
    {
        TOutput Parse(TInput input);
    }
}