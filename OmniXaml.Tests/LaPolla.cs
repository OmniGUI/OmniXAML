using System;
using OmniXaml.Rework;
using OmniXaml.Services;

namespace OmniXaml.Tests
{
    public class LaPolla
    {

        public LaPolla()
        {

        }

        public object Inflate(ConstructionNode constructionNode)
        {
            INewObjectBuilder innerBestia = new NewObjectBuilder(new SimpleInstanceCreator(), new SimpleValueConverter(), new NoActionValuePipeline());
            innerBestia.NodeInflated.Subscribe();
            return innerBestia.Inflate(constructionNode);
        }
    }
}