using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;
using JetBrains.ReSharper.Psi.Tree;

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

            foreach (var result in patterns.GetMatchingContainsKey(element.Condition))
            {
                var matchedElement = result.MatchedElement;
                var dictionary = result.GetMatchedElement("dictionary");
                var key = result.GetMatchedElement("key");

                var dictionaryAccess = patterns.GetMatchingDictionaryAccess(element, dictionary, key);
                if (dictionaryAccess.Length > 0)
                {
                    var highlighting = new DictionaryHighlighting(element, dictionaryAccess, matchedElement, key, (IExpression) dictionary);
                    consumer.AddHighlighting(highlighting, matchedElement.GetHighlightingRange());
                    foreach (var treeNode in dictionaryAccess)
                    {
                        consumer.AddHighlighting(highlighting, treeNode.GetHighlightingRange());
                    }
                }
            }
        }
    }
}
