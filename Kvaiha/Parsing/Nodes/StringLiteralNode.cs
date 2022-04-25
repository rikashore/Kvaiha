using Pidgin;
using Spectre.Console;

namespace Kvaiha.Nodes;

public class StringLiteralNode : INode
{
    public string Value { get; }

    public SourcePos Location { get; }

    public StringLiteralNode(string value, SourcePos location)
    {
        Value = value;
        Location = location;
    }

    public void Render(IHasTreeNodes tree)
        => tree.AddNode(Markup.Escape($"StringLiteral::{Value}::[{Location.Line}:{Location.Col}]"));
}