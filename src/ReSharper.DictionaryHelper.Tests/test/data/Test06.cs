using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test01
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            return dictionary./*{caret}*/ContainsKey(key) ? dictionary[key] : null;
        }
    }
}
