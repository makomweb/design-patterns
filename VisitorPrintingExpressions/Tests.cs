using NUnit.Framework;
using System;
using System.Text;
using System.Collections.Generic;

namespace VisitorPrintingExpressions
{
    using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

    public abstract class Expression
    {
        public abstract void Print(StringBuilder sb);
    }

    public class DoubleExpression : Expression
    {
        public DoubleExpression(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override void Print(StringBuilder sb)
        {
            sb.Append(Value);
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

        public override void Print(StringBuilder sb)
        {
            sb.Append("(");
            Left.Print(sb);
            sb.Append(" + ");
            Right.Print(sb);
            sb.Append(")");
        }
    }

    public static class ExpressionPrinter
    {
#if true
        private static readonly DictType _actions = new DictType
        {
            [typeof(DoubleExpression)] = (e, sb) =>
            {
                var de = (DoubleExpression)e;
                sb.Append(de.Value);
            },
            [typeof(AdditionExpression)] = (e, sb) =>
            {
                var ae = (AdditionExpression)e;
                sb.Append("(");
                ae.Left.Print(sb);
                sb.Append(" + ");
                ae.Right.Print(sb);
                sb.Append(")");
            }
        };

        public static void Print(Expression e, StringBuilder sb)
        {
            _actions[e.GetType()](e, sb);
        }
#else
        public static void Print(Expression e, StringBuilder sb)
        {
            if (e is DoubleExpression de)
            {
                sb.Append(de.Value);
            }
            else if (e is AdditionExpression ae)
            {
                sb.Append("(");
                ae.Left.Print(sb);
                sb.Append(" + ");
                ae.Right.Print(sb);
                sb.Append(")");
            }
        }
#endif

    public class Tests
    {
        [Test]
        public void Test_using_the_print_method()
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

        [Test]
        public void Test_using_the_print_function()
        {
            var exp = new AdditionExpression(
                new DoubleExpression(1.0),
                new AdditionExpression(
                    new DoubleExpression(2.0),
                    new DoubleExpression(3.0))
                );

            var sb = new StringBuilder();

            ExpressionPrinter.Print(exp, sb);

            Assert.False(string.IsNullOrEmpty(sb.ToString()));
        }
    }
}