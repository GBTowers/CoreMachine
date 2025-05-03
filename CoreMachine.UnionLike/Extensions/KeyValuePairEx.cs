using CoreMachine.UnionLike.Model;

namespace CoreMachine.UnionLike.Extensions;

public static class KeyValuePairEx
{
	public static void Deconstruct(
		this KeyValuePair<string, UnionMemberToGenerate> memberGroup,
		out string tuple,
		out UnionMemberToGenerate member)
	{
		tuple = memberGroup.Key;
		member = memberGroup.Value;
	}
}
