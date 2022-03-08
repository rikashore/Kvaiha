namespace Kvaiha.Parsing.Nodes;

public class IdentifierNode : INode
{
    public string Name { get; }

    public IdentifierNode(string name)
        => Name = name;

    public string PrettyPrint()
        => $"Identifier::{Name}";
}