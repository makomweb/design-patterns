using NUnit.Framework;
using System.Text;

namespace VisitorClassic
{
    public interface IExpressionVisitor
    {
        void Visit(DoubleExpression exp);

        void Visit(AdditionExpression exp);
    }

    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }

    public class DoubleExpression : Expression
    {
        public DoubleExpression(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class AdditionExpression : Expression
    {
        public AdditionExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public Expression Left { get; }
        public Expression Right { get; }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ExpressionPrinter : IExpressionVisitor
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public void Visit(DoubleExpression de)
        {
            _sb.Append(de.Value);
        }

        public void Visit(AdditionExpression ae)
        {
            _sb.Append("(");
            ae.Left.Accept(this);
            _sb.Append(" + ");
            ae.Right.Accept(this);
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
                new DoubleExpression(1.0),
                new AdditionExpression(
                    new DoubleExpression(2.0),
                    new DoubleExpression(3.0))
                );

            var printer = new ExpressionPrinter();

            printer.Visit(exp);

            string result = printer.ToString();
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}