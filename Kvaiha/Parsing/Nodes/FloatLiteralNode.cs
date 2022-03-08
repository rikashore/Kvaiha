namespace Kvaiha.Parsing.Nodes;

public class FloatLiteralNode : INode
{
    public float Value { get; }

    public FloatLiteralNode(float value)
        => Value = value;

    public string PrettyPrint()
        => $"FloatNode::{Value}";
}