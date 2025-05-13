using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CoreMachine.UnionLike.Attributes;
using CoreMachine.UnionLike.Composers;
using CoreMachine.UnionLike.Data;
using CoreMachine.UnionLike.Extensions;
using CoreMachine.UnionLike.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoreMachine.UnionLike;

[Generator]
public class UnionGenerator : IIncrementalGenerator
{
	private const string AsyncExtensionsOption = "build_property.AsyncUnionExtensions";
	private const string FullyQualifiedAttributeName = "CoreMachine.UnionLike.Attributes.UnionAttribute";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValueProvider<GeneratorOptions> options = context.AnalyzerConfigOptionsProvider.Select(ParseOptions);

		IncrementalValuesProvider<UnionTarget> pipeline = context.SyntaxProvider
		.ForAttributeWithMetadataName(FullyQualifiedAttributeName, SyntaxPredicate, SemanticTransform)
		.Where(u => u is not null)
		.Where(u => u?.Members.Any() == true)!;

		IncrementalValuesProvider<Union> unions = pipeline.Collect()
		.SelectMany((u, _) => u.Select(x => x.Members.Count()).Distinct().Select((arity, _) => new Union(arity)));

		context.RegisterImplementationSourceOutput(unions, AritySourceComposer.BuildArity);

		context.RegisterImplementationSourceOutput(pipeline.Combine(options), UnionSourceComposer.BuildUnion);
	}

	private static GeneratorOptions ParseOptions(AnalyzerConfigOptionsProvider options, CancellationToken _)
	{
		bool generateAsyncExtensions = options.GlobalOptions.TryGetValue(AsyncExtensionsOption, out string? value)
		&& value switch
			{
				"enable" or "enabled" or "true" => true,
				"disable" or "disabled" or "false" => true,
				_ => false
			};

		return new GeneratorOptions(generateAsyncExtensions);
	}

	private static UnionTarget? SemanticTransform(GeneratorAttributeSyntaxContext context, CancellationToken token)
	{
		if (context.TargetNode is not RecordDeclarationSyntax candidate) return null;

		ImmutableArray<string> candidateTypeParameters = ExtractTypeParameters(candidate);

		string candidateGenericDeclaration = candidateTypeParameters.JoinString() is { Length: > 0 } typeParams
			? $"<{typeParams}>"
			: "";

		List<UnionTargetMember> candidateMembers = [];

		foreach (var member in candidate.Members)
		{
			if (member is not RecordDeclarationSyntax recordMember) continue;
			if (!recordMember.Modifiers.Any(SyntaxKind.PartialKeyword)) continue;
			if (recordMember.Modifiers.Any(SyntaxKind.PrivateKeyword)) continue;
			if (recordMember.TypeParameterList?.Parameters.Count > 0) continue;

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
					Math.Max(modifiers.IndexOf(SyntaxKind.PartialKeyword) - 1, val2: 0),
					SyntaxFactory.Token(SyntaxKind.SealedKeyword)
				);

			if (!modifiers.Any(SyntaxKind.PublicKeyword) && !modifiers.Any(SyntaxKind.InternalKeyword))
				modifiers = modifiers.Insert(index: 0, SyntaxFactory.Token(SyntaxKind.PublicKeyword));

			candidateMembers.Add(
				new UnionTargetMember(
					recordMember.Identifier.Text,
					context.TargetSymbol.Name + candidateGenericDeclaration,
					string.Join(" ", modifiers.RearrangeKeywords()),
					constructor
				)
			);
		}

		EquatableArray<UnionTargetMember> members = candidateMembers.ToImmutableArray();

		bool generateAsyncExtensions = context.Attributes
		.FirstOrDefault(attribute => attribute.AttributeClass?.Name == nameof(UnionAttribute))
		?.NamedArguments.FirstOrDefault(x => x.Key == nameof(UnionAttribute.GenerateAsyncExtensions))
		.Value.Value as bool? is true;

		return new UnionTarget(
			context.TargetSymbol.ContainingNamespace.ToDisplayString(),
			context.TargetSymbol.Name,
			string.Join(" ", candidate.Modifiers),
			members,
			candidateTypeParameters,
			generateAsyncExtensions,
			ExtractUsings(context.TargetNode),
			GetParentClasses(candidate)
		);
	}

	private static ParentType? GetParentClasses(SyntaxNode typeSyntax)
	{
		var parentSyntax = typeSyntax.Parent as TypeDeclarationSyntax;

		ParentType? parentInfo = null;
		while (parentSyntax != null && IsAllowedKind(parentSyntax.Kind()))
		{
			parentInfo = new ParentType(
				parentSyntax.Keyword.ValueText,
				parentSyntax.Identifier.ToString() + parentSyntax.TypeParameterList,
				parentSyntax.ConstraintClauses.ToString(),
				parentInfo
			);

			parentSyntax = parentSyntax.Parent as TypeDeclarationSyntax;
		}

		return parentInfo;

		static bool IsAllowedKind(SyntaxKind kind)
			=> kind is SyntaxKind.ClassDeclaration or SyntaxKind.StructDeclaration or SyntaxKind.RecordDeclaration;
	}

	[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
	private static ImmutableArray<string> ExtractUsings(SyntaxNode target)
	{
		SyntaxList<UsingDirectiveSyntax> allUsings = SyntaxFactory.List<UsingDirectiveSyntax>();
		foreach (var ancestor in target.Ancestors(ascendOutOfTrivia: false))
			allUsings = ancestor switch
			{
				BaseNamespaceDeclarationSyntax namespaceDeclaration => allUsings.AddRange(namespaceDeclaration.Usings),
				CompilationUnitSyntax compilationUnit => allUsings.AddRange(compilationUnit.Usings),
				_ => allUsings
			};

		return [..allUsings.Select(u => u.ToString())];
	}

	private static ImmutableArray<string> ExtractTypeParameters(RecordDeclarationSyntax recordMember)
		=> recordMember.TypeParameterList?.Parameters.Select(p => p.Identifier.Text).ToImmutableArray() ?? [];

	private static bool SyntaxPredicate(SyntaxNode node, CancellationToken token)
		=> node is RecordDeclarationSyntax candidate && candidate.Modifiers.Any(SyntaxKind.PartialKeyword);
}
