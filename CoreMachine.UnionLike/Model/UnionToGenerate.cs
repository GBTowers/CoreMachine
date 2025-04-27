using CoreMachine.UnionLike.Data;
using Microsoft.CodeAnalysis;

namespace CoreMachine.UnionLike.Model;

public sealed record UnionToGenerate(
	string Namespace,
	string Name,
	EquatableArray<UnionMemberToGenerate> Members,
	SyntaxTokenList Modifiers,
	EquatableArray<string> TypeParameters
)
{
	public string GenericDeclaration => TypeParameters.JoinString();
	public string FullName => Name + (string.IsNullOrWhiteSpace(GenericDeclaration) ? "" : $"<{GenericDeclaration}>");
}