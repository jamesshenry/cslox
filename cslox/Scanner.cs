using static cslox.TokenType;

namespace cslox;

internal class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = [];
    private int start;
    private int current;
    private int line = 1;

    public Scanner(string source)
    {
        this.source = source;
    }

    public bool IsAtEnd => current >= source.Length;

    internal List<Token> ScanTokens()
    {

        while (!IsAtEnd)
        {
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(EOF, "", null, line));
        return tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(LEFT_PAREN); break;
            case ')': AddToken(RIGHT_PAREN); break;
            case '{': AddToken(LEFT_BRACE); break;
            case '}': AddToken(RIGHT_BRACE); break;
            case ',': AddToken(COMMA); break;
            case '.': AddToken(DOT); break;
            case '-': AddToken(MINUS); break;
            case '+': AddToken(PLUS); break;
            case ';': AddToken(SEMICOLON); break;
            case '*': AddToken(STAR); break;
        }
    }

    private char Advance() => source[current++];

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object literal)
    {
        string text = source.Substring(start, current);
        tokens.Add(new Token(type, text, literal, line));
    }
}