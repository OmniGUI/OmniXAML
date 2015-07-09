using System.Collections;

namespace OmniXaml.NewAssembler.Commands
{
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
            return (ICollection) StateCommuter.PreviousMember.GetValue(StateCommuter.PreviousInstance);
        }
    }
}