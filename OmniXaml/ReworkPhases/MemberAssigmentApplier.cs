namespace OmniXaml.ReworkPhases
{
    using System.Linq;
    using Zafiro.Core;

    public class MemberAssigmentApplier
    {
        private readonly IStringSourceValueConverter converter;

        public MemberAssigmentApplier(IStringSourceValueConverter converter)
        {
            this.converter = converter;
        }

        public bool TryApply(InflatedMemberAssignment inflatedAssignment, object instance)
        {
            if (inflatedAssignment.Member.MemberType.IsCollection())
            {
                return AssignCollection(inflatedAssignment, instance);
            }
            else
            {
                return AssignSingleValue(inflatedAssignment, instance);
            }
        }

        private bool AssignSingleValue(InflatedMemberAssignment inflatedAssignment, object instance)
        {
            var value = (string) inflatedAssignment.Children.First().Instance;
            var conversion = converter.TryConvert(value, inflatedAssignment.Member.MemberType);
            if (conversion.Item1)
            {
                inflatedAssignment.Member.SetValue(instance, conversion.Item2);
                return true;
            }

            return false;
        }

        private bool AssignCollection(InflatedMemberAssignment inflatedAssignment, object instance)
        {
            var parent = inflatedAssignment.Member.GetValue(instance);
            var children = from n in inflatedAssignment.Children select n.Instance;
            foreach (var child in children)
            {
                Collection.UniversalAdd(parent, child);
            }
            return true;
        }
    }
}