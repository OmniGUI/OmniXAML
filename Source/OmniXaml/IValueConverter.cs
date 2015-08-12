namespace OmniXaml
{
    // ReSharper disable once UnusedMember.Global
    public interface IValueConverter<TFirst, TSecond>
    {
        // ReSharper disable once UnusedMember.Global
        TSecond Convert(TFirst first);
        // ReSharper disable once UnusedMember.Global
        TFirst Convert(TSecond first);
    }
}