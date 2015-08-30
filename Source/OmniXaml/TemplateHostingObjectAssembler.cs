namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Glass;
    using ObjectAssembler;
    using Typing;

    public class TemplateHostingObjectAssembler : IObjectAssembler
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly DeferredLoaderMapping mapping;
        private bool recording;
        private IList<XamlInstruction> nodeList;
        private int depth;
        private IDeferredLoader assembler;

        public TemplateHostingObjectAssembler(IObjectAssembler objectAssembler, DeferredLoaderMapping mapping)
        {
            this.objectAssembler = objectAssembler;
            this.mapping = mapping;
        }

        public IWiringContext WiringContext => objectAssembler.WiringContext;

        public void Process(XamlInstruction instruction)
        {
            if (recording)
            {
                if (instruction.InstructionType == XamlInstructionType.StartMember)
                {
                    depth++;
                }

                if (instruction.InstructionType == XamlInstructionType.EndMember)
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
                if (instruction.InstructionType == XamlInstructionType.StartMember && !instruction.Member.IsDirective)
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

        private bool TryGetDeferredAssembler(MutableXamlMember xamlMember, out IDeferredLoader loader)
        {
            Guard.ThrowIfNull(xamlMember, nameof(xamlMember));

            var propInfo = xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name);
            if  (propInfo!=null)
            {
                var success = mapping.TryGetMapping(propInfo, out loader);
                return success;
            }

            loader = null;
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
    }
}