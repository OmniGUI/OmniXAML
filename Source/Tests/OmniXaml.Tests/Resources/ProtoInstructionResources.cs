namespace OmniXaml.Tests.Resources
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using Typing;

    internal class ProtoInstructionResources
    {
        public ProtoInstructionResources(GivenAWiringContextWithNodeBuilders context)
        {
            RootNs = context.RootNs;
            AnotherNs = context.AnotherNs;
            P = context.P;
        }

        private NamespaceDeclaration RootNs { get; }
        public NamespaceDeclaration AnotherNs { get; }

        private ProtoInstructionBuilder P { get; }

        public IEnumerable<ProtoXamlInstruction> ContentPropertyNesting
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main1", RootNs.Prefix),
                    P.Text(),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main2", RootNs.Prefix),
                    P.Text(),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.NonEmptyElement(typeof (Item), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Item1", RootNs.Prefix),
                    P.Text(),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Item2", RootNs.Prefix),
                    P.Text(),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Item3", RootNs.Prefix),
                    P.Text(),
                    P.EndTag(),
                    P.Text(),
                    P.NonEmptyPropertyElement<ChildClass>(c => c.Child, RootNs),
                    P.EmptyElement<ChildClass>(RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> TwoNestedProperties
        {
            get
            {
                var input = new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main1", RootNs.Prefix),
                    P.Text(),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main2", RootNs.Prefix),
                    P.Text(),
                    P.EndTag(),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
                return input;
            }
        }

        public IEnumerable<ProtoXamlInstruction> ImplicitContentPropertyWithImplicityCollection
        {
            get
            {
                var input = new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.NonEmptyElement(typeof (Item), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Item1", RootNs.Prefix),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };

                return input;
            }
        }

        public IEnumerable<ProtoXamlInstruction> TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem
        {
            get
            {
                var input = new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.NonEmptyElement(typeof (Item), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Item1", RootNs.Prefix),
                    P.Text(),
                    P.EndTag(),
                    P.Text(),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.EmptyElement(typeof (ChildClass), RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
                return input;
            }
        }

        public IEnumerable<ProtoXamlInstruction> CollapsedTag
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement(typeof(DummyClass), RootNs),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> FourLevelsOfNesting
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof(ChildClass), RootNs),
                    P.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof(ChildClass), RootNs),
                    P.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                    P.EmptyElement(typeof(ChildClass), RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> TwoNestedPropertiesEmpty
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                    P.EndTag(),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> GetString(NamespaceDeclaration sysNs)
        {
            return new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(sysNs),
                P.NonEmptyElement(typeof (string), sysNs),
                P.Text("Text"),
                P.EndTag(),
            };
        }

        public IEnumerable<ProtoXamlInstruction> ContentPropertyForCollectionOneElement
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.EmptyElement(typeof(Item), RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ThreeLevelsOfNesting
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                    P.EmptyElement(typeof (ChildClass), RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> InstanceWithStringPropertyAndNsDeclaration
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs.Prefix),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> KeyDirective
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration("x", CoreTypes.SpecialNamespace),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Resources, RootNs),
                    P.EmptyElement(typeof(ChildClass), RootNs),
                    P.Key("SomeKey"),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> AttachedProperty
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration("", "root"),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.AttachableProperty<Container>("Property", "Value", RootNs),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> SingleOpenWithNs
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass),  RootNs),
                    P.EndTag(),
                };
            }
        }
        public IEnumerable<ProtoXamlInstruction> SingleOpenAndClose
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ElementWith2NsDeclarations
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration(AnotherNs),
                    P.EmptyElement<DummyClass>(RootNs),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ElementWithChild
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.EmptyElement(typeof(ChildClass), RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> CollectionWithMixedEmptyAndNotEmptyNestedElements
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (Grid), RootNs),
                    P.NonEmptyPropertyElement<Grid>(g => g.Children, RootNs),
                    P.NonEmptyElement(typeof (TextBlock), RootNs),
                    P.EndTag(),
                    P.Text(),
                    P.EmptyElement(typeof (TextBlock), RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> MixedPropertiesWithContentPropertyBefore
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (Grid), RootNs),
                    P.EmptyElement<TextBlock>(RootNs),
                    P.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, RootNs),
                    P.EmptyElement(typeof (RowDefinition), RootNs),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> EmptyElementWithStringProperty
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement(typeof (DummyClass), RootNs),
                    P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs.Prefix),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> EmptyElementWithTwoStringProperties
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement(typeof (DummyClass), RootNs),
                    P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs.Prefix),
                    P.Attribute<DummyClass>(d => d.AnotherProperty, "Another!", RootNs.Prefix),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> SingleCollapsed
        {
            get
            {
                return new Collection<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement<DummyClass>(RootNs),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> CollectionWithMoreThanOneItem
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement<DummyClass>(RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> NestedChildWithContentProperty
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ElementWith2NsDeclarations2
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration(AnotherNs),
                    P.EmptyElement(typeof(DummyClass), RootNs),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ElementWithNestedChild
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.EmptyElement(typeof (ChildClass), RootNs),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> NestedCollectionWithContentProperty
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> TwoNestedPropertiesUsingContentProperty
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),

                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main1", RootNs.Prefix),
                    P.Text(),

                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main2", RootNs.Prefix),
                    P.Text(),

                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof(ChildClass), RootNs),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ExpandedStringProperty
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, RootNs),
                    P.Text("Property!"),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ComplexNesting
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.Attribute<DummyClass>(@class => @class.SampleProperty, "Sample", RootNs.Prefix),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.NonEmptyPropertyElement<ChildClass>(d => d.Content, RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(@class => @class.Text, "Value!", RootNs.Prefix),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> MixedPropertiesWithContentPropertyAfter
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (Grid), RootNs),
                    P.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, RootNs),
                    P.EmptyElement(typeof (RowDefinition), RootNs),
                    P.EndTag(),
                    P.EmptyElement<TextBlock>(RootNs),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoXamlInstruction> ContentPropertyInInnerContent
        {
            get
            {
                return new List<ProtoXamlInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (TextBlock), RootNs),                    
                    P.Text("Hi all!!"),
                    P.EndTag(),                                        
                };
            }
        }
    }
}