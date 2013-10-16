using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test04
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            if (dictionary.ContainsKey(key))
            {
                var value = dictionary /*{caret}*/[key];
                var value2 = dictionary[key];
                return dictionary[key];
            }

            return null;
        }
    }
}
