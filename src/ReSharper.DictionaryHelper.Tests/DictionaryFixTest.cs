using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.Intentions.CSharp.QuickFixes.Tests;
using NUnit.Framework;

namespace ReSharper.DictionaryHelper.Tests
{
    [TestFixture]
    public class DictionaryAccessTests : CSharpQuickFixTestBase<DictionaryFix>
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
                .EnumerateFiles("*.cs")
                .Select(x => x.Name);
        }
    }
}
