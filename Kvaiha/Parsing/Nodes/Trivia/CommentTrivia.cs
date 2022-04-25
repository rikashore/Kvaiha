namespace Kvaiha.Nodes;

public class CommentTrivia : ITrivia
{
    public string Value { get; }

    public CommentTrivia(string value)
        => Value = value;
}