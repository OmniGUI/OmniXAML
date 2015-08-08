namespace OmniXaml
{
    using System.Collections.Generic;

    public static class LookaheadBuffer
    {
        public static IEnumerable<XamlNode> GetUntilEndOfRoot(IEnumerator<XamlNode> enumerator)
        {
            var count = 0;
            var isEndOfRootObject = false;

            if (enumerator.Current.Equals(default(XamlNode)))
            {
                yield break;
            }

            do
            {
                yield return enumerator.Current;

                switch (enumerator.Current.NodeType)
                {
                    case XamlNodeType.StartObject:
                        count++;
                        break;
                    case XamlNodeType.EndObject:
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