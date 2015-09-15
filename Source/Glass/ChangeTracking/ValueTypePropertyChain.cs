namespace Glass.ChangeTracking
{
    public class ValueTypePropertyChain : PropertyChain
    {
        public ValueTypePropertyChain(object instance, string path) : base(path)
        {
            Property = new Property(instance, PropertyName);

            if (SubPath.Length > 0)
            {
                Child = new ValueTypePropertyChain(Value, SubPath);
            }
        }
    }
}