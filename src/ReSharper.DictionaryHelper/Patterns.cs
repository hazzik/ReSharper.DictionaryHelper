using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch.Placeholders;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;
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
                new IdentifierPlaceholder("a"),
                new ExpressionPlaceholder("dictionary", "System.Collections.Generic.IDictionary<,>"),
                new ExpressionPlaceholder("key")).CreateMatcher());
        }

        public IStructuralMatcher ContainsKeyMatcher
        {
            get { return _containsKey.Value; }
        }

        public IStructuralMatcher DictionaryAccessMatcher
        {
            get { return _dictionaryAccess.Value; }
        }

        private IEnumerable<IStructuralMatchResult> FindDictionaryAccess(ITreeNode statement)
        {
            var result = DictionaryAccessMatcher.Match(statement);
            if (result.Matched)
            {
                yield return result;
                yield break;
            }
            foreach (var r in statement.Children().SelectMany(FindDictionaryAccess))
            {
                yield return r;
            }
        }

        private IEnumerable<IStructuralMatchResult> GetDictionaryAccess(ITreeNode statement)
        {
            var assignment = statement as IAssignmentExpression;
            if (assignment != null)
            {
                foreach (var assignmentResult in FindDictionaryAccess(assignment.Source))
                {
                    yield return assignmentResult;
                }
                yield break;
            }
            var initializer = statement as IExpressionInitializer;
            if (initializer != null)
            {
                foreach (var initializerResult in FindDictionaryAccess(initializer.Value))
                {
                    yield return initializerResult;
                }
                yield break;
            }

            foreach (var r in statement.Children().SelectMany(GetDictionaryAccess))
            {
                yield return r;
            }
        }

        public IEnumerable<ITreeNode> GetMatchingDictionaryAccess(ITreeNode statement, ITreeNode dictionary, ITreeNode key)
        {
            return GetDictionaryAccess(statement)
                .Where(x => AreSame(dictionary, x.GetMatchedElement("dictionary")) &&
                            AreSame(key, x.GetMatchedElement("key")))
                .Select(x => x.MatchedElement);
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
    }
}
