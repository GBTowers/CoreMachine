# CoreMachine.Result
Just another Result library.

### Examples
```csharp
Result<int, string> result = MethodReturningResult();

string matched = result.Match(
    num => num.ToString(),
    err => $"Error occurred: {err}");
    
// default case is imposible, but the compiler complains if you don't add it
string switched = result switch
{
    IOk<int>(var num) => num.ToString(),
    IErr<string>(var err) => $"Error occurred: {err}");
    _ => throw InvalidOperationException();
}
```