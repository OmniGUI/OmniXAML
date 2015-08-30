namespace OmniXaml
{
    using System.Collections.Generic;

    public static class LookaheadBuffer
    {
        public static IEnumerable<XamlInstruction> GetUntilEndOfRoot(IEnumerator<XamlInstruction> enumerator)
        {
            var count = 0;
            var isEndOfRootObject = false;

            if (enumerator.Current.Equals(default(XamlInstruction)))
            {
                yield break;
            }

            do
            {
                yield return enumerator.Current;

                switch (enumerator.Current.InstructionType)
                {
                    case XamlInstructionType.StartObject:
                        count++;
                        break;
                    case XamlInstructionType.EndObject:
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