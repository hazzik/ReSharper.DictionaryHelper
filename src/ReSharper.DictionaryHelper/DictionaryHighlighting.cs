using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharper.DictionaryHelper
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DictionaryHighlighting : IHighlighting
    {
        private readonly IIfStatement _ifStatement;

        public DictionaryHighlighting(IIfStatement ifStatement)
        {
            _ifStatement = ifStatement;
        }

        public bool IsValid()
        {
            return IfStatement != null && IfStatement.IsValid();
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
    }
}
