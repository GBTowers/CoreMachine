using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace CoreMachine.UnionLike.Analyzers.Tests;

public class UnionSyntaxAnalyzerTests
{
	[Fact]
	public async Task UL1001_UnionMissingPartialKeyword()
	{
		const string text = """
			using CoreMachine.UnionLike.Attributes;

			namespace Test;

			[Union]
			public {|UL1001:record Result|}<T, TE>
			{
				partial record Ok(T Value);
				partial record Err(TE Error);
			}

			""";

		var context = new CSharpAnalyzerTest<UnionGenerationSyntaxAnalyzer, DefaultVerifier>
		{
			TestCode = text,
			ReferenceAssemblies = ReferenceAssemblies.Net.Net50.AddPackages(
				[new PackageIdentity(id: "UnionLike", version: "2.0.0")]
			)
		};

		await context.RunAsync();
	}
	
	[Fact]
	public async Task UL1002_ParentMissingPartialKeyword()
	{
		const string text = """
			using CoreMachine.UnionLike.Attributes;

			namespace Test;

			public {|UL1002:class ParentClass|}
			{
				[Union]
				public partial record Result<T, TE>
				{
					partial record Ok(T Value);
					partial record Err(TE Error);
				}
			}
			""";

		var context = new CSharpAnalyzerTest<UnionGenerationSyntaxAnalyzer, DefaultVerifier>
		{
			TestCode = text,
			ReferenceAssemblies = ReferenceAssemblies.Net.Net50.AddPackages(
				[new PackageIdentity(id: "UnionLike", version: "2.0.0")]
			)
		};

		await context.RunAsync();
	}
}
