using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoreMachine.UnionLike.Extensions;

public static class SyntaxKindEx
{
	private static bool IsAccessModifier(this SyntaxKind kind)
		=> kind is SyntaxKind.PublicKeyword
		or SyntaxKind.PrivateKeyword
		or SyntaxKind.ProtectedKeyword
		or SyntaxKind.InternalKeyword
		or SyntaxKind.FileKeyword;
	
	public static bool HasNonInterfaceBaseType(this BaseTypeDeclarationSyntax type, SemanticModel semanticModel)
		=> type.BaseList?.Types is { Count: > 0 } baseList
		&& semanticModel.GetSymbolInfo(baseList.First().Type).Symbol is not ITypeSymbol { TypeKind: TypeKind.Interface };


	public static bool IsNonPublic(this BaseTypeDeclarationSyntax type) => type.Modifiers.Any(IsNonPublicAccessibility);

	private static bool IsNonPublicAccessibility(this SyntaxToken token)
		=> token.Kind() is SyntaxKind.PrivateKeyword
		or SyntaxKind.InternalKeyword
		or SyntaxKind.ProtectedKeyword
		or SyntaxKind.FileKeyword;

	public static SyntaxTokenList RearrangeKeywords(this SyntaxTokenList source)
		=> new(source.OrderByDescending(token => token.Kind().IsAccessModifier()));

	public static SyntaxTokenList WithSealedKeyword(this SyntaxTokenList source)
		=> source.Any(SyntaxKind.SealedKeyword)
			? source
			: source.Insert(
				index: Math.Max(val1: source.IndexOf(SyntaxKind.PartialKeyword) - 1, val2: 0),
				token: SyntaxFactory.Token(SyntaxKind.SealedKeyword)
			);

	public static SyntaxTokenList WithPublicKeyword(this SyntaxTokenList source)
		=> source.Any(SyntaxKind.PublicKeyword)
			? source
			: source.Insert(index: 0, token: SyntaxFactory.Token(SyntaxKind.PublicKeyword));
}
