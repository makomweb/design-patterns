using NUnit.Framework;
using System.Text;

namespace VisitorDynamic
{
    public abstract class Expression
    {
    }

    public class DoubleExpression : Expression
    {
        public DoubleExpression(double value)
        {
            Value = value;
        }

        public double Value { get; }
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
    }

    public class ExpressionPrinter
    {
        public void Print(AdditionExpression ae, StringBuilder sb)
        {
            sb.Append("(");
            Print((dynamic)ae.Left, sb);
            sb.Append(" + ");
            Print((dynamic)ae.Right, sb);
            sb.Append(")");
        }

        public void Print(DoubleExpression de, StringBuilder sb)
        {
            sb.Append(de.Value);
        }
    }

    public class Tests
    {
        [Test]
        public void Test_using_the_print_visitor()
        {
            Expression exp = new AdditionExpression(
                new DoubleExpression(1.0),
                new AdditionExpression(
                    new DoubleExpression(2.0),
                    new DoubleExpression(3.0))
                );

            var sb = new StringBuilder();
            var printer = new ExpressionPrinter();

            printer.Print((dynamic)exp, sb);

            string result = sb.ToString();
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}