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
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
		];
		
		var compilation = CSharpCompilation.Create(
			assemblyName: "Tests",
			syntaxTrees: [syntaxTree],
			references: references);
		
		var generator = new UnionGenerator();

		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		driver = driver.RunGenerators(compilation);

		return Verifier.Verify(driver);
	}
}