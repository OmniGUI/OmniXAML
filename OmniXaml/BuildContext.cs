namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Subjects;
    using Ambient;
    using Zafiro.Core;

    public class BuildContext : IBuildContext
    {
        private readonly HashSet<Association> associations = new HashSet<Association>();
        private readonly ISubject<Association> childAssociated = new Subject<Association>();

        public BuildContext(INamescopeAnnotator namescopeAnnotator, IAmbientRegistrator ambientRegistrator, IInstanceLifecycleSignaler instanceLifecycleSignaler)
        {
            NamescopeAnnotator = namescopeAnnotator;
            AmbientRegistrator = ambientRegistrator;
            InstanceLifecycleSignaler = instanceLifecycleSignaler;
        }

        public INamescopeAnnotator NamescopeAnnotator { get; }
        public IPrefixAnnotator PrefixAnnotator { get; set; }

        public IAmbientRegistrator AmbientRegistrator { get; }
        public IInstanceLifecycleSignaler InstanceLifecycleSignaler { get; }
        public IDictionary<string, object> Bag { get; set; } = new Dictionary<string, object>();
        public ConstructionNode CurrentNode { get; set; }
        public IPrefixedTypeResolver PrefixedTypeResolver { get; set; }
        public ConstructionNode Root { get; set; }

        public void AddAssociation(Association pendingAssociation)
        {
            associations.Add(pendingAssociation);
            childAssociated.OnNext(pendingAssociation);
        }

        public IObservable<Association> ChildAssociated => childAssociated;

        public IEnumerable<Association> Associations => new ReadOnlyCollection<Association>(associations.ToList());
    }
}