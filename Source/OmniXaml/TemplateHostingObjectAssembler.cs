namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using Assembler;
    using Glass;
    using Typing;

    public class TemplateHostingObjectAssembler : IObjectAssembler
    {
        private readonly IObjectAssembler objectAssembler;
        private bool recording;
        private IList<XamlInstruction> nodeList;
        private int depth;

        private readonly IDictionary<PropertyInfo, IDeferredLoader> deferredObjectAssemblers = new Dictionary<PropertyInfo, IDeferredLoader>();
        private IDeferredLoader assembler;

        public TemplateHostingObjectAssembler(IObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
        }

        public IWiringContext WiringContext => objectAssembler.WiringContext;

        public void Process(XamlInstruction instruction)
        {
            if (recording)
            {
                if (instruction.NodeType == XamlNodeType.StartMember)
                {
                    depth++;
                }

                if (instruction.NodeType == XamlNodeType.EndMember)
                {
                    depth--;
                    if (depth == 0)
                    {
                        recording = false;
                        var loaded = assembler.Load(new ReadOnlyCollection<XamlInstruction>(nodeList), WiringContext);
                        objectAssembler.OverrideInstance(loaded);
                        objectAssembler.Process(instruction);
                    }
                }

                if (depth > 0)
                {
                    nodeList.Add(instruction);
                }
            }
            else
            {
                if (instruction.NodeType == XamlNodeType.StartMember && !instruction.Member.IsDirective)
                {                    
                    var hasAssembler = TryGetDeferredAssembler((MutableXamlMember) instruction.Member, out assembler);
                    if (hasAssembler)
                    {
                        recording = true;
                        nodeList = new Collection<XamlInstruction>();
                        depth++;
                        objectAssembler.Process(instruction);
                    }
                }

                if (!recording)
                {
                    objectAssembler.Process(instruction);
                }                
            }
        }

        private bool TryGetDeferredAssembler(MutableXamlMember xamlMember, out IDeferredLoader assembler)
        {
            Guard.ThrowIfNull(xamlMember, nameof(xamlMember));

            var propInfo = xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name);
            if  (propInfo!=null)
            {
                var success = deferredObjectAssemblers.TryGetValue(propInfo, out assembler);
                return success;
            }

            assembler = null;
            return false;
        }

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler
        {
            get { return objectAssembler.XamlSetValueHandler; }
            set { objectAssembler.XamlSetValueHandler = value; }
        }

        public void OverrideInstance(object instance)
        {            
        }

        public IList NodeList => new ReadOnlyCollection<XamlInstruction>(nodeList);

        public void AddDeferredLoader<TItem>(Expression<Func<TItem, object>> selector, IDeferredLoader assembler)
        {
            var propInfo = typeof(TItem).GetRuntimeProperty(selector.GetFullPropertyName());
            deferredObjectAssemblers.Add(propInfo, assembler);
        }
    }

}