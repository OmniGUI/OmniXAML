namespace Glass.Core
{
    public interface IAdd<in T>
    {
        void Add(T item);
    }
}