using System;
using System.Linq.Expressions;
using Zafiro.Core;

namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;

    public class MemberAssignment : IChild<ConstructionNode>
    {
        public Member Member { get; set; }
        public string SourceValue { get; set; }
        public IEnumerable<ConstructionNode> Values { get; set; } = new List<ConstructionNode>();

        public override string ToString()
        {
            if (SourceValue != null)
            {
                return $@"{Member} = ""{SourceValue}""";
            }
            else
            {
                var formattedChildren = string.Join(", ", Values);
                return $"{Member} = {formattedChildren}";
            }
        }

        protected bool Equals(MemberAssignment other)
        {
            return Equals(Member, other.Member) && string.Equals(SourceValue, other.SourceValue) && Enumerable.SequenceEqual(Values, other.Values);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return Equals((MemberAssignment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Member != null ? Member.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SourceValue != null ? SourceValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Values != null ? Values.GetHashCode() : 0);
                return hashCode;
            }
        }

        public ConstructionNode Parent { get; set; }
    }

    public class MemberAssignment<T> : MemberAssignment
    {
        public MemberAssignment(Expression<Func<T, object>> selector, string value)
        {
            Member = Member.FromStandard(selector);
            SourceValue = value;
        }

        public MemberAssignment(string memberName, string value)
        {
            Member = Member.FromStandard<T>(memberName);
            SourceValue = value;
        }

        public MemberAssignment(Expression<Func<T, object>> selector, ConstructionNode childNode)
        {
            Member = Member.FromStandard(selector);
            Values = new List<ConstructionNode> { childNode };
        }
    }
}