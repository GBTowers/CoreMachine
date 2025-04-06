// ReSharper disable InconsistentNaming    

namespace CoreMachine.UnionLike.Diagnosis
{
    internal abstract record Result<T, E>
        where T : IEquatable<T>
        where E : IEquatable<E>
    {
        private record Ok(T Value) : Result<T, E>, UResult.Ok<T>;

        private record Err(E Error) : Result<T, E>, UResult.Err<E>;

        public static implicit operator Result<T, E>(T value) => new Ok(value);
        public static implicit operator Result<T, E>(E error) => new Err(error);
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