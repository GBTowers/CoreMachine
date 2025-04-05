using System.Diagnostics;
using System.Text;
using CoreMachine.UnionLike.Data;
using CoreMachine.UnionLike.Diagnosis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CoreMachine.UnionLike;

[Generator]
public class UnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(InitializationCallback);

        var pipeline = context
            .SyntaxProvider
            .ForAttributeWithMetadataName(GenerationCore.FullyQualifiedAttributeName, 
                                          SyntaxPredicate, 
                                          SemanticTransform);
    }

    private static Result<UnionToGenerate, string> SemanticTransform(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token)
    {
        if (context.TargetNode is not RecordDeclarationSyntax candidate)
        {
            return "invalid";
        }

        if (context.TargetSymbol is INamedTypeSymbol namedType)
        {
            
        }
        
        return "hello";
    }

    
    
    private sealed record UnionToGenerate(string Name, EquatableArray<string> PrivateRecordMembers);

    private static bool SyntaxPredicate(SyntaxNode node, CancellationToken token)
    {
        return node is RecordDeclarationSyntax
               {
                   AttributeLists.Count: > 0
               } candidate
               && candidate.Modifiers.Any(SyntaxKind.PartialKeyword)
               && candidate.Modifiers.Any(SyntaxKind.AbstractKeyword);
    }

    private static void InitializationCallback(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource("Roslyn.Generated.UnionAttribute.g.cs",
            SourceText.From(GenerationCore.UnionAttribute, Encoding.UTF8));
    }
}