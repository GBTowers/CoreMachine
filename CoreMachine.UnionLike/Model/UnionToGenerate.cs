using CoreMachine.UnionLike.Data;
using CoreMachine.UnionLike.Extensions;

namespace CoreMachine.UnionLike.Model;

public sealed record UnionToGenerate(
	string Namespace,
	string Name,
	string Modifiers,
	EquatableArray<UnionMemberToGenerate> Members,
	EquatableArray<string> TypeParameters
)
{
	public string GenericDeclaration => TypeParameters.JoinString();
	public string FullName => Name + (string.IsNullOrWhiteSpace(GenericDeclaration) ? "" : $"<{GenericDeclaration}>");
}
