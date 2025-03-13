namespace CoreMachine.Result.Test;

public class Playground
{
    [Fact]
    public void Play()
    {
        var result = Result.Ok<int, string>(5);
        if (result is Ok<int, string> val)
        {
            Assert.Equal(10, val + 5);
        }

    }
}