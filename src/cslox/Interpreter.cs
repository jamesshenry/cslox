using System.Reflection.Metadata.Ecma335;

namespace cslox
{
    public class Interpreter : Expr.IVisitor<object>
    {
        public object Visit(Expr.Binary expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object Visit(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object Visit(Expr.Unary expr)
        {
            var right = Evaluate(expr.Right);

            return expr.Operator.Type switch
            {
                TokenType.MINUS => -(double)right,
                TokenType.BANG => !IsTruthy(right),
                _ => throw new InvalidOperationException(),
            };
        }

        private bool IsTruthy(object obj)
        {
            return obj switch
            {
                null => false,
                bool _ => (bool)obj,
                _ => true,
            };
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }
    }
}
