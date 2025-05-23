﻿using CoreMachine.UnionLike.Attributes;

namespace CoreMachine.UnionLike.Integration;

[Union]
public abstract partial record Result<T, TE>
{
	partial record Ok(T Value);
	partial record Err(TE Error);
}

public class UnionIntegration
{
	[Fact]
	public void MatchMethodWorksOnDerivedClasses()
	{
		Result<int, string> result = 4;

		string s = result.Match(success => success.Value.ToString(), failure => failure.Error);

		Assert.Equal("4", s);
	}
}
