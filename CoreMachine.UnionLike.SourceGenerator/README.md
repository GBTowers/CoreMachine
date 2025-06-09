# UnionLike

C# Source Generator for Discriminated Union-like polymorphic relationships

[Source Code](https://github.com/GBTowers/CoreMachine)

---

## Basic Usage

`install-package UnionLike`

```csharp
using CoreMachine.UnionLike.Attributes;

namespace Test;

[Union]
public abstract partial record Notification
{
	partial record StatusNotification(string Message, int Code);
	partial record PaymentNotification(decimal Amount, string Account);
}
```
