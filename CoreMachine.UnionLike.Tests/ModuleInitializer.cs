using System.Runtime.CompilerServices;

namespace CoreMachine.UnionLike.Tests;

public static class ModuleInitializer
{
	[ModuleInitializer]
	public static void Initialize()
	{
		VerifySourceGenerators.Initialize();
		VerifyDiffPlex.Initialize();
	}
}
