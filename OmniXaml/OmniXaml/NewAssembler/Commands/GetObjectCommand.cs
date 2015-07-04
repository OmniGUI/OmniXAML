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
            StateCommuter.IsGetObject = true;
            StateCommuter.Collection = GetCollection();
            StateCommuter.RaiseLevel();
        }

        private ICollection GetCollection()
        {
            return (ICollection) StateCommuter.PreviousMember.GetValue(StateCommuter.PreviousInstance);
        }
    }
}