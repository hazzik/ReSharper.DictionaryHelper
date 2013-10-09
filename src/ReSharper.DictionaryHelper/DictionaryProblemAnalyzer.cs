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
            var conditionMatch = patterns.ContainsKeyMatcher.Match(element.Condition);
            if (conditionMatch.Matched)
            {
                var dictionary = conditionMatch.GetMatchedElement("dictionary");
                var key = conditionMatch.GetMatchedElement("key");

                if (patterns.GetMatchingDictionaryAccess(element.Then, dictionary, key).Any())
                    consumer.AddHighlighting(new DictionaryHighlighting(element), element.Condition.GetHighlightingRange());
            }
        }
    }
}
