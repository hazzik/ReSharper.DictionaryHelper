using System;
using System.Collections.Generic;

namespace ReSharper.DictionaryHelper.Tests
{
    public class Test02
    {
        public string Method(IDictionary<int, string> dictionary, int key)
        {
            Func<string> method = () => dictionary./*{caret}*/ContainsKey(key) ? dictionary[key] : null;
            return method();
        }
    }
}
