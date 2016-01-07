namespace OmniXaml
{
    using System.Collections.Generic;

    public static class LookaheadBuffer
    {
        public static IEnumerable<Instruction> GetUntilEndOfRoot(IEnumerator<Instruction> enumerator)
        {
            var count = 0;
            var isEndOfRootObject = false;

            if (enumerator.Current.Equals(default(Instruction)))
            {
                yield break;
            }

            do
            {
                yield return enumerator.Current;

                switch (enumerator.Current.InstructionType)
                {
                    case InstructionType.StartObject:
                    case InstructionType.GetObject:
                        count++;
                        break;
                    case InstructionType.EndObject:
                        count--;
                        break;
                }

                if (count == 0)
                {
                    isEndOfRootObject = true;
                }

                enumerator.MoveNext();
                
            } while (!isEndOfRootObject);
        }
    }
}