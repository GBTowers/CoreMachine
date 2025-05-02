using System.Collections.Immutable;
using CoreMachine.UnionLike.Core;
using CoreMachine.UnionLike.Data;
using CoreMachine.UnionLike.Extensions;
using CoreMachine.UnionLike.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoreMachine.UnionLike;

[Generator]
public class UnionGenerator : IIncrementalGenerator
{
	private const string FullyQualifiedAttributeName = "CoreMachine.UnionLike.Attributes.UnionAttribute"; 
	
	
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<UnionToGenerate> pipeline = context
			.SyntaxProvider
			.ForAttributeWithMetadataName(FullyQualifiedAttributeName,
				SyntaxPredicate,
				SemanticTransform)
			.Where(u => u is not null)!;

		IncrementalValueProvider<ImmutableArray<int>> arities = pipeline.Collect()
			.Select((u, _)
				=> u.Select(x => x.Members.Count()).Distinct().ToImmutableArray());

		context.RegisterImplementationSourceOutput(arities, ArityCore.GenerateArities);

		context.RegisterImplementationSourceOutput(pipeline, UnionCore.BuildUnion);
	}

	private static UnionToGenerate? SemanticTransform(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	{
		if (context.TargetNode is not RecordDeclarationSyntax candidate) return null;


		List<UnionMemberToGenerate> candidateMembers = [];

		foreach (var member in candidate.Members)
		{
			if (member is not RecordDeclarationSyntax recordMember
			    || !recordMember.Modifiers.Any(SyntaxKind.PartialKeyword)
			    || recordMember.Modifiers.Any(SyntaxKind.PrivateKeyword))
				continue;

			RecordConstructor? constructor = null;
			if (recordMember.ParameterList?.Parameters is { Count: > 0 } parameterList)
			{
				EquatableArray<ConstructorParameter> parameters = parameterList
					.Select(p => new ConstructorParameter(p.Type?.ToString() ?? string.Empty, p.Identifier.Text))
					.ToImmutableArray();

				constructor = new RecordConstructor(parameters);
			}


			var modifiers = recordMember.Modifiers;
			if (!modifiers.Any(SyntaxKind.SealedKeyword))
				modifiers = modifiers.Insert(
					Math.Max(modifiers.IndexOf(SyntaxKind.PartialKeyword) - 1, 0),
					SyntaxFactory.Token(SyntaxKind.SealedKeyword));

			if (!modifiers.Any(SyntaxKind.PublicKeyword) && !modifiers.Any(SyntaxKind.InternalKeyword))
				modifiers = modifiers.Insert(0,
					SyntaxFactory.Token(SyntaxKind.PublicKeyword));

			candidateMembers.Add(new UnionMemberToGenerate(
				recordMember.Identifier.Text,
				string.Join(" ", modifiers.RearrangeKeywords()),
				constructor,
				ExtractTypeParameters(recordMember))
			);
		}

		EquatableArray<UnionMemberToGenerate> members = candidateMembers.ToImmutableArray();

		return new UnionToGenerate(
			context.TargetSymbol.ContainingNamespace.ToDisplayString(),
			context.TargetSymbol.Name,
			string.Join(" ", candidate.Modifiers),
			members,
			ExtractTypeParameters(candidate)
		);
	}

	private static ImmutableArray<string> ExtractTypeParameters(RecordDeclarationSyntax recordMember)
	{
		return recordMember.TypeParameterList?.Parameters
			.Select(p => p.Identifier.Text).ToImmutableArray() ?? [];
	}

	private static bool SyntaxPredicate(SyntaxNode node, CancellationToken token) =>
		node is RecordDeclarationSyntax candidate && candidate.Modifiers.Any(SyntaxKind.PartialKeyword);
}
