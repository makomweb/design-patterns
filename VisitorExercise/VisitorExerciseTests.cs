using NUnit.Framework;
using System.Text;

namespace VisitorExercise
{
    public abstract class ExpressionVisitor
    {
        public abstract void Visit(Value e);

        public abstract void Visit(AdditionExpression e);

        public abstract void Visit(MultiplicationExpression e);
    }

    public abstract class Expression
    {
        public abstract void Accept(ExpressionVisitor ev);
    }

    public class Value : Expression
    {
        public readonly int TheValue;

        public Value(int value)
        {
            TheValue = value;
        }

        public override void Accept(ExpressionVisitor ev)
        {
            ev.Visit(this);
        }
    }

    public class AdditionExpression : Expression
    {
        public readonly Expression LHS, RHS;

        public AdditionExpression(Expression lhs, Expression rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public override void Accept(ExpressionVisitor ev)
        {
            ev.Visit(this);
        }
    }

    public class MultiplicationExpression : Expression
    {
        public readonly Expression LHS, RHS;

        public MultiplicationExpression(Expression lhs, Expression rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public override void Accept(ExpressionVisitor ev)
        {
            ev.Visit(this);
        }
    }

    public class ExpressionPrinter : ExpressionVisitor
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public override void Visit(Value value)
        {
            _sb.Append(value.TheValue);
        }

        public override void Visit(AdditionExpression ae)
        {
            _sb.Append("(");
            ae.LHS.Accept(this);
            _sb.Append("+");
            ae.RHS.Accept(this);
            _sb.Append(")");
        }

        public override void Visit(MultiplicationExpression me)
        {
            me.LHS.Accept(this);
            _sb.Append("*");
            me.RHS.Accept(this);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }

    public class VisitorExerciseTests
    {
        [Test]
        public void Test_simple_addition()
        {
            var simple = new AdditionExpression(new Value(2), new Value(3));
            var ep = new ExpressionPrinter();
            ep.Visit(simple);
            Assert.That(ep.ToString(), Is.EqualTo("(2+3)"));
        }
    }
}