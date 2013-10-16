using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test02
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            if (!dictionary /*{caret}*/.ContainsKey(key))
                return null;
            else
                return dictionary[key];
        }
    }
}
