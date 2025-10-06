namespace cslox;

public class Lox
{
    private static bool hadError = false;

    static void Main(string[] args)
    {
        // if (args.Length > 1)
        // {
        //     Console.WriteLine("Usage: cslox [script]");
        //     Environment.Exit(64);
        // }
        // else if (args.Length == 1)
        // {
        //     RunFile(args[0]);
        // }
        // else
        // {
        //     RunPrompt();
        // }
        Expr expression = new Expr.Binary(
            new Expr.Unary(new Token(TokenType.MINUS, "-", null, 1), new Expr.Literal(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new Expr.Grouping(new Expr.Literal("str"))
        );

        Console.WriteLine(new RpnPrinter().Print(expression));
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
        byte[] bytes = File.ReadAllBytes(path);
        Run(System.Text.Encoding.Default.GetString(bytes));
        if (hadError)
            Environment.Exit(65);
    }

    public static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
        hadError = true;
    }
}
