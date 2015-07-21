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

        public List<XamlNode> CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem()
        {
            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""), x.StartObject(typeof (ChildClass)), x.StartMember<ChildClass>(d => d.Content), x.StartObject(typeof (Item)), x.StartMember<Item>(item => item.Children), x.GetObject(), x.Items(), x.StartObject(typeof (Item)), x.StartMember<Item>(i => i.Title), x.Value("Item1"), x.EndMember(), x.EndObject(), x.EndMember(), x.EndObject(), x.EndMember(), x.EndObject(), x.EndMember(), x.StartMember<DummyClass>(d => d.Child), x.StartObject(typeof (ChildClass)), x.EndObject(), x.EndMember(), x.EndObject(),
            };
            return expectedNodes;
        }

        public IEnumerable<ProtoXamlNode> CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(string rootNs)
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (ChildClass),  string.Empty),
                p.NonEmptyElement(typeof (Item), string.Empty),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Item1", ""),
                p.Text(),
                p.EndTag(),
                p.Text(),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Child, ""),
                p.EmptyElement(typeof (ChildClass), ""),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };
            return input;
        }

        public List<XamlNode> CreateExpectedNodesForTwoNestedProperties()
        {
            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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

        public List<XamlNode> CreateExpectedNodesCollectionsContentPropertyNesting()
        {
            return new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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

        public IEnumerable<ProtoXamlNode> CreateInputForCollectionsContentPropertyNesting()
        {
            const string rootNs = "root";
            return new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (DummyClass), string.Empty),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Main1", ""),
                p.Text(),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Main2", ""),
                p.Text(),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Child, ""),
                p.NonEmptyElement(typeof (ChildClass), string.Empty),
                p.NonEmptyElement(typeof (Item), string.Empty),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Item1", ""),
                p.Text(),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Item2", ""),
                p.Text(),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Item3", ""),
                p.Text(),
                p.EndTag(),
                p.Text(),
                p.NonEmptyPropertyElement<ChildClass>(c => c.Child, ""),
                p.EmptyElement<ChildClass>(""),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };
        }

        public IEnumerable<ProtoXamlNode> CreateInputForTwoNestedProperties()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, "root"),
                p.NonEmptyElement(typeof (DummyClass), string.Empty),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Items, ""),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Main1", ""),
                p.Text(),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Main2", ""),
                p.Text(),
                p.EndTag(),
                p.NonEmptyPropertyElement<DummyClass>(d => d.Child, ""),
                p.NonEmptyElement(typeof (ChildClass), string.Empty),
                p.EndTag(),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };
            return input;
        }

        public List<XamlNode> CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection()
        {
            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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

        public IEnumerable<ProtoXamlNode> CreateInputForImplicitContentPropertyWithImplicityCollection()
        {
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (ChildClass), string.Empty),
                p.NonEmptyElement(typeof (Item), string.Empty),
                p.EmptyElement(typeof (Item), ""),
                p.Attribute<Item>(d => d.Title, "Item1", ""),
                p.Text(),
                p.EndTag(),
                p.EndTag(),
            };

            return input;
        }
    }
}