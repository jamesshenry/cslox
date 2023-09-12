using System.Runtime.InteropServices;

namespace cslox;

class Lox
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
            runFile(args[0]);
        }
        else
        {
            runPrompt();
        }
    }

    private static void runPrompt()
    {
        StreamReader reader = new(Console.OpenStandardInput());

        for (; ; )
        {
            Console.Out.Write("> ");
            string? line = reader.ReadLine();
            if (line == null) break;
            run(line);
            hadError = false;
        }
    }

    private static void runFile(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        run(System.Text.Encoding.Default.GetString(bytes));
        if (hadError) Environment.Exit(65);
    }



    private static void run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    private static void error(int line, string message)
    {
        report(line, "", message);
    }

    private static void report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
        hadError = true;
    }
}

class Token
{
    readonly TokenType type;
    readonly string lexeme;
    readonly object literal;
    readonly int line;
    public Token(TokenType type, string lexeme, object literal, int line)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }

    public override string ToString() =>
 $"{type} {lexeme} {literal}";

}
internal class Scanner
{
    private string source;

    public Scanner(string source)
    {
        this.source = source;
    }

    internal List<Token> ScanTokens()
    {
        throw new NotImplementedException();
    }
}