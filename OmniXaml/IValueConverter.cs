namespace OmniXaml
{
    public interface IValueConverter<TFirst, TSecond>
    {
        TSecond Convert(TFirst first);
        TFirst Convert(TSecond first);
    }
}