namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using Builder;
    using Classes;

    internal class SampleData
    {
        private readonly ProtoNodeBuilder p;
        private readonly XamlNodeBuilder x;

        public SampleData(ProtoNodeBuilder p, XamlNodeBuilder x)
        {
            this.p = p;
            this.x = x;
        }

        public ProtoNodeBuilder P
        {
            get { return p; }
        }

        public XamlNodeBuilder X
        {
            get { return x; }
        }

        public List<XamlNode> CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(NamespaceDeclaration rootNs)
        {
            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(rootNs), X.StartObject(typeof (ChildClass)), X.StartMember<ChildClass>(d => d.Content), X.StartObject(typeof (Item)), X.StartMember<Item>(item => item.Children), X.GetObject(), X.Items(), X.StartObject(typeof (Item)), X.StartMember<Item>(i => i.Title), X.Value("Item1"), X.EndMember(), X.EndObject(), X.EndMember(), X.EndObject(), X.EndMember(), X.EndObject(), X.EndMember(), X.StartMember<DummyClass>(d => d.Child), X.StartObject(typeof (ChildClass)), X.EndObject(), X.EndMember(), X.EndObject(),
            };
            return expectedNodes;
        }

        public IEnumerable<ProtoXamlNode> CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(NamespaceDeclaration rootNs)
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.NonEmptyElement(typeof (Item), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item1", rootNs),
                P.Text(),
                P.EndTag(),
                P.Text(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                P.EmptyElement(typeof (ChildClass), rootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };
            return input;
        }

        public List<XamlNode> CreateExpectedNodesForTwoNestedProperties(NamespaceDeclaration rootNs)
        {
            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(rootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Main1"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
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
            return expectedNodes;
        }

        public List<XamlNode> CreateExpectedNodesCollectionsContentPropertyNesting(NamespaceDeclaration rootNs)
        {
            return new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(rootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Main1"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
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
                X.StartObject(typeof (Item)),
                // Collection of Items
                X.StartMember<Item>(i => i.Children),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item1"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item2"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
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

        public IEnumerable<ProtoXamlNode> CreateInputForCollectionsContentPropertyNesting(NamespaceDeclaration rootNs)
        {
            
            return new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (DummyClass), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main1", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main2", rootNs),
                P.Text(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.NonEmptyElement(typeof (Item), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item1", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item2", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item3", rootNs),
                P.Text(),
                P.EndTag(),
                P.Text(),
                P.NonEmptyPropertyElement<ChildClass>(c => c.Child, rootNs),
                P.EmptyElement<ChildClass>(rootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };
        }

        public IEnumerable<ProtoXamlNode> CreateInputForTwoNestedProperties(NamespaceDeclaration rootNs)
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (DummyClass), rootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main1", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main2", rootNs),
                P.Text(),
                P.EndTag(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };
            return input;
        }

        public List<XamlNode> CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection(NamespaceDeclaration rootNs)
        {
            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(rootNs),
                X.StartObject(typeof (ChildClass)),
                X.StartMember<ChildClass>(d => d.Content),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(item => item.Children),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
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
            return expectedNodes;
        }

        public IEnumerable<ProtoXamlNode> CreateInputForImplicitContentPropertyWithImplicityCollection(NamespaceDeclaration rootNs)
        {
            
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.NonEmptyElement(typeof (Item), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item1", rootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            return input;
        }
    }
}