﻿namespace CoreMachine.UnionLike.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class UnionAttribute : Attribute
{
	public bool GenerateAsyncExtensions { get; set; }
}
