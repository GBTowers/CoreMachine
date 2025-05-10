using CoreMachine.UnionLike.Attributes;

namespace CoreMachine.UnionLike.Integration;

[Union]
public abstract partial record Notification
{
	partial record Hello<E, R>;
	partial record Hello<T>;
	partial record Bye;
}




public class Playground
{
	[Fact]
	public async Task Play()
	{
		Notification notification = new Notification.Bye();

		var result = Task.FromResult("Hello");

		var x =  notification.MatchAsync(async b => await result);
	}
}