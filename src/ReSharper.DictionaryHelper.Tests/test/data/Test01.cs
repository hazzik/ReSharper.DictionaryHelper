using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test01
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            if (dictionary/*{caret}*/.ContainsKey(key))
                return dictionary[key];

            return null;
        }
    }
}
