namespace cslox;

class Lox
{
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
            Console.Out.WriteLine("> ");
            string? line = reader.ReadLine();
            if (line == null) break;
            run(line);
        }
    }

    private static void runFile(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        run(System.Text.Encoding.Default.GetString(bytes));
    }



    private static void run(string line)
    {

    }
}
