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
			"Tests",
			[syntaxTree],
			references);

		var generator = new UnionGenerator();

		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		driver = driver.RunGenerators(compilation);

		var settings = new VerifySettings();
		settings.UseUniqueDirectory();

		return Verifier.Verify(driver, settings).UseDirectory("Snapshots");
	}
}