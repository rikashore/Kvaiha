using System.Linq;
using Kvaiha.Parsing;
using Kvaiha.Parsing.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Kvaiha.Tests;

public class ParserTests
{
    private readonly ITestOutputHelper _outputHelper;

    public ParserTests(ITestOutputHelper outputHelper)
        => _outputHelper = outputHelper;

    [Theory]
    [InlineData("let _foo = 1")]
    [InlineData("let bar = 1 + 1")]
    [InlineData("let baz = (2 * 2) + (11 - 1) / (22)")]
    [InlineData("let baz = 2 * 2 + (11 - 1) / 22")]
    public void ParserWillParseAssignment(string inputData)
    {
        var res = KvaihaParser.Parse(inputData);

        Assert.True(res.Success);

        _outputHelper.WriteLine((res.Value.First() as AssignmentNode)?.Expression.PrettyPrint());
    }
}