namespace OmniXaml.Tests.Resources
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Builder;
    using Classes;
    using Classes.Another;
    using Classes.WpfLikeModel;
    using Common;
    using Typing;

    public class XamlInstructionResources
    {
        public XamlInstructionResources(GivenAWiringContextWithNodeBuilders context)
        {
            RootNs = context.RootNs;
            AnotherNs = context.AnotherNs;
            SpecialNs = context.SpecialNs;
            X = context.X;
        }

        public NamespaceDeclaration SpecialNs { get; set; }

        private XamlInstructionBuilder X { get; }

        private NamespaceDeclaration RootNs { get; }

        private NamespaceDeclaration AnotherNs { get; }

        public IEnumerable<XamlInstruction> ExtensionWithTwoArguments()
        {
            return new Collection<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.StartObject(typeof (DummyExtension)),
                X.MarkupExtensionArguments(),
                X.Value("One"),
                X.Value("Second"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };
        }

        public IEnumerable<XamlInstruction> ExtensionWithNonStringArgument()
        {
            return new Collection<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(d => d.Number),
                X.StartObject(typeof (IntExtension)),
                X.MarkupExtensionArguments(),
                X.Value("123"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };
        }

        public IEnumerable<XamlInstruction> OneObject
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> ObjectWithMember
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.Value("Property!"),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> ObjectWithTwoMembers
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.Value("Property!"),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.AnotherProperty),
                    X.Value("Another!"),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> CollectionWithInnerCollection
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),

                    // Inner collection
                    X.StartMember<Item>(d => d.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> WithCollectionAndInnerAttribute
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(d => d.Title),
                    X.Value("SomeText"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> GetMemberWithIncompatibleTypes()
        {
            return new Collection<XamlInstruction>
            {
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(d => d.Number),
                X.Value("12"),
                X.EndMember(),
                X.EndObject()
            };
        }

        public IEnumerable<XamlInstruction> ExtensionWithArgument
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.StartObject(typeof (DummyExtension)),
                    X.MarkupExtensionArguments(),
                    X.Value("Option"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> GetString(NamespaceDeclaration sysNs)
        {
            return new Collection<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(sysNs),
                X.StartObject<string>(),
                X.StartDirective("_Initialization"),
                X.Value("Text"),
                X.EndMember(),
                X.EndObject()
            };
        }

        public IEnumerable<XamlInstruction> GetSingleObject()
        {
            return new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };
        }

        public IEnumerable<XamlInstruction> NestedChild
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(c => c.Child),
                    X.StartObject<ChildClass>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> InstanceWithChild
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject(typeof (ChildClass)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> ObjectWithChild
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject<ChildClass>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> ComplexNesting
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(c => c.SampleProperty),
                    X.Value("Sample"),
                    X.EndMember(),
                    X.StartMember<DummyClass>(c => c.Child),
                    X.StartObject<ChildClass>(),
                    X.StartMember<ChildClass>(c => c.Content),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(d => d.Text),
                    X.Value("Value!"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> CollectionWithMoreThanOneItem
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> NestedChildWithContentProperty
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<ChildClass>(),
                    X.StartMember<ChildClass>(c => c.Content),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> TwoNestedPropertiesUsingContentProperty
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Main1"),
                    X.EndMember(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Main2"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject(typeof (ChildClass)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> MixedPropertiesWithContentPropertyAfter
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (Grid)),
                    X.StartMember<Grid>(d => d.RowDefinitions),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject(typeof (RowDefinition)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<Grid>(d => d.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject(typeof (TextBlock)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> CollectionWithMixedEmptyAndNotEmptyNestedElements
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (Grid)),
                    X.StartMember<Grid>(d => d.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject(typeof (TextBlock)),
                    X.EndObject(),
                    X.StartObject(typeof (TextBlock)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> MixedPropertiesWithContentPropertyBefore
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (Grid)),
                    X.StartMember<Grid>(d => d.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject(typeof (TextBlock)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<Grid>(d => d.RowDefinitions),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject(typeof (RowDefinition)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem
        {
            get
            {
                var expectedInstructions = new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (ChildClass)),
                    X.StartMember<ChildClass>(d => d.Content),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(item => item.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Item1"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject(typeof (ChildClass)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
                return expectedInstructions;
            }
        }

        public IEnumerable<XamlInstruction> TwoNestedProperties
        {
            get
            {
                var expectedInstructions = new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Main1"),
                    X.EndMember(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Main2"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject(typeof (ChildClass)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
                return expectedInstructions;
            }
        }

        public IEnumerable<XamlInstruction> ContentPropertyNesting
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Main1"),
                    X.EndMember(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Main2"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject(typeof (ChildClass)),
                    X.StartMember<ChildClass>(d => d.Content),
                    X.StartObject<Item>(),
                    // Collection of Items
                    X.StartMember<Item>(i => i.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Item1"),
                    X.EndMember(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Item2"),
                    X.EndMember(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(i => i.Title),
                    X.Value("Item3"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    // End of collection of items

                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<ChildClass>(c => c.Child),
                    X.StartObject(typeof (ChildClass)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (ChildClass)),
                X.StartMember<ChildClass>(d => d.Content),
                X.StartObject<Item>(),
                X.StartMember<Item>(item => item.Children),
                X.GetObject(),
                X.Items(),
                X.StartObject<Item>(),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item1"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };
            return expectedInstructions;
        }

        public IEnumerable<XamlInstruction> ElementWithTwoDeclarations
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.NamespacePrefixDeclaration(AnotherNs),
                    X.StartObject<DummyClass>(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> KeyDirective
        {
            get
            {
                return new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Resources),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<ChildClass>(),
                    X.StartDirective("Key"),
                    X.Value("SomeKey"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> KeyDirective2
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.NamespacePrefixDeclaration(CoreTypes.SpecialNamespace, "x"),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Resources),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject(typeof (ChildClass)),
                    X.StartDirective("Key"),
                    X.Value("SomeKey"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> DifferentNamespacesAndMoreThanOneProperty
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration("root", string.Empty),
                    X.NamespacePrefixDeclaration("another", "x"),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.Value("One"),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.AnotherProperty),
                    X.Value("Two"),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                    X.StartObject(typeof (Foreigner)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> ContentPropertyForSingleProperty
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (ChildClass)),
                    X.StartMember<ChildClass>(d => d.Content),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> ContentPropertyForCollectionMoreThanOneElement
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> CollapsedTagWithProperty
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.Value("SomeText"),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> SingleInstance
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> DifferentNamespaces
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration("root", string.Empty),
                    X.NamespacePrefixDeclaration("another", "x"),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                    X.StartObject(typeof (Foreigner)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> CollectionWithOneItem
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> CollectionWithOneItemAndAMember
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(d => d.Title),
                    X.Value("SomeText"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> ExpandedStringProperty
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(c => c.SampleProperty),
                    X.Value("Property!"),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> TestReverseMembersReverted
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.StartMember<Setter>(c => c.Property),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<ChildClass>(c => c.Content),
                    X.Value("Tío"),
                    X.EndMember(),
                    X.StartMember<ChildClass>(c => c.Name),
                    X.Value("Hola"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<Setter>(c => c.Value),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<ChildClass>(c => c.Content),
                    X.Value("Tío"),
                    X.EndMember(),
                    X.StartMember<ChildClass>(c => c.Name),
                    X.Value("Hola"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                };
            }
        }

        public IEnumerable<XamlInstruction> TestReverseMembers
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.StartMember<Setter>(c => c.Value),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<ChildClass>(c => c.Name),
                    X.Value("Hola"),
                    X.EndMember(),
                    X.StartMember<ChildClass>(c => c.Content),
                    X.Value("Tío"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.StartMember<Setter>(c => c.Property),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<ChildClass>(c => c.Name),
                    X.Value("Hola"),
                    X.EndMember(),
                    X.StartMember<ChildClass>(c => c.Content),
                    X.Value("Tío"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                };
            }
        }

        public IEnumerable<XamlInstruction> TwoMembersReversed
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.StartMember<Setter>(c => c.Property),
                    X.Value("Property"),
                    X.EndMember(),
                    X.StartMember<Setter>(c => c.Value),
                    X.Value("Value"),
                    X.EndMember(),
                };
            }
        }

        public IEnumerable<XamlInstruction> TwoMembers
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.StartMember<Setter>(c => c.Value),
                    X.Value("Value"),
                    X.EndMember(),
                    X.StartMember<Setter>(c => c.Property),
                    X.Value("Property"),
                    X.EndMember(),
                };
            }
        }

        public List<XamlInstruction> SimpleExtensionWithOneAssignment
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.StartObject(typeof (DummyExtension)),
                    X.StartMember<DummyExtension>(d => d.Property),
                    X.Value("SomeValue"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public List<XamlInstruction> SimpleExtension
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.StartObject(typeof (DummyExtension)),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public List<XamlInstruction> ContentPropertyForCollectionOneElement
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject()
                };
            }
        }

        public IEnumerable<XamlInstruction> TextBlockWithText
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<TextBlock>(),
                    X.StartMember<TextBlock>(c => c.Text),
                    X.Value("Hi all!!"),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> ChildInNameScope
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.NamespacePrefixDeclaration(SpecialNs),
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject<ChildClass>(),
                    X.StartDirective("Name"),
                    X.Value("MyObject"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> ChildInDeeperNameScope
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject<Window>(),
                    X.StartMember<Window>(d => d.Content),
                    X.StartObject<Grid>(),

                    X.StartMember<Grid>(g => g.Children),
                    X.GetObject(),

                    X.StartObject<TextBlock>(),
                    X.StartDirective("Name"),
                    X.Value("MyTextBlock"),
                    X.EndMember(),
                    X.EndObject(),

                    X.StartObject<TextBlock>(),
                    X.StartDirective("Name"),
                    X.Value("MyOtherTextBlock"),
                    X.EndMember(),
                    X.EndObject(),

                    X.StartObject<TextBlock>(),
                    X.EndObject(),

                    X.EndObject(),
                    X.EndMember(),

                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                };
            }
        }

        public IEnumerable<XamlInstruction> NameWithNoNamescopesToRegisterTo
        {
            get
            {
                return new List<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    
                    X.StartObject<Grid>(),

                    X.StartMember<Grid>(g => g.Children),
                    X.GetObject(),

                    X.StartObject<TextBlock>(),
                    X.StartDirective("Name"),
                    X.Value("MyTextBlock"),
                    X.EndMember(),
                    X.EndObject(),

                    X.StartObject<TextBlock>(),
                    X.EndObject(),

                    X.EndObject(),
                    X.EndMember(),

                    X.EndObject(),                    
                };
            }
        }
    }
}