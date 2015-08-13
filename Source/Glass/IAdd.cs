namespace OmniXaml
{
    public interface IAdd<in T>
    {
        void Add(T item);
    }
}