namespace OmniXaml.Tests.Classes
{
    using System;
    using System.Collections.Generic;

    public class Template
    {
        public TemplateContent Content { get; set; }
    }

    public class DummyDeferredObjectAssembler : IDeferredObjectAssembler
    {
        public object Load(IEnumerable<XamlNode> nodes)
        {
            return new TemplateContent(nodes);
        }
    }

    public interface IDeferredObjectAssembler
    {
        object Load(IEnumerable<XamlNode> nodes);
    }
}