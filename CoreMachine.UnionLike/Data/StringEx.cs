namespace CoreMachine.UnionLike.Data;

public static class StringEx
{
	public static string? FirstCharToLower(this string? str)
		=> str is { Length: > 0 } ? char.ToLower(str[0]) + new string(str.Skip(1).ToArray()) : null;
}
