using CoreMachine.UnionLike.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CoreMachine.UnionLike.Tests;

public static class UnionGeneratorTester
{
	public static Task Verify(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);

		IEnumerable<PortableExecutableReference> references =
		[
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(UnionAttribute).Assembly.Location)
		];

		var compilation = CSharpCompilation.Create(
			"Tests",
			[syntaxTree],
			references);

		GeneratorDriver driver = CSharpGeneratorDriver.Create(new UnionGenerator());

		return Verifier.Verify(driver.RunGenerators(compilation)).UseDirectory("Snapshots").UseUniqueDirectory();
	}
}
