using Pidgin;
using Spectre.Console;

namespace Kvaiha.Nodes;

public class IntegerLiteralNode : INode
{
    public int Value { get; }

    public SourcePos Location { get; }

    public IntegerLiteralNode(int value, SourcePos location)
    {
        Value = value;
        Location = location;
    }

    public override string ToString()
        => $"IntValue::{Value}";

    public void Render(IHasTreeNodes tree)
        => tree.AddNode(Markup.Escape($"IntegerLiteral::{Value}::[{Location.Line}:{Location.Col}]"));
}