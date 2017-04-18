namespace OmniXaml.Tests.Rework
{
    using System;
    using OmniXaml.Rework;
    using Services;

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