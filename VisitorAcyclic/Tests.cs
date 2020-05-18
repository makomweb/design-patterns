using NUnit.Framework;
using System.Text;

namespace VisitorAcyclic
{
    public interface IVisitor<TVisitable>
    {
        void Visit(TVisitable obj);
    }

    public interface IVisitor { }

    // 3 - double expression
    // (1+2) (1+(2+3)) addition expression
    public abstract class Expression
    {
        public virtual void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Expression> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class DoubleExpression : Expression
    {
        public double Value { get; }

        public DoubleExpression(double value)
        {
            Value = value;
        }

        public virtual void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<DoubleExpression> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class AdditionExpression : Expression
    {
        public Expression Left, Right;

        public AdditionExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public virtual void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<AdditionExpression> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class ExpressionPrinter : IVisitor,
        IVisitor<Expression>,
        IVisitor<DoubleExpression>,
        IVisitor<AdditionExpression>
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public void Visit(Expression obj)
        {
        }

        public void Visit(DoubleExpression obj)
        {
            _sb.Append(obj.Value);
        }

        public void Visit(AdditionExpression obj)
        {
            _sb.Append("(");
            obj.Left.Accept(this);
            _sb.Append(" + ");
            obj.Right.Accept(this);
            _sb.Append(")");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }


    public class Tests
    {
        [Test]
        public void Test_using_the_print_visitor()
        {
            var exp = new AdditionExpression(
                left: new DoubleExpression(1.0),
                right: new AdditionExpression(
                    left: new DoubleExpression(2.0),
                    right: new DoubleExpression(3.0))
                );

            var printer = new ExpressionPrinter();

            printer.Visit(exp);

            string result = printer.ToString();
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}