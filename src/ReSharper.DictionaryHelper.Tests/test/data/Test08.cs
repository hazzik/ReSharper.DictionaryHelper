using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test08
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            if (dictionary/*{caret}*/.ContainsKey(key))
            {
                var val = dictionary[key];
                dictionary[key] = "a new value";
                return dictionary[key];
            }

            return null;
        }
    }
}
