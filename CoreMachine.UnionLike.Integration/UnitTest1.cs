using CoreMachine.UnionLike.Attributes;

namespace CoreMachine.UnionLike.Integration;

[Union]
public partial record Result<T, TE>
{
	partial record Ok(T Value)
	{
		public static implicit operator T(Ok ok)
		{
			return ok.Value;
		}
	}

	partial record Err(TE Error)
	{
		public static implicit operator TE(Err err)
		{
			return err.Error;
		}
	}
}

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		var result = new Result<int, string>.Ok(4);

		string r = result.Match(ok => ok.Value.ToString(), err => err.Error);

		if (result.IsOk(out var ok))
		{
			int num = ok;
		}

		if (result.IsErr(out var err))
		{
			string str = err;
		}
	}
}