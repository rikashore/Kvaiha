using Pidgin;
using Spectre.Console;

namespace Kvaiha.Nodes;

public class BinaryOperatorNode : INode
{
    public INode Left { get; }
    public INode Right { get; }

    public BinaryOperatorType Type { get; }

    public SourcePos Location { get; }

    public BinaryOperatorNode(INode left, INode right, BinaryOperatorType type, SourcePos location)
    {
        Left = left;
        Right = right;
        Type = type;
        Location = location;
    }

    public void Render(IHasTreeNodes tree)
    {
        var subNode = tree.AddNode(Markup.Escape($"BinaryOp::{Type}::[{Location.Line}:{Location.Col}]"));

        Left.Render(subNode);
        Right.Render(subNode);
    }
}

public enum BinaryOperatorType
{
    Addition,

    Subtraction,

    Multiplication,

    Division,

    Exponent
}