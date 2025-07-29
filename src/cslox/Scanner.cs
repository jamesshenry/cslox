using static cslox.TokenType;

namespace cslox;

public class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = [];
    private int start;
    private int current;
    private int line = 1;

    private static readonly Dictionary<string, TokenType> keywords = new()
    {
        ["and"] = AND,
        ["class"] = CLASS,
        ["else"] = ELSE,
        ["false"] = FALSE,
        ["for"] = FOR,
        ["fun"] = FUN,
        ["if"] = IF,
        ["nil"] = NIL,
        ["or"] = OR,
        ["print"] = PRINT,
        ["return"] = RETURN,
        ["super"] = SUPER,
        ["this"] = THIS,
        ["true"] = TRUE,
        ["var"] = VAR,
        ["while"] = WHILE,
    };

    public Scanner(string source)
    {
        this.source = source;
    }

    internal List<Token> ScanTokens()
    {
        while (!IsAtEnd())
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
            case '(':
                AddToken(LEFT_PAREN);
                break;
            case ')':
                AddToken(RIGHT_PAREN);
                break;
            case '{':
                AddToken(LEFT_BRACE);
                break;
            case '}':
                AddToken(RIGHT_BRACE);
                break;
            case ',':
                AddToken(COMMA);
                break;
            case '.':
                AddToken(DOT);
                break;
            case '-':
                AddToken(MINUS);
                break;
            case '+':
                AddToken(PLUS);
                break;
            case ';':
                AddToken(SEMICOLON);
                break;
            case '*':
                AddToken(STAR);
                break;
            case '!':
                AddToken(Match('=') ? BANG_EQUAL : BANG);
                break;
            case '=':
                AddToken(Match('=') ? EQUAL_EQUAL : EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? LESS_EQUAL : LESS);
                break;
            case '>':
                AddToken(Match('=') ? GREATER_EQUAL : GREATER);
                break;
            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd())
                        Advance();
                }
                else
                {
                    AddToken(SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;
            case '\n':
                line++;
                break;
            case '"':
                ParseString();
                break;

            default:
                if (char.IsDigit(c))
                    ParseNumber();
                else if (IsAlpha(c))
                    ParseIdentifier();
                else
                    Lox.Error(line, "Unexpected character.");
                break;
        }
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object? literal)
    {
        string text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal, line));
    }

    private void ParseString()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n')
                line++;
            Advance();
        }
        if (IsAtEnd())
        {
            Lox.Error(line, "Unterminated string.");
            return;
        }

        Advance();

        string value = source[(start + 1)..(current - start)];
        AddToken(STRING, value);
    }

    private void ParseNumber()
    {
        while (char.IsDigit(Peek()))
            Advance();

        if (Peek() == '.' && char.IsDigit(PeekNext()))
        {
            Advance();

            while (char.IsDigit(Peek()))
                Advance();
        }

        AddToken(NUMBER, double.Parse(source[start..current]));
    }

    private void ParseIdentifier()
    {
        while (IsAlphaNumeric(Peek()))
            Advance();

        string text = source.Substring(start, current - start);

        if (!keywords.TryGetValue(text, out TokenType type))
            type = IDENTIFIER;

        AddToken(type);
    }

    private char Peek() => IsAtEnd() ? '\0' : source[current];

    private char PeekNext() => current + 1 >= source.Length ? '\0' : source[current + 1];

    private bool IsAtEnd() => current >= source.Length;

    private char Advance() => source[current++];

    private bool Match(char expected)
    {
        if (IsAtEnd())
            return false;
        if (source[current] != expected)
            return false;

        current++;
        return true;
    }

    private static bool IsAlpha(char c) => char.IsAsciiLetter(c) || c == '_';

    private static bool IsAlphaNumeric(char c) => IsAlpha(c) || char.IsAsciiDigit(c);
}
