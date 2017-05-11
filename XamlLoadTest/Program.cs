using System;
using System.Text;
using System.Xml;
using ExtendedXmlSerializer.Configuration;
using Xunit.Sdk;

namespace XamlLoadTest
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using OmniXaml;
    using OmniXaml.Rework;
    using OmniXaml.Services;

    class Program
    {
        static void Main(string[] args)
        {
            var instance = LoadXaml(args.First());
            
            ShowResult(instance);
        }

        private static void ShowResult(object instance)
        {
            var json = JsonConvert.SerializeObject(instance, Formatting.Indented);
            Console.WriteLine(json);


            var sb = new StringBuilder();
            var cc = new ConfigurationContainer();
            cc.Create().Serialize(XmlWriter.Create(new StringWriter(sb)), instance);
            Console.Write(sb.ToString());
        }

        private static object LoadXaml(string file)
        {
            var assemblies = new List<Assembly>() {Assembly.GetEntryAssembly(), typeof(OmniXaml.Tests.Model.Window).GetTypeInfo().Assembly};
            var loader = new ExtendedXamlLoader(assemblies);
            return loader.Load(File.ReadAllText(file));
        }
    }

    internal class ExtendedXamlLoader : XamlLoader
    {
        public ExtendedXamlLoader(List<Assembly> assemblies) : base(assemblies)
        {
        }

        protected override IValuePipeline GetValuePipeline()
        {
            return new MarkupExtensionValuePipeline(new NoActionValuePipeline());
        }
    }

    public abstract class ValuePipeline : IValuePipeline
    {
        private readonly IValuePipeline pipeline;

        protected ValuePipeline(IValuePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Handle(object parent, Member member, MutablePipelineUnit mutable)
        {
            if (!mutable.Handled)
            {
                pipeline.Handle(parent, member, mutable);
                HandleCore(parent, member, mutable);
            }
        }

        protected abstract void HandleCore(object parent, Member member, MutablePipelineUnit mutable);
    }

    public class MarkupExtensionValuePipeline : ValuePipeline
    {
        public MarkupExtensionValuePipeline(IValuePipeline inner) : base(inner)
        {
        }

        protected override void HandleCore(object parent, Member member, MutablePipelineUnit mutable)
        {
            var extension = mutable.Value as IMarkupExtension;
            if (extension != null)
            {
                var keyedInstance = new KeyedInstance(parent);
                var assignment = new Assignment(keyedInstance, member, mutable.Value);
                var finalValue = extension.GetValue(new ExtensionValueContext(assignment, null, null, null));
                mutable.Value = finalValue;
            }
        }
    }
}