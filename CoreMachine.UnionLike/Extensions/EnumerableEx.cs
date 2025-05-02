using System.Diagnostics.Contracts;

namespace CoreMachine.UnionLike.Extensions;

[Serializable, ContractClass(typeof(EnumerableEx))]
public static class EnumerableEx
{
	public static string JoinSelect<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ", ")
		=> source.Select(selector).JoinString(separator);

	public static string JoinString<T>(this IEnumerable<T> source, string separator = ", ")
		=> string.Join(separator, source);

	/// <inheritdoc cref="List{T}.IndexOf(T)"/>
	public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> selector)
	{
		var index = 0;
		foreach (var element in source)
		{
			if (selector(element)) return index;
			
			index++;
		}
		
		return -1;
	}
}
