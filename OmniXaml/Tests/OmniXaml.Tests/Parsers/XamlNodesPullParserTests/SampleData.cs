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

        public List<XamlNode> CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(NamespaceDeclaration rootNs)
        {
            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs), x.StartObject(typeof (ChildClass)), x.StartMember<ChildClass>(d => d.Content), x.StartObject(typeof (Item)), x.StartMember<Item>(item => item.Children), x.GetObject(), x.Items(), x.StartObject(typeof (Item)), x.StartMember<Item>(i => i.Title), x.Value("Item1"), x.EndMember(), x.EndObject(), x.EndMember(), x.EndObject(), x.EndMember(), x.EndObject(), x.EndMember(), x.StartMember<DummyClass>(d => d.Child), x.StartObject(typeof (ChildClass)), x.EndObject(), x.EndMember(), x.EndObject(),
            };
            return expectedNodes;
        }

        public IEnumerable<ProtoXamlNode> CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(NamespaceDeclaration rootNs)
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (ChildClass), rootNs),
                p.NonEmptyElement(typeof (Item), rootNs),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Item1", rootNs),
                p.Text(),
                p.EndTag(),
                p.Text(),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                p.EmptyElement(typeof (ChildClass), rootNs),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };
            return input;
        }

        public List<XamlNode> CreateExpectedNodesForTwoNestedProperties(NamespaceDeclaration rootNs)
        {
            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof (DummyClass)),
                x.StartMember<DummyClass>(d => d.Items),
                x.GetObject(),
                x.Items(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Main1"),
                x.EndMember(),
                x.EndObject(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Main2"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.StartMember<DummyClass>(d => d.Child),
                x.StartObject(typeof (ChildClass)),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };
            return expectedNodes;
        }

        public List<XamlNode> CreateExpectedNodesCollectionsContentPropertyNesting(NamespaceDeclaration rootNs)
        {
            return new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof (DummyClass)),
                x.StartMember<DummyClass>(d => d.Items),
                x.GetObject(),
                x.Items(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Main1"),
                x.EndMember(),
                x.EndObject(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Main2"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.StartMember<DummyClass>(d => d.Child),
                x.StartObject(typeof (ChildClass)),
                x.StartMember<ChildClass>(d => d.Content),
                x.StartObject(typeof (Item)),
                // Collection of Items
                x.StartMember<Item>(i => i.Children),
                x.GetObject(),
                x.Items(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Item1"),
                x.EndMember(),
                x.EndObject(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Item2"),
                x.EndMember(),
                x.EndObject(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Item3"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                // End of collection of items

                x.EndObject(),
                x.EndMember(),
                x.StartMember<ChildClass>(c => c.Child),
                x.StartObject(typeof (ChildClass)),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };
        }

        public IEnumerable<ProtoXamlNode> CreateInputForCollectionsContentPropertyNesting(NamespaceDeclaration rootNs)
        {
            
            return new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Main1", rootNs),
                p.Text(),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Main2", rootNs),
                p.Text(),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                p.NonEmptyElement(typeof (ChildClass), rootNs),
                p.NonEmptyElement(typeof (Item), rootNs),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Item1", rootNs),
                p.Text(),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Item2", rootNs),
                p.Text(),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Item3", rootNs),
                p.Text(),
                p.EndTag(),
                p.Text(),
                p.NonEmptyPropertyElement<ChildClass>(c => c.Child, rootNs),
                p.EmptyElement<ChildClass>(rootNs),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };
        }

        public IEnumerable<ProtoXamlNode> CreateInputForTwoNestedProperties(NamespaceDeclaration rootNs)
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Main1", rootNs),
                p.Text(),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Main2", rootNs),
                p.Text(),
                p.EndTag(),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                p.NonEmptyElement(typeof (ChildClass), rootNs),
                p.EndTag(),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };
            return input;
        }

        public List<XamlNode> CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection(NamespaceDeclaration rootNs)
        {
            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof (ChildClass)),
                x.StartMember<ChildClass>(d => d.Content),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(item => item.Children),
                x.GetObject(),
                x.Items(),
                x.StartObject(typeof (Item)),
                x.StartMember<Item>(i => i.Title),
                x.Value("Item1"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };
            return expectedNodes;
        }

        public IEnumerable<ProtoXamlNode> CreateInputForImplicitContentPropertyWithImplicityCollection(NamespaceDeclaration rootNs)
        {
            
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (ChildClass), rootNs),
                p.NonEmptyElement(typeof (Item), rootNs),
                p.EmptyElement(typeof (Item), rootNs),
                p.Attribute<Item>(d => d.Title, "Item1", rootNs),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };

            return input;
        }
    }
}