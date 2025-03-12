namespace CoreMachine.Result.Test;

public class ResultTest
{
    [Fact]
    public void ResultThrowsIfNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var r = Result.Ok<string, string>(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var r = Result.Error<string, string>(null!);
        });
    }

    [Fact]
    public void AssertReturnsDifferentErrors()
    {
        var beforeAssert = Result.Error<string, string>("before assert")
            .Assert(val => val.StartsWith("Hello!"), "This should not be reached");
        var afterAssert = Result.Ok<string, string>("This is ok")
            .Assert(val => val.StartsWith("Hello!"), "after assert");

        var beforeResult = beforeAssert.Match(ok => ok, err => err);
        var afterResult = afterAssert.Match(ok => ok, err => err);
        
        Assert.Equal("before assert", beforeResult);
        Assert.Equal("after assert", afterResult);
    }
}