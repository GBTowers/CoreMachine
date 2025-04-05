namespace CoreMachine.UnionLike.Data;

public class NotNullWhenAttribute : Attribute
{ 
    public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;
    public bool ReturnValue { get; }
}