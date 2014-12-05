using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
#if !RESHARPER9
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch.Placeholders;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;
#else
using JetBrains.ReSharper.Feature.Services.StructuralSearch;
using JetBrains.ReSharper.Feature.Services.CSharp.StructuralSearch;
using JetBrains.ReSharper.Feature.Services.CSharp.StructuralSearch.Placeholders;
#endif
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.Lazy;

namespace ReSharper.DictionaryHelper
{
    public class Patterns
    {
        private readonly Lazy<IStructuralMatcher> _containsKey;
        private readonly Lazy<IStructuralMatcher> _dictionaryAccess;

        public Patterns()
        {
            _containsKey = Lazy.Of(() => new CSharpStructuralSearchPattern("$dictionary$.ContainsKey($key$)",
                new ExpressionPlaceholder("dictionary", "System.Collections.Generic.IDictionary<,>"),
                new ExpressionPlaceholder("key")).CreateMatcher());
            _dictionaryAccess = Lazy.Of(() => new CSharpStructuralSearchPattern("$dictionary$[$key$]",
                new ExpressionPlaceholder("dictionary", "System.Collections.Generic.IDictionary<,>"),
                new ExpressionPlaceholder("key")).CreateMatcher());
        }

        private static IEnumerable<IStructuralMatchResult> FindMatches(IStructuralMatcher matcher, ITreeNode statement)
        {
            if (statement == null || !statement.IsValid())
            {
                yield break;
            }
            var result = matcher.Match(statement);
            if (result.Matched)
            {
                yield return result;
                yield break;
            }
            foreach (var r in statement.Children().SelectMany(node => FindMatches(matcher, node)))
            {
                yield return r;
            }
        }

        private static bool IsNotAssignmentDestination(ITreeNode node)
        {
            var assignmentExpression = node.Parent as IAssignmentExpression;
            return assignmentExpression == null || node != assignmentExpression.Dest;
        }

        public ITreeNode[] GetMatchingDictionaryAccess(ITreeNode statement, ITreeNode dictionary, ITreeNode key)
        {
            return FindMatches(_dictionaryAccess.Value, statement)
                .Where(r => AreSame(dictionary, r.GetMatchedElement("dictionary")) &&
                            AreSame(key, r.GetMatchedElement("key")))
                .TakeWhile(r => IsNotAssignmentDestination(r.MatchedElement))
                .Select(r => r.MatchedElement)
                .ToArray();
        }

        private static bool AreSame(ITreeNode x, ITreeNode y)
        {
            var literalX = x as ILiteralExpression;
            var literalY = y as ILiteralExpression;
            if (literalX != null && literalY != null)
            {
                return Equals(literalX.ConstantValue.Value, literalY.ConstantValue.Value);
            }
            var referenceX = x as IReferenceExpression;
            var referenceY = y as IReferenceExpression;
            if (referenceX != null && referenceY != null)
            {
                return Equals(referenceX.Reference.Resolve().DeclaredElement, referenceY.Reference.Resolve().DeclaredElement);
            }
            return false;
        }

        public IEnumerable<IStructuralMatchResult> GetMatchingContainsKey(ITreeNode expression)
        {
            return FindMatches(_containsKey.Value, expression);
        }
    }
}
