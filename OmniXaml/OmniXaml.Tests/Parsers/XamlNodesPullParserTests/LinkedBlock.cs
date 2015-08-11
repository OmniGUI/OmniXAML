namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using Glass;

    internal class LinkedBlock : IDependency<LinkedBlock>
    {
        private readonly ICollection<XamlNode> nodes;
        private readonly IEnumerable<LinkedBlock> dependencies;

        public LinkedBlock(ICollection<XamlNode> nodes, IEnumerable<LinkedBlock> dependencies)
        {
            this.nodes = nodes;
            this.dependencies = dependencies;
        }

        public IEnumerable<LinkedBlock> Dependencies => dependencies;

        public IEnumerable<XamlNode> Nodes => nodes;
    }
}