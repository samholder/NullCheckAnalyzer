using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NullCheckAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NullCheckAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NullCheckAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private const string Category = "Naming";
        internal const string Title = "Improve readability";
        internal const string MessageFormat = "Replace with call to .Exists()";
        internal const string Description = "Use convenience methods for null checking.";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.EqualsExpression);
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find

            var binaryExpressionSyntax = (BinaryExpressionSyntax)context.Node;

            if (binaryExpressionSyntax.OperatorToken.Kind() == SyntaxKind.EqualsEqualsToken &&
                binaryExpressionSyntax.Right.Kind() == SyntaxKind.NullLiteralExpression)
            {
                var diagnostic = Diagnostic.Create(Rule, binaryExpressionSyntax.GetLocation());

                context.ReportDiagnostic(diagnostic);
            }

            
        }
    }
}
