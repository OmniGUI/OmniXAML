namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Subjects;
    using Ambient;

    public class BuildContext
    {
        private readonly HashSet<ParentChildRelationship> associations = new HashSet<ParentChildRelationship>();
        private readonly ISubject<ParentChildRelationship> childAssociated = new Subject<ParentChildRelationship>();

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

        public void AddAssociation(ParentChildRelationship pendingAssociation)
        {
            associations.Add(pendingAssociation);
        }

        public IObservable<ParentChildRelationship> ChildAssociated => childAssociated;

        public IEnumerable<ParentChildRelationship> Associations => new ReadOnlyCollection<ParentChildRelationship>(associations.ToList());
    }
}