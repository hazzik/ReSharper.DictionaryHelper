using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ReSharper.DictionaryHelper.Tests
{
    [UsedImplicitly]
    public class DictionaryManualTests
    {
        private static int z4;
        private int z3;

        public int Test1(Dictionary<int, int> dictionary, int key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return -1;
        }

        [Test]
        public void AvailabilityTest()
        {
            var dictionary = new Dictionary<int, int>();
            var key = 1 + 1;

            if (dictionary.ContainsKey(key))
            {
                {
                }
                var z = 0;
                dictionary[key] = z;
            }
            else
            {
            }

            if (dictionary.ContainsKey(key))
            {
                {
                }
                var z = 0;
                dictionary[key] = z;
                int z2;
                var z1 = dictionary[key + 1];
                z2 = dictionary[key];
                z3 = dictionary[key];
                z3 = dictionary[key];
                z4 = dictionary[key];
                z4 = dictionary[key];
                new DictionaryManualTests().z3 = dictionary[key];
                var x = new DictionaryManualTests();
                x.z3 = dictionary[key];
            }
            else
            {
            }
        }
    }
}
