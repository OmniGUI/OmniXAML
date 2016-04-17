using OmniXaml.Tests.Classes.Another;

namespace OmniXaml.Tests.Resources
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using Typing;

    internal class ProtoInstructionResources
    {
        public ProtoInstructionResources(GivenARuntimeTypeSource source)
        {
            RootNs = source.RootNs;
            AnotherNs = source.AnotherNs;
            P = source.P;
        }

        private NamespaceDeclaration RootNs { get; }
        public NamespaceDeclaration AnotherNs { get; }

        private ProtoInstructionBuilder P { get; }

        public IEnumerable<ProtoInstruction> ContentPropertyNesting
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> TwoNestedProperties
        {
            get
            {
                var input = new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> ImplicitContentPropertyWithImplicityCollection
        {
            get
            {
                var input = new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem
        {
            get
            {
                var input = new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> CollapsedTag
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement(typeof(DummyClass), RootNs),
                };
            }
        }

        public IEnumerable<ProtoInstruction> FourLevelsOfNesting
        {
            get
            {
                return new Collection<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> TwoNestedPropertiesEmpty
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> GetString(NamespaceDeclaration sysNs)
        {
            return new List<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(sysNs),
                P.NonEmptyElement(typeof (string), sysNs),
                P.Text("Text"),
                P.EndTag(),
            };
        }

        public IEnumerable<ProtoInstruction> ContentPropertyForCollectionOneElement
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.EmptyElement(typeof(Item), RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ThreeLevelsOfNesting
        {
            get
            {
                return new Collection<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> InstanceWithStringPropertyAndNsDeclaration
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs.Prefix),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> KeyDirective
        {
            get
            {
                return new Collection<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> AttachedProperty
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration("", "root"),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.InlineAttachableProperty<Container>("Property", "Value", RootNs),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ExpandedAttachedProperty
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration("", "root"),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),     
                    P.ExpandedAttachedProperty<Container>("Property", RootNs),
                    P.Text("Value"),
                    P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ExpandedAttachablePropertyAndItemBelow
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration("", "root"),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                        P.NonEmptyElement<Item>(RootNs),
                            P.ExpandedAttachedProperty<Container>("Property", RootNs),
                                P.Text("Value"),
                            P.EndTag(),                            
                        P.EndTag(),
                        P.Text(),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> PrefixedExpandedAttachablePropertyAndItemBelow
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration("", "root"),
                    P.NamespacePrefixDeclaration("a", "another"),
                    P.NonEmptyElement(typeof (DummyClass), RootNs),
                        P.NonEmptyElement<Item>(RootNs),
                            P.ExpandedAttachedProperty<Foreigner>("Property", AnotherNs),
                                P.Text("Value"),
                            P.EndTag(),
                        P.EndTag(),
                        P.Text(),
                    P.EmptyElement<Item>(RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> SingleOpenWithNs
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass),  RootNs),
                    P.EndTag(),
                };
            }
        }
        public IEnumerable<ProtoInstruction> SingleOpenAndClose
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof(DummyClass), RootNs),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ElementWith2NsDeclarations
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration(AnotherNs),
                    P.EmptyElement<DummyClass>(RootNs),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ElementWithChild
        {
            get
            {
                return new Collection<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> CollectionWithMixedEmptyAndNotEmptyNestedElements
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> MixedPropertiesWithContentPropertyBefore
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> EmptyElementWithStringProperty
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement(typeof (DummyClass), RootNs),
                    P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs.Prefix),
                };
            }
        }

        public IEnumerable<ProtoInstruction> EmptyElementWithTwoStringProperties
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement(typeof (DummyClass), RootNs),
                    P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs.Prefix),
                    P.Attribute<DummyClass>(d => d.AnotherProperty, "Another!", RootNs.Prefix),
                };
            }
        }

        public IEnumerable<ProtoInstruction> SingleCollapsed
        {
            get
            {
                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.EmptyElement<DummyClass>(RootNs),
                };
            }
        }

        public IEnumerable<ProtoInstruction> CollectionWithMoreThanOneItem
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> NestedChildWithContentProperty
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ElementWith2NsDeclarations2
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration(AnotherNs),
                    P.EmptyElement(typeof(DummyClass), RootNs),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ElementWithNestedChild
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> NestedCollectionWithContentProperty
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> TwoNestedPropertiesUsingContentProperty
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> ExpandedStringProperty
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> ComplexNesting
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> MixedPropertiesWithContentPropertyAfter
        {
            get
            {
                return new List<ProtoInstruction>
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

        public IEnumerable<ProtoInstruction> ContentPropertyInInnerContent
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement(typeof (TextBlock), RootNs),
                    P.Text("Hi all!!"),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> PureCollection
        {
            get
            {
                var system = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");
                var colections = new NamespaceDeclaration("clr-namespace:System.Collections;assembly=mscorlib", "sysCol");

                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration("sysCol", "clr-namespace:System.Collections;assembly=mscorlib"),
                    P.NamespacePrefixDeclaration("sys", "clr-namespace:System;assembly=mscorlib"),
                    P.NonEmptyElement(typeof (ArrayList), colections),
                    P.NonEmptyElement(typeof (int), system),
                    P.Text("1"),
                    P.EndTag(),
                    P.Text(),
                    P.NonEmptyElement(typeof (int), system),
                    P.Text("2"),
                    P.EndTag(),
                    P.Text(),
                    P.NonEmptyElement(typeof (int), system),
                    P.Text("3"),
                    P.EndTag(),
                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> MixedCollection
        {
            get
            {
                var colections = new NamespaceDeclaration("clr-namespace:System.Collections;assembly=mscorlib", "sysCol");
                var root = new NamespaceDeclaration("root", "");
                
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(colections),
                    P.NamespacePrefixDeclaration(root),
                    P.NonEmptyElement<ArrayList>(colections),

                    P.EmptyElement<DummyClass>(root),
                    P.Text(),

                    P.EmptyElement<DummyClass>(root),
                    P.Text(),

                    P.EmptyElement<DummyClass>(root),
                    P.Text(),

                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ChildInDeeperNameScopeWithNamesInTwoLevels
        {
            get
            {
                var root = new NamespaceDeclaration("root", "");
                var special = new NamespaceDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x");

                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(root),
                    P.NamespacePrefixDeclaration(special),

                    P.NonEmptyElement<Window>(root),

                    P.NonEmptyElement<ListBox>(root),
                    P.Directive(CoreTypes.Name, "MyListBox"),

                    P.NonEmptyElement<ListBoxItem>(root),
                    P.Directive(CoreTypes.Name, "MyListBoxItem"),
                    
                    P.EmptyElement<TextBlock>(root),
                    P.Directive(CoreTypes.Name, "MyTextBlock"),
                    P.Text(),
                    P.EndTag(),

                    P.Text(),
                    P.EndTag(),

                    P.Text(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> AttachableMemberThatIsCollection
        {
            get
            {
                var system = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration(system),
                    P.NonEmptyElement<DummyClass>(RootNs),

                        P.ExpandedAttachedProperty<Container>("Collection", RootNs),

                            P.NonEmptyElement<CustomCollection>(RootNs),

                                P.NonEmptyElement<int>(system),
                                    P.Text("1"),
                                P.EndTag(),
                                P.Text(),

                                P.NonEmptyElement<int>(system),
                                    P.Text("2"),
                                P.EndTag(),
                                P.Text(),

                                P.NonEmptyElement<int>(system),
                                    P.Text("3"),
                                P.EndTag(),
                                P.Text(),                                
                                                                
                            P.EndTag(),
                            P.Text(),
                        P.EndTag(),
                    P.EndTag()                                     
                };
            }
        }

        public IEnumerable<ProtoInstruction> AttachableMemberThatIsCollectionImplicit
        {
            get
            {
                var system = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

                return new Collection<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NamespacePrefixDeclaration(system),
                    P.NonEmptyElement<DummyClass>(RootNs),

                        P.ExpandedAttachedProperty<Container>("Collection", RootNs),
                           
                            P.NonEmptyElement<int>(system),
                                P.Text("1"),
                            P.EndTag(),
                            P.Text(),

                            P.NonEmptyElement<int>(system),
                                P.Text("2"),
                            P.EndTag(),
                            P.Text(),

                            P.NonEmptyElement<int>(system),
                                P.Text("3"),
                            P.EndTag(),
                            P.Text(),
                           
                        P.EndTag(),
                    P.EndTag()
                };
            }
        }

        public IEnumerable<ProtoInstruction> DirectContentForOneToMany
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement<ItemsControl>(RootNs),
                    P.Text("Hello"),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ImplicitCollection
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement<RootObject>(RootNs),
                        P.NonEmptyPropertyElement<RootObject>(d => d.Collection, RootNs),
                            P.EmptyElement<DummyClass>(RootNs),
                            P.Text(),
                            P.EmptyElement<DummyClass>(RootNs),
                            P.Text(),
                            P.EmptyElement<DummyClass>(RootNs),
                            P.Text(),
                        P.EndTag(),
                    P.EndTag(),
                };
            }
        }

        public IEnumerable<ProtoInstruction> ExplicitCollection
        {
            get
            {
                return new List<ProtoInstruction>
                {
                    P.NamespacePrefixDeclaration(RootNs),
                    P.NonEmptyElement<RootObject>(RootNs),
                        P.NonEmptyPropertyElement<RootObject>(d => d.Collection, RootNs),
                            P.NonEmptyElement<CustomCollection>(RootNs),
                                P.EmptyElement<DummyClass>(RootNs),
                                P.Text(),
                                P.EmptyElement<DummyClass>(RootNs),
                                P.Text(),
                                P.EmptyElement<DummyClass>(RootNs),
                                P.Text(),
                            P.EndTag(),
                            P.Text(),
                        P.EndTag(),
                    P.EndTag(),
                };
            }
        }
    }
}