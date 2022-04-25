using Kvaiha.Nodes;
using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

using BinaryOperatorFunc = System.Func<Kvaiha.Nodes.INode, Kvaiha.Nodes.INode, Kvaiha.Nodes.INode>;
using BinaryOperatorType = Kvaiha.Nodes.BinaryOperatorType;

namespace Kvaiha;

public static class KvaihaParser
{
    private static Parser<char, INode> Upcast<T>(this Parser<char, T> parser)
        where T : INode
        => parser.Cast<INode>();

    private static Parser<char, T> SkipWs<T>(this Parser<char, T> token)
        => Try(token).Before(SkipWhitespaces);

    private static Parser<char, U> WithLocation<T, U>(this Parser<char, T> parser, Func<SourcePos, T, U> selector)
        => CurrentPos.Then(parser, selector);

    private static Parser<char, string> SkipWs(string token)
        => SkipWs(String(token));

    private static Parser<char, T> Parenthesised<T>(Parser<char, T> parser)
        => parser.Between(SkipWs("("), SkipWs(")"));

    private static Parser<char, BinaryOperatorFunc> Binary(Parser<char, BinaryOperatorType> op)
        => CurrentPos
            .Then(op, (pos, type) => (type, pos))
            .Select<BinaryOperatorFunc>(
                typeAndLocation => (left, right) => new BinaryOperatorNode(
                    left,
                    right,
                    typeAndLocation.type, typeAndLocation.pos
                )
            );

    private static readonly Parser<char, BinaryOperatorFunc> Add =
        Binary(SkipWs("+").ThenReturn(BinaryOperatorType.Addition));

    private static readonly Parser<char, BinaryOperatorFunc> Sub =
        Binary(SkipWs("-").ThenReturn(BinaryOperatorType.Subtraction));

    private static readonly Parser<char, BinaryOperatorFunc> Mul =
        Binary(SkipWs("*").ThenReturn(BinaryOperatorType.Multiplication));

    private static readonly Parser<char, BinaryOperatorFunc> Div =
        Binary(SkipWs("/").ThenReturn(BinaryOperatorType.Division));

    private static readonly Parser<char, BinaryOperatorFunc> Exp =
        Binary(SkipWs("^").ThenReturn(BinaryOperatorType.Exponent));

    private static readonly Parser<char, IdentifierNode> Identifier =
        Map(
            (location, h, t) => new IdentifierNode(h + t, location),
            CurrentPos,
            Letter.Or(Char('_')),
            LetterOrDigit.ManyString()
        ).SkipWs();

    private static readonly Parser<char, StringLiteralNode> StringLiteral =
        Token(c => c != '"')
            .ManyString()
            .Between(Char('"'))
            .WithLocation((location, s) => new StringLiteralNode(s, location))
            .SkipWs()
            .Labelled("string literal");

    private static readonly Parser<char, IntegerLiteralNode> IntegerLiteral =
        OneOf(
            DecimalNum,
            OctalNum,
            HexNum
        ).WithLocation((location, i) => new IntegerLiteralNode(i, location))
            .SkipWs();

    private static readonly Parser<char, INode> Expression =
        ExpressionParser.Build(
            expr => OneOf(
                Identifier.Upcast(),
                StringLiteral.Upcast(),
                IntegerLiteral.Upcast(),
                Parenthesised(expr.SkipWs())
            ),
            new[]
            {
                Operator.InfixL(Add)
                    .And(Operator.InfixL(Sub))
                    .And(Operator.InfixL(Mul))
                    .And(Operator.InfixL(Div))
                    .And(Operator.InfixL(Exp))
            }
        ).Labelled("expression");

    public static Result<char, IEnumerable<INode>> Parse(string text)
        => Expression.SkipWs().Many().Parse(text);
}