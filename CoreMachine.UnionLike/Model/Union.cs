using CoreMachine.UnionLike.Extensions;

namespace CoreMachine.UnionLike.Model;

public record Union(int Arity)
{
	public ArityMember[] ArityMembers { get; } = Enumerable.Range(start: 1, Arity)
	.Select(i => new ArityMember(i))
	.ToArray();

	public string TypeParameters => $"T, {ArityMembers.JoinSelect(m => m.Name)}";
	public string Declaration => nameof(Union) + '<' + TypeParameters + '>';
	public string MatchTypeParams => "TOut, " + TypeParameters;

	public string WhereClauses => $"where T : {Declaration} " + ArityMembers.JoinSelect(m => $"where {m.Name} : T", " ");
}

public record ArityMember(int Arity)
{
	public string Name => "T" + Arity;
	public string VariableName => "t" + Arity;
	public string FuncName => "f" + Arity;
	public string FuncDeclaration => $"Func<{Name}, TOut>";
	public string AsyncFuncDeclaration => $"Func<{Name}, Task<TOut>>";
	public string AsyncValueFuncDeclaration => $"Func<{Name}, ValueTask<TOut>>";
	public string ActName => "a" + Arity;
	public string ActDeclaration => $"Action<{Name}>";
	public string AsyncActDeclaration => $"Func<{Name}, Task>";
	public string AsyncValueActDeclaration => $"Func<{Name}, ValueTask>";
}
