using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test03
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            if (B())
                if (!dictionary /*{caret}*/.ContainsKey(key))
                    return null;
                else
                    return dictionary[key];
        }

        public static bool B()
        {
            return true;
        }
    }
}
