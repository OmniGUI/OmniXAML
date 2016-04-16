namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Glass.Core;
    using ObjectAssembler;
    using ObjectAssembler.Commands;
    using Typing;

    public class TemplateHostingObjectAssembler : IObjectAssembler
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly DeferredLoaderMapping mapping;
        private bool recording;
        private IList<Instruction> nodeList;
        private int depth;
        private IDeferredLoader assembler;

        public TemplateHostingObjectAssembler(IObjectAssembler objectAssembler, DeferredLoaderMapping mapping)
        {
            this.objectAssembler = objectAssembler;
            this.mapping = mapping;
        }

        public IRuntimeTypeSource TypeSource => objectAssembler.TypeSource;
        public ITopDownValueContext TopDownValueContext => objectAssembler.TopDownValueContext;

        public IInstanceLifeCycleListener LifecycleListener => objectAssembler.LifecycleListener;

        public void Process(Instruction instruction)
        {
            if (recording)
            {
                if (instruction.InstructionType == InstructionType.StartMember)
                {
                    depth++;
                }

                if (instruction.InstructionType == InstructionType.EndMember)
                {
                    depth--;
                    if (depth == 0)
                    {
                        recording = false;
                        var loaded = assembler.Load(new ReadOnlyCollection<Instruction>(nodeList), this.objectAssembler.TypeSource);
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
                if (instruction.InstructionType == InstructionType.StartMember && !instruction.Member.IsDirective)
                {                    
                    var hasAssembler = TryGetDeferredAssembler((MutableMember) instruction.Member, out assembler);
                    if (hasAssembler)
                    {
                        recording = true;
                        nodeList = new Collection<Instruction>();
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

        private bool TryGetDeferredAssembler(MutableMember member, out IDeferredLoader loader)
        {
            Guard.ThrowIfNull(member, nameof(member));

            var propInfo = member.DeclaringType.UnderlyingType.GetRuntimeProperty(member.Name);
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
     
        public IList NodeList => new ReadOnlyCollection<Instruction>(nodeList);     
    }
}