// ReSharper disable InconsistentNaming    

using CoreMachine.UnionLike.Diagnosis.UResult;

namespace CoreMachine.UnionLike.Diagnosis
{
	internal abstract record Result<T, E>
		where T : IEquatable<T>
		where E : IEquatable<E>
	{
		public static implicit operator Result<T, E>(T value) => new Ok(value);

		public static implicit operator Result<T, E>(E error) => new Err(error);

		private record Ok(T Value) : Result<T, E>, Ok<T>;

		private record Err(E Error) : Result<T, E>, Err<E>;
	}

	namespace UResult
	{
		public interface Ok<T>
		{
			public T Value { get; }
			public void Deconstruct(out T value);
		}

		public interface Err<E>
		{
			public E Error { get; }
			public void Deconstruct(out E error);
		}
	}
}
