namespace Glass.Core
{
    using System.Collections.Generic;

    public interface IDependency<out T>
    {
        IEnumerable<T> Dependencies { get; }
    }
}