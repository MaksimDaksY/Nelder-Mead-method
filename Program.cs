using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;

namespace NelderMead
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public class Vector
    {
        public Vector(int size)
        {
            Coordinates = new double[size];
            for (int i = 0; i < size; i++) Coordinates[i] = 0;
        }
        public Vector(double[] coordinates)
        {
            Coordinates = new double[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++) Coordinates[i] = coordinates[i];
        }
        public double[] Coordinates;
        public static Vector VectorSum(Vector first, Vector second)
        {
            Vector sum = new Vector(first.Coordinates.Length);
            for (int i = 0;i < first.Coordinates.Length;i++) sum.Coordinates[i] = first.Coordinates[i] + second.Coordinates[i];
            return sum;
        }
        public static Vector VectorSum(Vector[] vectors)
        {
            Vector sum = new Vector(vectors[0].Coordinates.Length);
            for (int i = 0; i < vectors[0].Coordinates.Length;i++)
            {
                for (int j = 0; j < vectors.Length;j++) sum.Coordinates[i] += vectors[j].Coordinates[i];
            }
            return sum;
        }
        public Vector Scale(double scale)
        {
            Vector scaledVector = new Vector(Coordinates);
            for (int i = 0; i < Coordinates.Length; i++) scaledVector.Coordinates[i] *= scale;
            return scaledVector;
        }
        public double Norm()
        {
            double norm = 0;
            for (int i = 0; i < Coordinates.Length;i++)
            {
                norm += Math.Pow(Coordinates[i], 2);
            }
            return Math.Sqrt(norm);
        }
    }
    public class Simplex
    {
        public Simplex(Vector[] points) 
        { 
            Points = new Vector[points.Length];
            for (int i = 0; i < points.Length; i++) Points[i] = points[i];
        }
        public Vector[] Points;
        public void SortPointsByResult(Function goalFunction)
        {
            double[] functionResults = new double[Points.Length];
            for (int i = 0; i < functionResults.Length; i++)
            {
                functionResults[i] = goalFunction.Value(Points[i]);
            }
            Array.Sort(functionResults, Points);
        }
        public void Shrink(double shrinkCoefficient)
        {
            for (int i = 1; i < Points.Length; i++)
            {
                Points[i] = Vector.VectorSum(Points[0], Vector.VectorSum(Points[i], Points[0].Scale(-1)).Scale(shrinkCoefficient));
            }
        }
    }
    public class NelderMeadAlgorithm
    {
        public NelderMeadAlgorithm(double alpha, double beta, double gamma, Simplex simplex, Function goalFunction)
        {
            ReflectionCoefficient = alpha;
            ExpansionCoefficient = beta;
            ContractCoefficient = gamma;
            CurrentSimplex = simplex;
            GoalFunction = goalFunction;
            EndReached = false;
        }
        public double ReflectionCoefficient; 
        public double ExpansionCoefficient; 
        public double ContractCoefficient;
        public Simplex CurrentSimplex; 
        public Function GoalFunction;
        public bool EndReached;
        public void NextIteration()
        {
            Vector bestpoint = CurrentSimplex.Points[0]; 
            Vector okpoint = CurrentSimplex.Points[CurrentSimplex.Points.Length - 2];
            Vector badpoint = CurrentSimplex.Points.Last();
            Vector midpoint = Vector.VectorSum(Vector.VectorSum(CurrentSimplex.Points), badpoint.Scale(-1)).Scale((double)1 / (CurrentSimplex.Points.Length-1));
            Vector reflected = Vector.VectorSum(midpoint, Vector.VectorSum(midpoint, badpoint.Scale(-1)).Scale(ReflectionCoefficient));
            if (GoalFunction.Value(reflected) < GoalFunction.Value(bestpoint))
            {
                Vector expanded = Vector.VectorSum(midpoint, Vector.VectorSum(reflected, midpoint.Scale(-1)).Scale(ExpansionCoefficient));
                if (GoalFunction.Value(expanded) < GoalFunction.Value(bestpoint)) { badpoint = expanded; }
                else { badpoint = reflected; }
            }
            else
            {
                if (GoalFunction.Value(reflected) < GoalFunction.Value(okpoint)) { badpoint = reflected; }
                else
                {
                    if (GoalFunction.Value(reflected) < GoalFunction.Value(badpoint))
                    { badpoint = reflected; }
                    Vector contracted = Vector.VectorSum(midpoint, Vector.VectorSum(badpoint, midpoint.Scale(-1)).Scale(ContractCoefficient));
                    if (GoalFunction.Value(contracted) < GoalFunction.Value(badpoint)) { badpoint = contracted; }
                    else 
                    {
                        CurrentSimplex.Points[CurrentSimplex.Points.Length-1] = badpoint;
                        CurrentSimplex.Shrink(0.5);
                    }
                }
            }
            CurrentSimplex.Points[CurrentSimplex.Points.Length - 1] = badpoint;
            CurrentSimplex.SortPointsByResult(GoalFunction);
            double dispersion = 
                Vector.VectorSum(Vector.VectorSum(CurrentSimplex.Points).Scale((double)1 / CurrentSimplex.Points.Length), CurrentSimplex.Points[0].Scale(-1)).Norm();
            if (dispersion < 0.000001) EndReached = true;
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
            switch(op)
            {
                case '+': return AddOper();
                case '-': return SubOper();
                case '*': return MulOper();
                case '/': return DivOper();
                case '^': return PowOper();
                default: throw new ArgumentException("Unknown operator");
            }
        }
        public static BinaryOperator AddOper() =>
            new BinaryOperator("+", (double a, double b) => a + b, 1);
        public static BinaryOperator SubOper() =>
            new BinaryOperator("-", (double a, double b) => a - b, 1);
        public static BinaryOperator MulOper() =>
            new BinaryOperator("*", (double a, double b) => a * b, 2);
        public static BinaryOperator DivOper() =>
            new BinaryOperator("/", (double a, double b) => a / b, 2);
        public static BinaryOperator PowOper() =>
            new BinaryOperator("^", (double a, double b) => Math.Pow(a, b), 3);
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
                default: throw new ArgumentException("Unknown operator");
            };
        }
        public static UnaryOperator NegOper() => new UnaryOperator("~", (double a) => -a);
        public static UnaryOperator SinOper() => new UnaryOperator("sin", (double a) => Math.Sin(a));
        public static UnaryOperator CosOper() => new UnaryOperator("cos", (double a) => Math.Cos(a));
        public static UnaryOperator TanOper() => new UnaryOperator("tg", (double a) => Math.Tan(a));
        public static UnaryOperator LogOper() => new UnaryOperator("ln", (double a) => Math.Log(a));
    }
    public class Function
    {
        private enum TokenType { None, Variable, Constant, Operator, BracketO, BracketC };
        private Dictionary<string, Variable> Variables = new Dictionary<string, Variable>();
        private List<ParserToken> Postfix = new List<ParserToken>();
        public string FunctionText = "";
        public int VariablesCount;
        public Function(string expression)
        {
            FunctionText = expression; Parse(); VariablesCount = Variables.Count;
        }
        public double GetValue(string token)
        {
            return Variables.ContainsKey(token) ? Variables[token] : double.NaN;
        }
        public void SetValue(string token, double value)
        {
            if (Variables.ContainsKey(token))
                Variables[token].Value = value;
            else
                Variables.Add(token, new Variable(token, value));
        }
        public void SetValues(string[] tokens, double[] values)
        {
            for (int i = 0; i < tokens.Length && i < values.Length; i++)
                SetValue(tokens[i], values[i]);
        }
        public List<ParserToken> Parse()
        {
            string replacementFunction = FunctionText.ToLower();
            replacementFunction = replacementFunction.Replace("e", Math.E.ToString());
            replacementFunction = replacementFunction.Replace("pi", Math.PI.ToString());
            string token = "";
            Stack<ParserToken> stack = new Stack<ParserToken>();
            TokenType last = TokenType.None;
            void pushOperand()
            {
                if (last == TokenType.Constant)
                {
                    Postfix.Add(new Constant(Convert.ToDouble(token)));
                }
                else if (last == TokenType.Variable)
                {
                    if (Variables.ContainsKey(token))
                        Postfix.Add(Variables[token]);
                    else
                    {
                        Variables.Add(token, new Variable(token));
                        Postfix.Add(Variables[token]);
                    }
                }
                token = "";
            }
            void stackUpdate(ParserToken op)
            {
                while ((stack.Count != 0) && op.Order <= stack.Peek().Order)
                {
                    ParserToken t = stack.Pop();
                    if (t != "(" && t != ")")
                        Postfix.Add(t);
                }
                stack.Push(op);
            }
            for (int i = 0; i < replacementFunction.Length; i++)
            {
                if (char.IsWhiteSpace(replacementFunction[i]))
                    continue;
                else if (char.IsDigit(replacementFunction[i]) || replacementFunction[i] == '.' || replacementFunction[i] == ',')
                {
                    if (last == TokenType.BracketC)
                        throw new WrongSymbolException(replacementFunction, i);
                    if (last == TokenType.Variable && (replacementFunction[i] == '.' || replacementFunction[i] == ','))
                        throw new WrongSymbolException(replacementFunction, i);
                    else if (last != TokenType.Variable)
                        last = TokenType.Constant;
                    token += replacementFunction[i] == '.' ? ',' : replacementFunction[i];
                }
                else if (char.IsLetter(replacementFunction[i]))
                {
                    if (last == TokenType.BracketC || last == TokenType.Constant)
                        throw new WrongSymbolException(replacementFunction, i);
                    last = TokenType.Variable;
                    token += replacementFunction[i];
                    if (token == "sin" || token == "cos" || token == "tan" || token == "tg" || token == "log" || token == "ln")
                    {
                        char identifier = token[0];
                        last = TokenType.Operator; pushOperand();
                        UnaryOperator op = UnaryOperator.SelectOperator(identifier);
                        stackUpdate(op);
                    }
                }
                else if (replacementFunction[i] == '(')
                {
                    if (last == TokenType.BracketC || last == TokenType.Variable || last == TokenType.Constant)
                        throw new WrongSymbolException(replacementFunction, i);
                    last = TokenType.BracketO;
                    stack.Push(new ParserToken("("));
                }
                else if (replacementFunction[i] == ')')
                {
                    if (last == TokenType.BracketO || last == TokenType.Operator || last == TokenType.None)
                        throw new WrongSymbolException(replacementFunction, i);
                    pushOperand();
                    while (stack.Count != 0 && stack.Peek() != "(")
                        Postfix.Add(stack.Pop());

                    if (stack.Count == 0)
                        throw new SyntaxException(replacementFunction, "closing and opening brackets do not match");
                    last = TokenType.BracketC;
                    stack.Pop();
                }
                else if (last != TokenType.BracketO && last != TokenType.Operator && last != TokenType.None)
                {
                    pushOperand(); last = TokenType.Operator;
                    BinaryOperator op = BinaryOperator.SelectOperator(replacementFunction[i]);
                    stackUpdate(op);
                }
                else if (last != TokenType.Operator)
                {
                    pushOperand(); last = TokenType.Operator;
                    UnaryOperator op = UnaryOperator.SelectOperator(replacementFunction[i]);
                    stackUpdate(op);
                }
                else throw new WrongSymbolException(replacementFunction, i);
            }
            pushOperand();
            while (stack.Count != 0)
            {
                if (stack.Peek() != "(" && stack.Peek() != ")")
                    Postfix.Add(stack.Pop());
                else
                    throw new SyntaxException(replacementFunction, "closing and opening brackets do not match");
            }
            return Postfix;
        }
        public double Value(Vector vector)
        {
            SetValues(Variables.Keys.ToArray(), vector.Coordinates);
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
                throw new EvaluatingException("Cannot evaluate expression, wrong postifx form");
            return values.Pop();
        }
    }
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
            Left = left; ;
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
}
