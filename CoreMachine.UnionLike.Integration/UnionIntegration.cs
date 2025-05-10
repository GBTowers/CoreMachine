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

public static class UnionT2Ex
{
	public static async Task<TOut> Match<TOut, T, T1, T2>(
		this Task<Union<T, T1, T2>> task,
		Func<T1, TOut> f1,
		Func<T2, TOut> f2)
		where T : Union<T, T1, T2>
		where T1 : T
		where T2 : T
		=> (await task.ConfigureAwait(false)).Match(f1, f2);
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
