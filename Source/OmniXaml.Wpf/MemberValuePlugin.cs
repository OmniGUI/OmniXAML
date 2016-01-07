namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using OmniXaml.ObjectAssembler;
    using Typing;

    public class MemberValuePlugin : Typing.MemberValuePlugin
    {
        private readonly MutableMember member;

        public MemberValuePlugin(MutableMember member) : base(member)
        {
            this.member = member;
        }

        public override void SetValue(object instance, object value)
        {
            if (member.Name == "Value" && instance is Setter)
            {
                var setter = (Setter) instance;                
                var targetType = setter.Property.PropertyType;
                var valuePipeline = new ValuePipeline(member.TypeRepository, null);
                var xamlType = member.TypeRepository.GetByType(targetType);
                base.SetValue(instance, valuePipeline.ConvertValueIfNecessary(value, xamlType));
            }
            else
            {
                if (!TrySetDependencyProperty(instance, value))
                {
                    base.SetValue(instance, value);
                }
            }
        }

        private bool TrySetDependencyProperty(object instance, object value)
        {
            var dp = GetDependencyProperty(instance.GetType(), member.Name + "Property");
            if (dp == null)
            {
                return false;
            }

            var dependencyObject = (DependencyObject) instance;
            if (value is BindingExpression)
            {
                dependencyObject.SetValue(dp, value);
            }
            else
            {
                dependencyObject.SetValue(dp, value);
            }
            return true;
        }

        private static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            FieldInfo fieldInfo = type.GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty) fieldInfo?.GetValue(null);
        }
    }
}