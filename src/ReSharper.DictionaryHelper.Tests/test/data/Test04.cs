using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test04
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary/*{caret}*/[key];

            return null;
        }
    }
}
