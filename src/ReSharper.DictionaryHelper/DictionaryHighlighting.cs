using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.DictionaryHelper
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DictionaryHighlighting : IHighlighting
    {
        private readonly IIfStatement _ifStatement;
        private readonly IStructuralMatchResult _matchResult;

        public DictionaryHighlighting(IIfStatement ifStatement, IStructuralMatchResult matchResult)
        {
            _ifStatement = ifStatement;
            _matchResult = matchResult;
        }

        public bool IsValid()
        {
            return _ifStatement != null && _matchResult != null &&
                   _ifStatement.IsValid() && _matchResult.MatchedElement.IsValid();
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

        public IIfStatement IfStatement
        {
            get { return _ifStatement; }
        }

        public IStructuralMatchResult MatchResult
        {
            get { return _matchResult; }
        }
    }
}
