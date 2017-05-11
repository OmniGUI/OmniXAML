using System;
using OmniXaml.Rework;

namespace OmniXaml.Tests
{
    internal class PipelineMock : IValuePipeline
    {
        public void SetMutator(Action<object, Member, MutablePipelineUnit> func)
        {
            this.mutator = func;
        }

        private Action<object, Member, MutablePipelineUnit> mutator = (o, member, arg3) => { };

        public void Handle(object parent, Member member, MutablePipelineUnit mutable)
        {
            mutator(parent, member, mutable);
        }
    }
}