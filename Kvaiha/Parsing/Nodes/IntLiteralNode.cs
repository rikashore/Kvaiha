namespace Kvaiha.Parsing.Nodes;

public class IntLiteralNode : INode
{
    public int Value { get; }

    public IntLiteralNode(int value)
        => Value = value;

    public string PrettyPrint()
        => $"IntNode::{Value}";
}