using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CoreMachine.UnionLike;

[Generator]
public class UnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(InitializationCallback);
    }

    private static void InitializationCallback(IncrementalGeneratorPostInitializationContext ctx)
    {
        ctx.AddSource("Roslyn.Generated.UnionAttribute.g.cs",
            SourceText.From(GenerationCore.UnionAttribute, Encoding.UTF8));
    }
}