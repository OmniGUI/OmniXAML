using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OmniXaml
{
    public static class NodeExtensions
    {
        public static IEnumerable<ConstructionNode> GetAllChildren(this ConstructionNode node)
        {
            return node.Children.Concat(node.Assignments.SelectMany(assignment => assignment.Values));
        }

        public static ConstructionNode WithAssignments(this ConstructionNode node, IEnumerable<MemberAssignment> assignment)
        {
            foreach (var ass in assignment)
            {
                node.Assignments.Add(ass);
            }

            return node;
        }

        public static ConstructionNode WithAssignment<T>(this ConstructionNode<T> node, Expression<Func<T, object>> selector, string value)
        {
            node.Assignments.Add(new MemberAssignment<T>(selector, value));

            return node;
        }

        public static ConstructionNode WithAssignment<TParent, TSub>(this ConstructionNode<TParent, TSub> node, Expression<Func<TSub, object>> selector, string value) where TSub : TParent
        {
            node.Assignments.Add(new MemberAssignment<TSub>(selector, value));

            return node;
        }

        public static ConstructionNode WithAssignment<T>(this ConstructionNode<T> node, string memberName, string value)
        {
            node.Assignments.Add(new MemberAssignment<T>(memberName, value));

            return node;
        }

        public static ConstructionNode WithAssignment<TParent, TSub>(this ConstructionNode<TParent, TSub> node, string memberName, string value) where TSub : TParent
        {
            node.Assignments.Add(new MemberAssignment<TSub>(memberName, value));

            return node;
        }

        public static ConstructionNode WithAssignments<T>(this ConstructionNode<T> node, params (Expression<Func<T, object>>, string)[] assignments)
        {
            foreach (var ass in assignments)
            {
                node.Assignments.Add(new MemberAssignment<T>(ass.Item1, ass.Item2));
            }

            return node;
        }

        public static ConstructionNode WithChildren(this ConstructionNode node, IEnumerable<ConstructionNode> children)
        {
            foreach (var ass in children)
            {
                node.Children.Add(ass);
            }

            return node;
        }        
    }
}