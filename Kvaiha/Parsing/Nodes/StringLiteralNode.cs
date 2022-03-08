namespace Kvaiha.Parsing.Nodes;

public class StringLiteralNode : INode
{
    public string Value { get; }

    public StringLiteralNode(string value)
        => Value = value;

    public string PrettyPrint()
        => $"StringNode::{Value}";
}