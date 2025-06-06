namespace CoreMachine.Result.Test;

public class ResultTest
{
	[Fact]
	public void ResultDoesNotThrowIfNull()
	{
		_ = Result.Ok<string, string>(null!);
		_ = Result.Err<string, string>(null!);
	}

	[Fact]
	public void AssertReturnsDifferentErrors()
	{
		Result<string, string> beforeAssert = Result.Err<string, string>("before assert")
			.Assert(assert: val => val.StartsWith("Hello!"), error: "This should not be reached");

		Result<string, string> afterAssert = Result.Ok<string, string>("This is ok")
			.Assert(assert: val => val.StartsWith("Hello!"), error: "after assert");

		string beforeResult = beforeAssert.Match(ok: ok => ok, err: err => err);
		string afterResult = afterAssert.Match(ok: ok => ok, err: err => err);

		Assert.Equal(expected: "before assert", actual: beforeResult);
		Assert.Equal(expected: "after assert", actual: afterResult);

		Result<int, string> result = Result.Ok<int, string>(5)
			.Assert(assert: ok => ok > 10, error: "Result is lower than 10");

		_ = "Hello!".ValueOr(new { Error = "Value is null" });
		Assert.Equal(expected: "Result is lower than 10", actual: result);
	}
}
