namespace Glass
{
    public interface IAdd<in T>
    {
        void Add(T item);
    }
}