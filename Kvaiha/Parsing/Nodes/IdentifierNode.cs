using Pidgin;
using Spectre.Console;

namespace Kvaiha.Nodes;

public class IdentifierNode : INode
{
    public string Name { get; }

    public SourcePos Location { get; }

    public IdentifierNode(string name, SourcePos location)
    {
        Name = name;
        Location = location;
    }

    public void Render(IHasTreeNodes tree)
        => tree.AddNode(Markup.Escape($"Identifier::{Name}::[{Location.Line}:{Location.Col}]"));
}