namespace CoreMachine.Result.Test;

public class Playground
{
	[Fact]
	public void Play()
	{
		Result<int, string> result = Result.Ok<int, string>(5);

		if (result is IOk<int>(var num)) Assert.Equal(expected: 10, actual: num + 5);

		Result<int, string> errorResult = Result.Err<int, string>("This is an error");
		if (errorResult is IErr<string>(var error)) Assert.Equal(expected: "This is an error", actual: error);

		string match = result switch
		{
			IOk<int>(var value) => $"Value is {value}",
			IErr<string>(var err) => err,
			_ => throw new InvalidOperationException()
		};

		string match2 = result.Match(ok: ok => $"Value is {ok}", err: err => err);

		Assert.Equal(expected: "Value is 5", actual: match);
		Assert.Equal(expected: "Value is 5", actual: match2);
	}
}
