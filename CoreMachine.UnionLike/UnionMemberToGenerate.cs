using CoreMachine.UnionLike.Data;
using Microsoft.CodeAnalysis;

namespace CoreMachine.UnionLike;

public sealed record UnionMemberToGenerate(
    string Name,
    string Modifiers,
    EquatableArray<string> TypeParameters,
    EquatableArray<RecordConstructor> Constructors)
{
    public string TypeDeclaration => TypeParameters.Any() ? '<' + TypeParameters.JoinString() + '>' : "";
    public string FullName => Name + TypeDeclaration;
    public string GetFullyQualifiedName(string parentName) => parentName + '.' + FullName;
}

public sealed record RecordConstructor(
    SyntaxTokenList Modifiers,
    EquatableArray<ConstructorParameter> Parameters);

public sealed record ConstructorParameter(
    string? Type,
    string Name);