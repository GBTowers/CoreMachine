using CoreMachine.UnionLike.Attributes;

namespace CoreMachine.UnionLike.Integration;

[Union]
public abstract partial record Notification
{
	partial record StatusNotification(string Message, int Code);
	partial record PaymentNotification(decimal Amount, string Account);
}

public class Playground
{
	[Fact]
	public void Play()
	{
		
	}
}
