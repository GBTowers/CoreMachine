namespace CoreMachine.Result.Test;

public class Playground
{
    [Fact]
    public void Play()
    {
        var result = Result.Ok<int, string>(5);
        if (result is IOk<int> ok)
        {
            Assert.Equal(10, ok.Value + 5);
        }

        var errorResult = Result.Error<int, string>("This is an error");
        if (errorResult is IErr<string> err)
        {
            Assert.Equal("This is an error", err.Error);
        }
    }
}