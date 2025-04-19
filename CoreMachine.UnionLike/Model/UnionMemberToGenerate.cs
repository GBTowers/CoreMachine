using CoreMachine.UnionLike.Data;

namespace CoreMachine.UnionLike.Model;

public sealed record UnionMemberToGenerate(
    string Name,
    string Modifiers,
    RecordConstructor? Constructor)
{
    public string FullName(string parentName) => parentName + '.' + Name;
    public string VariableName => Name.ToLower();
}

public sealed record RecordConstructor(EquatableArray<ConstructorParameter> Parameters);

public sealed record ConstructorParameter(string? Type, string Name);