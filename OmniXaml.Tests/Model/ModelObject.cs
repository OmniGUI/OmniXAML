namespace OmniXaml.Tests.Model
{
    using DefaultLoader;
    using System;
    using System.Collections.Generic;

    public class ModelObject
    {
        [Name]
        public string Name { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }

        private Dictionary<object, Action<EventArgs>> handlers = new Dictionary<object, Action<EventArgs>>();

        public void AddHandler(object routedEvent, Action<EventArgs> handler, bool handledEventsToo = false)
        {
            if(handlers.ContainsKey(routedEvent))
            {
                handlers[routedEvent] += handler;
            }
            else
            {
                handlers.Add(routedEvent, handler);
            }
        }

        public void RaiseEvent(object routedEvent, EventArgs args)
        {
            if (handlers.ContainsKey(routedEvent))
            {
                handlers[routedEvent]?.Invoke(args);
            }
        }

        protected bool Equals(ModelObject other)
        {
            return string.Equals(Name, other.Name) && Height.Equals(other.Height) && Width.Equals(other.Width);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ModelObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Height.GetHashCode();
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                return hashCode;
            }
        }
    }
}