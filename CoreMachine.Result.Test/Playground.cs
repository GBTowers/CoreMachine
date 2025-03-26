namespace CoreMachine.Result.Test;

public class Playground
{
	[Fact]
	public void Play()
	{
		Result<int, string> result = Result.Ok<int, string>(5);
		
		if (result is IOk<int>(var num))
		{
			Assert.Equal(10, num + 5);
		}
		
		var errorResult = Result.Err<int, string>("This is an error");
		if (errorResult is IErr<string>(var error))
		{
			Assert.Equal("This is an error", error);
		}

		string match = result switch
		{
			IOk<int>(var value) => $"Value is {value}",
			IErr<string>(var err) => err,
			_ => throw new InvalidOperationException()
		};

		string match2 = result.Match(
			ok => $"Value is {ok}",
			err => err);

		Assert.Equal("Value is 5", match);
		Assert.Equal("Value is 5", match2);
	}
}