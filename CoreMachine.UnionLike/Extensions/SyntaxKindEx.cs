using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CoreMachine.UnionLike.Extensions;

public static class SyntaxKindEx
{
	public static bool IsAccessModifier(this SyntaxKind kind)
		=> kind is SyntaxKind.PublicKeyword
		or SyntaxKind.PrivateKeyword
		or SyntaxKind.ProtectedKeyword
		or SyntaxKind.InternalKeyword
		or SyntaxKind.FileKeyword;

	public static SyntaxTokenList RearrangeKeywords(this SyntaxTokenList source)
		=> new(source.OrderByDescending(token => token.Kind().IsAccessModifier()));
}
