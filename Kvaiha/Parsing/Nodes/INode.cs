using Pidgin;
using Spectre.Console;

namespace Kvaiha.Nodes;

public interface INode
{
    SourcePos Location { get; }

    void Render(IHasTreeNodes tree);
}