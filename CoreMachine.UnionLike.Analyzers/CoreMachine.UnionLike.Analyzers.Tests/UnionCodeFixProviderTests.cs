using System.Threading.Tasks;
using CoreMachine.UnionLike.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace CoreMachine.UnionLike.Analyzers.Tests;

public class MissingPartialKeywordCodeFixProviderTests
{
	[Fact]
	public async Task UL1001_AddPartialKeyword()
	{
		const string text = """
			using CoreMachine.UnionLike.Attributes;

			namespace Test;

			[Union]
			public record [|Result|]<T, TE>
			{
				partial record Ok(T Value);
				partial record Err(TE Error);
			}

			""";

		const string newText = """
			using CoreMachine.UnionLike.Attributes;

			namespace Test;

			[Union]
			public partial record Result<T, TE>
			{
				partial record Ok(T Value);
				partial record Err(TE Error);
			}

			""";

		var context = new CSharpCodeFixTest<UnionGenerationSyntaxAnalyzer, MissingPartialKeywordCodeFixProvider, DefaultVerifier>
		{
			TestCode = text,
			FixedCode = newText,
			ReferenceAssemblies = ReferenceAssemblies.Net.Net50.AddPackages(
				[new PackageIdentity(id: "UnionLike", version: "2.0.0")]
			)
		};

		await context.RunAsync();
	}
}
