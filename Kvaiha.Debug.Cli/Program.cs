using Kvaiha;
using Kvaiha.Debug.Cli;
using Spectre.Console;

var parseRes = KvaihaParser.Parse("11 + 12 - 13 * _foo / \"shiftamus\" ^ _expBy");

if (!parseRes.Success)
{
    Console.WriteLine(parseRes.Error.RenderErrorMessage());
    return;
}

var renderer = new AstRenderer(parseRes.Value);

AnsiConsole.Write(renderer.Render());