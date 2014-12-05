using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.DictionaryHelper
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DictionaryContainsKeyWarning : IHighlighting
    {
        public ITreeNode[] DictionaryAccess { get; }

        public DictionaryContainsKeyWarning(ICSharpStatement statement, ITreeNode[] dictionaryAccess, ITreeNode matchedElement, ITreeNode key, IExpression dictionary)
        {
            Statement = statement;
            MatchedElement = matchedElement;
            Dictionary = dictionary;
            Key = key;
            DictionaryAccess = dictionaryAccess;
        }

        public bool IsValid() => Statement != null && Statement.IsValid();

        public DocumentRange CalculateRange() => MatchedElement.GetHighlightingRange();

        public string ToolTip => "Optimize access to dictionary";

        public string ErrorStripeToolTip => ToolTip;

        public int NavigationOffsetPatch => 0;

        public ICSharpStatement Statement { get; }

        public ITreeNode MatchedElement { get; }

        public ITreeNode Key { get; }

        public IExpression Dictionary { get; }
    }
}
