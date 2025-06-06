using CoreMachine.UnionLike.Extensions;
using CoreMachine.UnionLike.Data;

namespace CoreMachine.UnionLike.Model;

public sealed record UnionTargetMember(
	string Name,
	string ParentName,
	RecordConstructor? Constructor
)
{
	public string TupleConstructor
		=> Constructor is not null && Constructor.Parameters.Any()
			? Enumerable.Range(start: 1, count: Constructor.Parameters.Count()).JoinSelect(i => $"tuple.Item{i}")
			: "";

	public string VariableName => Name.FirstCharToLower()!;

	public string FullName => ParentName + '.' + Name;
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
