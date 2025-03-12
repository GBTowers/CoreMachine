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
    
    
}