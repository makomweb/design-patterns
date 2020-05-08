using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MementoExercise
{
    public class Token
    {
        public int Value = 0;

        public Token(int value)
        {
            this.Value = value;
        }
    }

    public class Memento
    {
        public List<Token> Tokens { get; }

        public Memento(IEnumerable<Token> tokens)
        {
            Tokens = tokens.ToList();
        }
    }

    public class TokenMachine
    {
        public List<Token> Tokens = new List<Token>();

        public Memento AddToken(int value)
        {
            return AddToken(new Token(value));
        }

        public Memento AddToken(Token token)
        {
            Tokens.Add(token);
            return new Memento(Tokens);
        }

        public void Revert(Memento m)
        {
            Tokens = m.Tokens;
        }
    }

    public class MementoExerciseTests
    {
        [Test]
        public void Test1()
        {
            var m = new TokenMachine();

            var m1 = m.AddToken(111);
            var m2 = m.AddToken(222);
            var m3 = m.AddToken(333);

            m.Revert(m1);
            Assert.AreEqual(111, m.Tokens.Last().Value);
        }
    }
}