using System.Collections;

namespace OmniXaml.NewAssembler.Commands
{
    using Typing;

    public class GetObjectCommand : Command
    {
        public GetObjectCommand(SuperObjectAssembler superObjectAssembler) : base(superObjectAssembler)
        {            
        }

        public override void Execute()
        {            
            StateCommuter.RaiseLevel();
            StateCommuter.IsGetObject = true;
            StateCommuter.Collection = GetCollection();            
        }

        private ICollection GetCollection()
        {
            var xamlMemberBase = (MutableXamlMember) StateCommuter.PreviousMember;
            return (ICollection) xamlMemberBase.GetValue(StateCommuter.PreviousInstance);
        }
    }
}