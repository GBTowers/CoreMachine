using static System.ArgumentOutOfRangeException;

namespace CoreMachine.UnionLike.Analyzers.Sample;

public class Spaceship
{
	public static void SetSpeed(long speed) => ThrowIfGreaterThan(value: speed, other: 299_792_458);
}
