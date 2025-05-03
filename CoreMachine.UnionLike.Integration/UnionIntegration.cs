using CoreMachine.UnionLike.Attributes;

namespace CoreMachine.UnionLike.Integration;

[Union]
public abstract partial record Result<T, TE>
{
	partial record Ok(T Value)
	{
		public static implicit operator T(Ok ok) => ok.Value;
	}

	partial record Err(TE Error)
	{
		public static implicit operator TE(Err err) => err.Error;
	}
	
}

public class UnionIntegration
{
	[Fact]
	public void MatchMethodWorksOnDerivedClasses()
	{
		Result<int, string> result = 4;
		
		string s = result.Match(
			success => success.Value.ToString(),
			failure => failure.Error);
		
		Assert.Equal("4", s);
	}
}
