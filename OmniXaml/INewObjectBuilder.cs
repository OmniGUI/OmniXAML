using System.Reactive.Subjects;
using OmniXaml.Rework;

namespace OmniXaml
{
    public interface INewObjectBuilder
    {
        object Inflate(ConstructionNode constructionNode);
        ISubject<NodeInflation> NodeInflated { get; }
    }
}