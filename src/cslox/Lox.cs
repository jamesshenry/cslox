using System.Security;

namespace cslox;

public class Lox
{
    private static bool hadError = false;

    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: cslox [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    public static void RunPrompt()
    {
        using StreamReader reader = new(Console.OpenStandardInput());

        for (; ; )
        {
            Console.Out.Write("> ");
            string? line = reader.ReadLine();
            if (line == null)
                break;
            Run(line);
            hadError = false;
        }
    }

    public static void RunFile(string path)
    {
        var bytes = File.ReadAllBytes(path);
        Run(System.Text.Encoding.Default.GetString(bytes));
        if (hadError)
            Environment.Exit(65);
    }

    public static void Run(string source)
    {
        var scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        var parser = new Parser(tokens);
        var expression = parser.Parse();

        if (hadError)
            return;

        Console.WriteLine(new AstPrinter().Print(expression));
    }

    public static void Error(Token token, string message)
    {
        Report(token.Line, token.Lexeme, message);
    }

    internal static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
        hadError = true;
    }
}
