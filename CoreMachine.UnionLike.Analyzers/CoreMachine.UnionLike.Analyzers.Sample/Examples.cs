// Resharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System;
using CoreMachine.UnionLike.Attributes;

namespace CoreMachine.UnionLike.Analyzers.Sample;

public partial class Examples
{
	[Union]
	public partial record Result<T, TE>
	{
    partial record Ok;
		partial record Err;	
	}
	
	[Union]
	public partial record Hello
	{
    partial record Hola;
		partial record Hai;
	}
}
