namespace Kvaiha.Parsing.Nodes;

public class BinaryOperatorToken : INode
{
    public BinaryOperatorType Type { get; }

    public INode Left { get; }
    public INode Right { get; }

    public BinaryOperatorToken(BinaryOperatorType type, INode left, INode right)
    {
        Type = type;
        Left = left;
        Right = right;
    }

    public string PrettyPrint()
        => $"BinOp::{Type}";
}

public enum BinaryOperatorType
{
    Add,

    Mul,

    Sub,

    Div
}