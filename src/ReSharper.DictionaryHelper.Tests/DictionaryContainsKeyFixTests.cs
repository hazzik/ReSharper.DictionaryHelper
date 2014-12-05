using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.TestFramework;
using JetBrains.Util;
#if !RESHARPER9
using JetBrains.ReSharper.Intentions.CSharp.QuickFixes.Tests;
#else
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
#endif
using NUnit.Framework;
namespace ReSharper.DictionaryHelper.Tests
{
    [TestFixture]
    public class DictionaryContainsKeyFixTests : CSharpQuickFixTestBase<DictionaryContainsKeyFix>
    {
        protected override string RelativeTestDataPath
        {
            get { return @""; }
        }

        [TestCaseSource("Files")]
        public void Test(string file)
        {
            DoTestFiles(file);
        }

        private IEnumerable<string> Files()
        {
            return new DirectoryInfo(BaseTestDataPath.Combine(RelativeTestDataPath).FullPath)
                .GetFiles("*.cs")
                .Select(x => x.Name);
        }
    }
}
