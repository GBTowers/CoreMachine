using CoreMachine.UnionLike.Data;
using CoreMachine.UnionLike.Extensions;

namespace CoreMachine.UnionLike.Model;

public sealed record UnionMemberToGenerate(
	string Name,
	string Modifiers,
	RecordConstructor? Constructor,
	EquatableArray<string> TypeParameters)
{
	public string VariableName => Name.ToLower();

	public string TupleConstructor => Constructor is not null && Constructor.Parameters.Any()
		? Enumerable.Range(1, Constructor.Parameters.Count())
			.JoinSelect(i => $"tuple.Item{i}")
		: "";

	public string FullName(string parentName) => parentName + '.' + Name;
}

public sealed record RecordConstructor(EquatableArray<ConstructorParameter> Parameters)
{
	public string TupleSignature => $"({Parameters.JoinSelect(p => p.Type)})";
	public string ParametersSignature => $"({Parameters.JoinSelect(p => p.Name)})";

	public override string ToString() => $"({Parameters.JoinString()})";
}

public sealed record ConstructorParameter(string Type, string Name)
{
	public override string ToString() => $"{Type} {Name}";
}
