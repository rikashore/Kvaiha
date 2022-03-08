namespace Kvaiha.Parsing.Nodes;

public class AssignmentNode : INode
{
    public IdentifierNode Identifier { get; }

    public INode Expression { get; }

    public AssignmentNode(IdentifierNode identifier, INode expression)
    {
        Identifier = identifier;
        Expression = expression;
    }

    public string PrettyPrint()
        => $"Assigment::{Identifier.Name}";
}