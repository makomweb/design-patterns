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
        public void Test_if_rolling_back_to_first_memento_works()
        {
            var m = new TokenMachine();

            var m1 = m.AddToken(111);
            var m2 = m.AddToken(222);
            var m3 = m.AddToken(333);

            m.Revert(m1);
            Assert.AreEqual(111, m.Tokens.Last().Value);
        }

        [Test]
        public void Changing_the_value_of_the_first_token_should_be_reverted_when_rolling_back()
        {
            var m = new TokenMachine();

            var t = new Token(111);
            var m1 = m.AddToken(t);
            var m2 = m.AddToken(222);
            var m3 = m.AddToken(333);

            t.Value = 444;

            m.Revert(m1);
            Assert.AreEqual(444, m.Tokens.Last().Value);
        }
    }
}