namespace CoreMachine.Result.Test;

public class Playground
{
	[Fact]
	public void Play()
	{
		Result<int, string> result = Result.Ok<int, string>(5);
		if (result is IOk<int> success)
		{
			Assert.Equal(10, success.Value + 5);
		}

		var errorResult = Result.Err<int, string>("This is an error");
		if (errorResult is IErr<string> error)
		{
			Assert.Equal("This is an error", error.Error);
		}

		var match = result switch
		{
			IOk<int> ok => $"Value is {ok.Value}",
			IErr<string> err => err.Error,
			_ => throw new ArgumentOutOfRangeException()
		};

		var match2 = result.Match(
			ok => $"Value is {ok}",
			err => err);

		Assert.Equal("Value is 5", match);
		Assert.Equal("Value is 5", match2);
	}
}