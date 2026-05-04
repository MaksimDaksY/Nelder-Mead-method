using System;
using System.Collections.Generic;
using System.Linq;

namespace NelderMeadParser
{
    public class EvaluatingException : Exception
    {
        public EvaluatingException(string msg) : base(msg) { }
    }

    public class MathException : EvaluatingException
    {
        public double Left, Right;
        public string Operator;
        public MathException(string err, double left, double right, string op) :
            base($"Cannot execute {left}{op}{right} ({err})")
        {
            Left = left;
            Right = right;
            Operator = op;
        }
    }

    public class ParsingException : Exception
    {
        public string Expression;
        public ParsingException(string expr) : base($"Cannot parse '{expr}'")
        {
            Expression = expr;
        }
        public ParsingException(string expr, string msg) : base(msg)
        {
            Expression = expr;
        }
    }

    public class WrongSymbolException : ParsingException
    {
        public int Position;
        public char FailedChar;
        public WrongSymbolException(string expr, int pos) :
            base(expr, $"Cannot parse '{expr}': unacceptable character '{expr[pos]}' found at position {pos}")
        {
            FailedChar = expr[pos];
            Position = pos;
        }
        public WrongSymbolException(string expr, char failed) :
            base(expr, $"Cannot parse '{expr}': unacceptable character '{failed}' found in expression")
        {
            Position = expr.IndexOf(failed);
            FailedChar = failed;
        }
    }

    public class SyntaxException : ParsingException
    {
        public int Position;
        public string SyntaxError;
        public SyntaxException(string expr, string error, int pos) :
            base(expr, $"Cannot parse '{expr}': {error} (at position {pos})")
        {
            SyntaxError = error;
            Position = pos;
        }
        public SyntaxException(string expr, string error) :
            base(expr, $"Cannot parse '{expr}': {error}")
        {
            SyntaxError = error;
            Position = -1;
        }
    }

    public class ParserToken
    {
        public readonly string Token;
        public readonly int Order;
        public readonly int Arguments;
        public virtual double Eval(params double[] args) => throw new NotImplementedException();
        public override string ToString() => Token;
        public static implicit operator string(ParserToken t) => t.Token;

        public ParserToken(string token, int order = -1, int args = 0)
        {
            Token = token.ToUpper();
            Order = order;
            Arguments = args;
        }
    }

    public class Variable : ParserToken
    {
        public double Value;
        public override double Eval(params double[] args) =>
            double.IsNaN(Value) ? throw new EvaluatingException($"Cannot evaluate expression, '{Token}' is not defined") : Value;
        public static implicit operator double(Variable v) => v.Value;

        public Variable(string name, double value = double.NaN) : base(name, -1, 0)
        {
            Value = value;
        }
    }

    public class Constant : Variable
    {
        public override double Eval(params double[] args) => Value;
        public Constant(double value) : base("_", value)
        {
            Value = value;
        }
        public override string ToString() => $"'{Value}'";
    }

    public class BinaryOperator : ParserToken
    {
        private Func<double, double, double> Func;
        public override double Eval(params double[] args) => Func(args[0], args[1]);

        public BinaryOperator(string name, Func<double, double, double> func, int order) : base(name, order, 2)
        {
            Func = func;
        }

        public static BinaryOperator SelectOperator(char op)
        {
            switch (op)
            {
                case '+': return AddOper();
                case '-': return SubOper();
                case '*': return MulOper();
                case '/': return DivOper();
                case '^': return PowOper();
                default: throw new ArgumentException("Unknown operator");
            }
        }

        public static BinaryOperator AddOper() => new BinaryOperator("+", (a, b) => a + b, 1);
        public static BinaryOperator SubOper() => new BinaryOperator("-", (a, b) => a - b, 1);
        public static BinaryOperator MulOper() => new BinaryOperator("*", (a, b) => a * b, 2);
        public static BinaryOperator DivOper() => new BinaryOperator("/", (a, b) => a / b, 2);
        public static BinaryOperator PowOper() => new BinaryOperator("^", (a, b) => Math.Pow(a, b), 3);
    }

    public class UnaryOperator : ParserToken
    {
        private Func<double, double> Func;
        public override double Eval(params double[] args) => Func(args[0]);

        public UnaryOperator(string name, Func<double, double> func) : base(name, int.MaxValue, 1)
        {
            Func = func;
        }

        public static UnaryOperator SelectOperator(char op)
        {
            switch (op)
            {
                case '-': return NegOper();
                case '~': return NegOper();
                case 's': return SinOper();
                case 'c': return CosOper();
                case 't': return TanOper();
                case 'l': return LogOper();
                case 'a': return AbsOper();
                default: throw new ArgumentException("Unknown operator");
            }
        }

        public static UnaryOperator NegOper() => new UnaryOperator("~", a => -a);
        public static UnaryOperator SinOper() => new UnaryOperator("sin", Math.Sin);
        public static UnaryOperator CosOper() => new UnaryOperator("cos", Math.Cos);
        public static UnaryOperator TanOper() => new UnaryOperator("tg", Math.Tan);
        public static UnaryOperator LogOper() => new UnaryOperator("ln", Math.Log);
        public static UnaryOperator AbsOper() => new UnaryOperator("abs", Math.Abs);
    }

    public class Function
    {
        private enum TokenType { None, Variable, Constant, Operator, BracketO, BracketC }

        private Dictionary<string, Variable> Variables = new Dictionary<string, Variable>();
        private List<string> variableOrder = new List<string>();
        private List<ParserToken> Postfix = new List<ParserToken>();

        public string FunctionText { get; }
        public int VariablesCount => variableOrder.Count;

        public Function(string expression)
        {
            FunctionText = expression;
            Parse();
        }

        public double Evaluate(params double[] args)
        {
            if (args.Length != variableOrder.Count)
                throw new EvaluatingException($"Expected {variableOrder.Count} arguments, got {args.Length}");

            for (int i = 0; i < variableOrder.Count; i++)
                Variables[variableOrder[i]].Value = args[i];

            Stack<double> values = new Stack<double>();
            foreach (ParserToken token in Postfix)
            {
                if (token.Arguments == 0)
                {
                    values.Push(token.Eval());
                }
                else if (token.Arguments == 1)
                {
                    double val = values.Pop();
                    double res = token.Eval(val);
                    values.Push(res);
                }
                else if (token.Arguments == 2)
                {
                    double right = values.Pop();
                    double left = values.Pop();
                    double res = token.Eval(left, right);
                    if (double.IsInfinity(res) || double.IsNaN(res))
                        throw new MathException("indeterminate", left, right, token.Token);
                    values.Push(res);
                }
            }
            if (values.Count != 1)
                throw new EvaluatingException("Cannot evaluate expression, wrong postfix form");
            return values.Pop();
        }

        private class ParseContext
        {
            public string Expr;
            public int Pos;
            public string Token;
            public Stack<ParserToken> Stack;
            public List<ParserToken> Postfix;
            public TokenType Last;
            public Dictionary<string, Variable> Variables;
            public List<string> VariableOrder;
        }

        private void Parse()
        {
            string expr = PreprocessExpression(FunctionText);
            var ctx = new ParseContext
            {
                Expr = expr,
                Pos = 0,
                Token = "",
                Stack = new Stack<ParserToken>(),
                Postfix = this.Postfix,
                Last = TokenType.None,
                Variables = this.Variables,
                VariableOrder = this.variableOrder
            };

            while (ctx.Pos < ctx.Expr.Length)
            {
                char c = ctx.Expr[ctx.Pos];
                if (char.IsWhiteSpace(c))
                {
                    ctx.Pos++;
                    continue;
                }

                if (char.IsDigit(c) || c == '.' || c == ',')
                    ProcessDigitOrDot(ctx, c);
                else if (char.IsLetter(c))
                    ProcessLetter(ctx, c);
                else if (c == '(')
                    ProcessOpenBracket(ctx, c);
                else if (c == ')')
                    ProcessCloseBracket(ctx, c);
                else
                    ProcessOperatorOrUnary(ctx, c);

                ctx.Pos++;
            }

            PushOperand(ctx);
            while (ctx.Stack.Count > 0)
            {
                var t = ctx.Stack.Pop();
                if (t != "(" && t != ")")
                    ctx.Postfix.Add(t);
                else
                    throw new SyntaxException(ctx.Expr, "closing and opening brackets do not match");
            }
        }

        private string PreprocessExpression(string expr)
        {
            string lower = expr.ToLower();
            lower = lower.Replace("e", Math.E.ToString());
            lower = lower.Replace("pi", Math.PI.ToString());
            return lower;
        }

        private void ProcessDigitOrDot(ParseContext ctx, char c)
        {
            if (ctx.Last == TokenType.BracketC)
                throw new WrongSymbolException(ctx.Expr, ctx.Pos);
            if (ctx.Last == TokenType.Variable && (c == '.' || c == ','))
                throw new WrongSymbolException(ctx.Expr, ctx.Pos);
            if (ctx.Last != TokenType.Variable)
                ctx.Last = TokenType.Constant;
            ctx.Token += (c == '.' ? ',' : c);
        }

        private void ProcessLetter(ParseContext ctx, char c)
        {
            if (ctx.Last == TokenType.BracketC || ctx.Last == TokenType.Constant)
                throw new WrongSymbolException(ctx.Expr, ctx.Pos);
            ctx.Last = TokenType.Variable;
            ctx.Token += c;

            if (ctx.Token == "sin" || ctx.Token == "cos" || ctx.Token == "tan" ||
                ctx.Token == "tg" || ctx.Token == "log" || ctx.Token == "ln" ||
                ctx.Token == "abs")
            {
                char identifier = ctx.Token[0];
                ctx.Last = TokenType.Operator;
                PushOperand(ctx);
                UnaryOperator op = UnaryOperator.SelectOperator(identifier);
                StackUpdate(ctx, op);
                ctx.Token = "";
            }
        }

        private void ProcessOpenBracket(ParseContext ctx, char c)
        {
            if (ctx.Last == TokenType.BracketC || ctx.Last == TokenType.Variable || ctx.Last == TokenType.Constant)
                throw new WrongSymbolException(ctx.Expr, ctx.Pos);
            ctx.Last = TokenType.BracketO;
            ctx.Stack.Push(new ParserToken("("));
        }

        private void ProcessCloseBracket(ParseContext ctx, char c)
        {
            if (ctx.Last == TokenType.BracketO || ctx.Last == TokenType.Operator || ctx.Last == TokenType.None)
                throw new WrongSymbolException(ctx.Expr, ctx.Pos);
            PushOperand(ctx);
            while (ctx.Stack.Count > 0 && ctx.Stack.Peek() != "(")
                ctx.Postfix.Add(ctx.Stack.Pop());
            if (ctx.Stack.Count == 0)
                throw new SyntaxException(ctx.Expr, "closing and opening brackets do not match");
            ctx.Last = TokenType.BracketC;
            ctx.Stack.Pop();
        }

        private void ProcessOperatorOrUnary(ParseContext ctx, char c)
        {
            if (ctx.Last != TokenType.BracketO && ctx.Last != TokenType.Operator && ctx.Last != TokenType.None)
            {
                PushOperand(ctx);
                ctx.Last = TokenType.Operator;
                BinaryOperator op = BinaryOperator.SelectOperator(c);
                StackUpdate(ctx, op);
            }
            else if (ctx.Last != TokenType.Operator)
            {
                PushOperand(ctx);
                ctx.Last = TokenType.Operator;
                UnaryOperator op = UnaryOperator.SelectOperator(c);
                StackUpdate(ctx, op);
            }
            else
                throw new WrongSymbolException(ctx.Expr, ctx.Pos);
        }

        private void PushOperand(ParseContext ctx)
        {
            if (ctx.Last == TokenType.Constant)
            {
                double val = Convert.ToDouble(ctx.Token);
                ctx.Postfix.Add(new Constant(val));
            }
            else if (ctx.Last == TokenType.Variable)
            {
                if (!ctx.Variables.ContainsKey(ctx.Token))
                {
                    ctx.Variables.Add(ctx.Token, new Variable(ctx.Token));
                    ctx.VariableOrder.Add(ctx.Token);
                }
                ctx.Postfix.Add(ctx.Variables[ctx.Token]);
            }
            ctx.Token = "";
        }

        private void StackUpdate(ParseContext ctx, ParserToken op)
        {
            while (ctx.Stack.Count > 0 && op.Order <= ctx.Stack.Peek().Order)
            {
                ParserToken t = ctx.Stack.Pop();
                if (t != "(" && t != ")")
                    ctx.Postfix.Add(t);
            }
            ctx.Stack.Push(op);
        }
    }
}