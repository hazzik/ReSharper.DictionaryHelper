using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
#if !RESHARPER9
using JetBrains.ReSharper.Psi.Services.StructuralSearch;
#else
using JetBrains.ReSharper.Feature.Services.StructuralSearch;
using JetBrains.ReSharper.Feature.Services.Daemon;
#endif
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.DictionaryHelper
{
    [ElementProblemAnalyzer(new[] { typeof (IConditionalTernaryExpression) }, HighlightingTypes = new[] { typeof (DictionaryContainsKeyWarning) })]
    public class DictionaryContainsKeyConditionalTernaryExpressionProblemAnalyzer : ElementProblemAnalyzer<IConditionalTernaryExpression>
    {
        private readonly Patterns patterns = new Patterns();

        public DictionaryContainsKeyConditionalTernaryExpressionProblemAnalyzer(StructuralSearchEngine ssr)
        {
        }

        protected override void Run(IConditionalTernaryExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            foreach (var result in patterns.GetMatchingContainsKey(element.ConditionOperand))
            {
                var matchedElement = result.MatchedElement;
                var dictionary = result.GetMatchedElement("dictionary");
                var key = result.GetMatchedElement("key");

                var dictionaryAccess = patterns.GetMatchingDictionaryAccess(element, dictionary, key);
                if (dictionaryAccess.Length > 0)
                {
                    var highlighting = new DictionaryContainsKeyWarning(element.GetContainingStatement(), dictionaryAccess, matchedElement, key, (IExpression)dictionary);
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