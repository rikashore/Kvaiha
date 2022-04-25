using Kvaiha.Nodes;
using Spectre.Console;

namespace Kvaiha.Debug.Cli;

public class AstRenderer
{
    private readonly IEnumerable<INode> _nodes;

    private readonly Tree _tree;

    public AstRenderer(IEnumerable<INode> nodes)
    {
        _nodes = nodes;
        _tree = new Tree("Parsed AST")
            .Guide(TreeGuide.Ascii)
            .Style(new Style(foreground: Color.SteelBlue));
    }

    public Tree Render()
    {
        foreach (var node in _nodes)
            node.Render(_tree);

        return _tree;
    }
}