namespace CoreMachine.Result.Test;

public class ResultTest
{
    [Fact]
    public void ResultThrowsIfNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = Result.Ok<string, string>(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = Result.Err<string, string>(null!);
        });
    }

    [Fact]
    public void CannotInheritFromResultOutsideAssembly()
    {
	    var r = new TestResult<int, string>();
	    r.Assert(ok => ok >= 0, "Hello");
    }

    [Fact]
    public void AssertReturnsDifferentErrors()
    {
        var beforeAssert = Result.Err<string, string>("before assert")
            .Assert(val => val.StartsWith("Hello!"), "This should not be reached");
        var afterAssert = Result.Ok<string, string>("This is ok")
            .Assert(val => val.StartsWith("Hello!"), "after assert");

        var beforeResult = beforeAssert.Match(ok => ok, err => err);
        var afterResult = afterAssert.Match(ok => ok, err => err);
        
        Assert.Equal("before assert", beforeResult);
        Assert.Equal("after assert", afterResult);

        var result = Result.Ok<int, string>(5).Assert(ok => ok > 10, "Result is lower than 10");
        
        var r = "Hello!".ValueOr(new { Error = "Value is null" });
        Assert.Equal("Result is lower than 10", result);
    }
}

public record TestResult<T, TE> : Result<T, TE>
{
	public TestResult()
	{
		
	}
}