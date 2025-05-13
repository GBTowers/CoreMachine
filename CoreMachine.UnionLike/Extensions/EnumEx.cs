namespace CoreMachine.UnionLike.Extensions;

public static class EnumE
{
	public static T? ToEnum<T>(this string? str)
		where T : struct, Enum
		=> Enum.TryParse(str, out T output) ? output : default;
}
