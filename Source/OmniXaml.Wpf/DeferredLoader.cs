namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class DeferredLoader : IDeferredLoader
    {
        public object Load(IEnumerable<XamlInstruction> nodes, IWiringContext wiringContext)
        {
            return new TemplateContent(nodes, wiringContext);
        }
    }
}