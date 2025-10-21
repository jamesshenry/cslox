using static cslox.TokenType;

namespace cslox;

public class Parser
{
    List<Token> _tokens = [];
    int _current = 0;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Expr Parse()
    {
        try
        {
            return Expression();
        }
        catch (ParseException ex)
        {
            return null!;
        }
    }

    // expression -> comma;
    private Expr Expression()
    {
        return Comma();
    }

    // comma      -> equality ( "," equality ) * ;
    private Expr Comma()
    {
        Expr expr = Equality();

        while (Match(COMMA))
        {
            Token opr = Previous();
            Expr right = Equality();
            expr = new Expr.Binary(expr, opr, right);
        }

        return expr;
    }

    // equality -> comparison ( ( "!=" | "==" ) comparison )* ;
    private Expr Equality()
    {
        Expr expr = Comparison();

        while (Match(BANG_EQUAL, EQUAL_EQUAL))
        {
            Token opr = Previous();
            Expr right = Comparison();
            expr = new Expr.Binary(expr, opr, right);
        }

        return expr;
    }

    // comparison -> term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
    private Expr Comparison()
    {
        Expr expr = Term();

        while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
        {
            Token opr = Previous();
            Expr right = Term();
            expr = new Expr.Binary(expr, opr, right);
        }

        return expr;
    }

    // term       -> factor ( ( "-" | "+" ) factor )* ;
    private Expr Term()
    {
        Expr expr = Factor();

        while (Match(MINUS, PLUS))
        {
            Token opr = Previous();
            Expr right = Factor();
            expr = new Expr.Binary(expr, opr, right);
        }

        return expr;
    }

    // factor     -> unary ( ( "/" | "*" ) unary )* ;
    private Expr Factor()
    {
        Expr expr = Unary();

        while (Match(SLASH, STAR))
        {
            Token opr = Previous();
            Expr right = Unary();
            expr = new Expr.Binary(expr, opr, right);
        }

        return expr;
    }

    private Expr Unary()
    {
        if (Match(BANG, MINUS))
        {
            Token opr = Previous();
            Expr right = Unary();
            return new Expr.Unary(opr, right);
        }

        return Primary();
    }

    private Expr Primary()
    {
        if (Match(FALSE))
            return new Expr.Literal(false);
        if (Match(TRUE))
            return new Expr.Literal(true);
        if (Match(NIL))
            return new Expr.Literal(null);
        if (Match(NUMBER, STRING))
        {
            return new Expr.Literal(Previous().Literal);
        }
        if (Match(LEFT_PAREN))
        {
            Expr expr = Expression();
            Consume(RIGHT_PAREN, "Expect ')' after expression.");
            return new Expr.Grouping(expr);
        }
        if (Match(BANG_EQUAL, EQUAL_EQUAL))
        {
            Error(Previous(), "Expected left hand operand");
            _ = Equality();
            return null!;
        }

        if (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
        {
            Error(Previous(), "Expected left hand operand");
            _ = Comparison();
            return null!;
        }

        if (Match(PLUS))
        {
            Error(Previous(), "Expected left hand operand");
            _ = Term();
            return null!;
        }
        if (Match(SLASH, STAR))
        {
            Error(Previous(), "Expected left hand operand");
            _ = Factor();
            return null!;
        }

        throw Error(Peek(), "Expected expression.");
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
            return Advance();
        throw Error(Peek(), message);
    }

    private Exception Error(Token token, string message)
    {
        Lox.Error(token, message);
        return new ParseException();
    }

    private bool Match(params TokenType[] tokens)
    {
        foreach (var tt in tokens)
        {
            if (Check(tt))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private bool Check(TokenType tt)
    {
        if (IsAtEnd())
            return false;
        return Peek().Type == tt;
    }

    private Token Advance()
    {
        if (!IsAtEnd())
            _current++;
        return Previous();
    }

    private void Synchronize()
    {
        Advance();

        while (!IsAtEnd())
        {
            if (Previous().Type == SEMICOLON)
                return;

            switch (Peek().Type)
            {
                case CLASS:
                case FUN:
                case VAR:
                case FOR:
                case IF:
                case WHILE:
                case PRINT:
                case RETURN:
                    return;
            }

            Advance();
        }
    }

    private bool IsAtEnd() => Peek().Type == EOF;

    private Token Peek() => _tokens[_current];

    private Token Previous() => _tokens.ElementAt(_current - 1);

    private class ParseException : Exception { }
}
