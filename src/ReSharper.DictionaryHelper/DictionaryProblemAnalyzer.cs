using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;

namespace ReSharper.DictionaryHelper
{
    [ElementProblemAnalyzer(new[] { typeof (IIfStatement) }, HighlightingTypes = new[] { typeof (DictionaryHighlighting) })]
    public class DictionaryProblemAnalyzer : ElementProblemAnalyzer<IIfStatement>
    {
        private readonly Patterns patterns = new Patterns();

        public DictionaryProblemAnalyzer(StructuralSearchEngine ssr)
        {
        }

        protected override void Run(IIfStatement element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var containsKeys = patterns.GetMatchingContainsKey(element.Condition);
            foreach (var containsKey in containsKeys)
            {
                var dictionary = containsKey.GetMatchedElement("dictionary");
                var key = containsKey.GetMatchedElement("key");

                if (patterns.GetMatchingDictionaryAccess(element, dictionary, key).Any())
                    consumer.AddHighlighting(new DictionaryHighlighting(element), containsKey.MatchedElement.GetHighlightingRange());
            }
        }
    }
}
