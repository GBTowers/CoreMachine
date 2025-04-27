namespace CoreMachine.UnionLike.Data;

public static class EnumerableEx
{
	public static string JoinSelect<T>(this IEnumerable<T> source, Func<T, string> selector,
		string separator = ", ")
	{
		return source.Select(selector).JoinString(separator);
	}

	public static string JoinString<T>(this IEnumerable<T> source, string separator = ", ")
	{
		return string.Join(separator, source);
	}
}