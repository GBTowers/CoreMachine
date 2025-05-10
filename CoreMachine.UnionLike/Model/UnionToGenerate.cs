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
	public string GenericDeclaration =>
		TypeParameters.JoinString() is not { Length: > 0 } parameters ? "" : $"<{parameters}>";

	public string UnionDeclaration =>
		$"Union<{FullName}, {Members.JoinSelect(m => m.FullName)}>";
	
	public string FullName => Name + GenericDeclaration;
}
