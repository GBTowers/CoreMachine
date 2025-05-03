using CoreMachine.UnionLike.Data;

namespace CoreMachine.UnionLike.Extensions;

public static class StringEx
{
	[return: NotNullIfNotNull(nameof(str))]
	public static string? FirstCharToLower(this string? str)
		=> str is { Length: > 0 } ? char.ToLower(str[0]) + new string(str.Skip(1).ToArray()) : null;

	public static bool IsNullOrWhiteSpace(this string? str)
		=> string.IsNullOrWhiteSpace(str);
}
