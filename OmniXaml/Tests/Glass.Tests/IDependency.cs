namespace Glass.Tests
{
    using System.Collections.Generic;

    public interface IDependency<T>
    {
        IEnumerable<T> Edges { get; set; }
    }
}