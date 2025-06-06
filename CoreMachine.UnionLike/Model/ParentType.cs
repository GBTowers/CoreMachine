namespace CoreMachine.UnionLike.Model;

public record ParentType(
	string Keyword,
	string Name,
	string Constraints,
	ParentType? Child
);
