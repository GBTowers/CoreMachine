using CoreMachine.UnionLike.Data;
using Microsoft.CodeAnalysis;

namespace CoreMachine.UnionLike;

public sealed record UnionToGenerate(
    string Namespace,
    string Name,
    EquatableArray<UnionMemberToGenerate> Members,
    SyntaxTokenList Modifiers
);