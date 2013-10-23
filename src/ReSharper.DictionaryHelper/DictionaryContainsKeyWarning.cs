using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.DictionaryHelper
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DictionaryContainsKeyWarning : IHighlighting
    {
        public ITreeNode[] DictionaryAccess
        {
            get { return _dictionaryAccess; }
        }

        private readonly ICSharpStatement _statement;
        private readonly ITreeNode _matchedElement;
        private readonly IExpression _dictionary;
        private readonly ITreeNode _key;
        private readonly ITreeNode[] _dictionaryAccess;

        public DictionaryContainsKeyWarning(ICSharpStatement statement, ITreeNode[] dictionaryAccess, ITreeNode matchedElement, ITreeNode key, IExpression dictionary)
        {
            _statement = statement;
            _matchedElement = matchedElement;
            _dictionary = dictionary;
            _key = key;
            _dictionaryAccess = dictionaryAccess;
        }

        public bool IsValid()
        {
            return _statement != null &&
                   _statement.IsValid();
        }

        public string ToolTip
        {
            get { return "Optimize access to dictionary"; }
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public ICSharpStatement Statement
        {
            get { return _statement; }
        }

        public ITreeNode MatchedElement
        {
            get { return _matchedElement; }
        }

        public ITreeNode Key
        {
            get { return _key; }
        }

        public IExpression Dictionary
        {
            get { return _dictionary; }
        }
    }
}
