namespace OmniXaml.ReworkPhases
{
    using System.Linq;
    using Rework;
    using Zafiro.Core;

    public interface IMemberAssigmentApplier
    {
        bool TryApply(InflatedMemberAssignment inflatedAssignment, object instance);
    }

    public class MemberAssigmentApplier : IMemberAssigmentApplier
    {
        private readonly IStringSourceValueConverter converter;
        private readonly IValuePipeline pipeline;

        public MemberAssigmentApplier(IStringSourceValueConverter converter, IValuePipeline pipeline)
        {
            this.converter = converter;
            this.pipeline = pipeline;
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
            var value = inflatedAssignment.Children.First().Instance;

            if (value is string)
            {
                var conversion = converter.TryConvert((string)value, inflatedAssignment.Member.MemberType);
                if (conversion.Item1)
                {
                    SetMember(instance, inflatedAssignment.Member, conversion.Item2);
                    return true;
                }

                return false;
            }
            else
            {
                SetMember(instance, inflatedAssignment.Member, value);
                return true;
            }
        }

        private void SetMember(object parent, Member member, object value)
        {
            var mutableUnit = new MutablePipelineUnit(value);
            pipeline.Handle(parent, member, mutableUnit);
            if (mutableUnit.Handled)
            {
                return;
            }

            member.SetValue(parent, mutableUnit.Value);
        }

        private static bool AssignCollection(InflatedMemberAssignment inflatedAssignment, object instance)
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