namespace OmniXaml.ObjectAssembler
{
    internal class NullLevel : Level
    {
        public NullLevel()
        {
            IsEmpty = true;
        }

        public override string ToString()
        {
            return "{Null Level}";
        }
    }
}