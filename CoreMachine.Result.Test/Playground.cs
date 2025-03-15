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
        
        string match = result switch
        {
	        Err<int, string> err => err.Error,
	        Ok<int, string> ok => $"Value is {ok.Value}"
        };

        string match2 = result.Match(
            ok => $"Value is {ok}",
            err => err);
        
        Assert.Equal("Value is 5", match);
        Assert.Equal("Value is 5", match2);
    }
}