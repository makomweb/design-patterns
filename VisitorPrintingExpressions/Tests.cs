using NUnit.Framework;
using System.Text;

namespace VisitorPrintingExpressions
{
    public abstract class Expression
    {
        public abstract void Print(StringBuilder sb);
    }

    public class DoubleExpression : Expression
    {
        private readonly double _value;

        public DoubleExpression(double value)
        {
            _value = value;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(_value);
        }
    }

    public class AdditionExpression : Expression
    {
        private Expression _left;
        private Expression _right;

        public AdditionExpression(Expression left, Expression right)
        {
            _left = left;
            _right = right;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append("(");
            _left.Print(sb);
            sb.Append(" + ");
            _right.Print(sb);
            sb.Append(")");
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var exp = new AdditionExpression(
                new DoubleExpression(1.0),
                new AdditionExpression(
                    new DoubleExpression(2.0),
                    new DoubleExpression(3.0))
                );

            var sb = new StringBuilder();

            exp.Print(sb);

            Assert.False(string.IsNullOrEmpty(sb.ToString()));
        }
    }
}