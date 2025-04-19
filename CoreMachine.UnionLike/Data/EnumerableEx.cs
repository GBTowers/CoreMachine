namespace CoreMachine.UnionLike.Data;

public static class EnumerableEx
{
    public static string JoinSelect<T>(this IEnumerable<T> source, Func<T, string> selector,
        string separator = ", ")
        => source.Select(selector).JoinString(separator);

    public static string JoinString<T>(this IEnumerable<T> source, string separator = ", ")
        => string.Join(separator, source);
}