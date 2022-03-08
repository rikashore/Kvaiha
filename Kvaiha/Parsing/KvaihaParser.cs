using Kvaiha.Parsing.Nodes;
using Pidgin;
using Pidgin.Expression;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using BinaryOperatorType = Kvaiha.Parsing.Nodes.BinaryOperatorType;
using BinaryOpParserFunc =
    System.Func<Kvaiha.Parsing.Nodes.INode, Kvaiha.Parsing.Nodes.INode, Kvaiha.Parsing.Nodes.INode>;

namespace Kvaiha.Parsing;

public static class KvaihaParser
{
    private static Parser<char, INode> Upcast<T>(this Parser<char, T> parser)
        where T : INode
        => parser.Cast<INode>();

    private static Parser<char, T> Tok<T>(Parser<char, T> token)
        => Try(token).Before(SkipWhitespaces);

    private static Parser<char, string> Tok(string token)
        => Tok(String(token));

    private static Parser<char, T> Parenthesised<T>(Parser<char, T> parser)
        => parser.Between(Tok("("), Tok(")"));

    private static Parser<char, BinaryOpParserFunc> Binary(Parser<char, BinaryOperatorType> op)
        => op.Select<BinaryOpParserFunc>(type => (l, r) => new BinaryOperatorToken(type, l, r));

    private static readonly Parser<char, BinaryOpParserFunc> Add =
        Binary(Tok("+").ThenReturn(BinaryOperatorType.Add));

    private static readonly Parser<char, BinaryOpParserFunc> Mul =
        Binary(Tok("*").ThenReturn(BinaryOperatorType.Mul));

    private static readonly Parser<char, BinaryOpParserFunc> Sub =
        Binary(Tok("-").ThenReturn(BinaryOperatorType.Sub));

    private static readonly Parser<char, BinaryOpParserFunc> Div =
        Binary(Tok("/").ThenReturn(BinaryOperatorType.Div));

    private static Parser<char, StringLiteralNode> StringLiteral =
        Tok(Any.ManyString().Between(Char('"')))
            .Select(s => new StringLiteralNode(s));

    private static Parser<char, IntLiteralNode> IntLiteral =
        Num.Select(i => new IntLiteralNode(i));

    // TODO Figure out ruby precision and match or change if necessary
    // private static Parser<char, FloatLiteralNode> FloatLiteral =
    //     Real

    private static readonly Parser<char, IdentifierNode> Identifier =
        Tok(Char('_').Or(Letter)
            .Then(LetterOrDigit.ManyString(), (h, t) => h + t)
            .Select(s => new IdentifierNode(s)));

    private static readonly Parser<char, INode> Expr = ExpressionParser.Build<char, INode>(
        expr => (
            OneOf(
                Tok(Identifier).Upcast(),
                Tok(IntLiteral).Upcast(),
                Tok(Parenthesised(expr))
            ),
            new[]
            {
                Operator.InfixL(Mul)
                    .And(Operator.InfixL(Div)),
                Operator.InfixL(Add)
                    .And(Operator.InfixL(Sub))
            })
    );

    private static readonly Parser<char, AssignmentNode> Assignment =
        Map(
            (_, ident, _, tok) => new AssignmentNode(ident, tok),
            Tok("let"),
            Tok(Identifier),
            Tok("="),
            Expr
        );

    private static readonly Parser<char, INode> Token =
        OneOf(
            Assignment.Upcast()
        );

    public static Result<char, IEnumerable<INode>> Parse(string input)
        => Token.Until(End).Parse(input);
}