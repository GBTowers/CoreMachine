namespace CoreMachine.UnionLike.Tests;



public class UnionGeneratorTests
{

	[Fact]
	public Task GeneratesPartialClassWhenAttributeIsPresent()
	{
		const string code = """
		                    using System;
		                    using CoreMachine.UnionLike.Attributes;

		                    namespace Tests;

		                    [Union]
		                    public partial record ApiResult
		                    {
		                        partial record Ok(string Message);
		                        partial record BadRequest(string Message);
		                    }

		                    """;

		return UnionGeneratorTester.Verify(code);
	}

	[Fact]
	public Task GeneratesUnionAttributeUnconditionally()
	{
		const string code = """
		                    using System;
		                    using CoreMachine.UnionLike.Attributes;

		                    namespace Tests;

		                    public partial record ApiResult
		                    {
		                        private partial record Ok;
		                    }    

		                    """;


		return UnionGeneratorTester.Verify(code);
	}
}

public class VerifyChecksTests
{
	[Fact]
	public Task Run() =>
		VerifyChecks.Run();
}