using System.Collections.Generic;
using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;
#if !RESHARPER9
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Feature.Services.Bulbs;
#else
using JetBrains.ReSharper.Feature.Services.QuickFixes;
#endif

namespace ReSharper.DictionaryHelper
{
    [QuickFix]
    public class DictionaryContainsKeyFix : QuickFixBase
    {
        private readonly ICSharpStatement _statement;
        private readonly ITreeNode _matchedElement;
        private readonly ITreeNode[] _dictionaryAccess;
        private readonly IExpression _dictionary;
        private readonly ITreeNode _key;

        public DictionaryContainsKeyFix(DictionaryContainsKeyWarning warning)
        {
            _statement = warning.Statement;
            _matchedElement = warning.MatchedElement;
            _dictionaryAccess = warning.DictionaryAccess;
            _dictionary = warning.Dictionary;
            _key = warning.Key;
        }

        public override string Text => "Optimize access to dictionary";

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var factory = CSharpElementFactory.GetInstance(_statement);

            var valueType = GuessValueType(_dictionary) ?? "var";

            var valueVariableName = SuggestVariableName(_statement, "value");
            var valueReference = factory.CreateReferenceExpression(valueVariableName);
            var valueDeclaration = factory.CreateStatement("$0 $1;", valueType, valueVariableName);
            var newCondition = factory.CreateExpression("$0.TryGetValue($1, out $2)", _dictionary, _key, valueReference);

            using (_statement.CreateWriteLock())
            {
                _dictionaryAccess.Where(x => x.IsValid()).ForEach(e => ModificationUtil.ReplaceChild(e, valueReference));
                ModificationUtil.ReplaceChild(_matchedElement, newCondition);
                if (_statement.Parent is IBlock)
                {
                    ModificationUtil.AddChildBefore(_statement, valueDeclaration);
                }
                else
                {
                    _statement.ReplaceBy(factory.CreateBlock("{$0$1}", valueDeclaration, _statement));
                }
            }

            return null;
        }

        private static string SuggestVariableName(ITreeNode context, string name)
        {
            return context.GetPsiServices().Naming.Suggestion
                .GetDerivedName(name,
                    NamedElementKinds.Locals,
                    ScopeKind.Common,
                    context.Language,
                    new SuggestionOptions
                    {
                        DefaultName = name,
                        UniqueNameContext = context
                    },
                    context.GetSourceFile());
        }

        private static object GuessValueType(IExpression dictionary)
        {
            var declaredType = dictionary.Type() as IDeclaredType;
            if (declaredType == null)
                return null;

            return CollectionTypeUtil.GetKeyValueTypesForGenericDictionary(declaredType)?
                .Select(x => x.Second)
                .FirstOrDefault();
        }

        public override bool IsAvailable(IUserDataHolder cache) => true;
    }
}
