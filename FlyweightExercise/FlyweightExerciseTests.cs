using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyweightExercise
{
    public class Sentence
    {
        private readonly List<WordToken> _token;

        public Sentence(string plainText)
        {
            _token = plainText.Split(' ')
                .Select(token => new WordToken(token))
                .ToList();
        }

        public WordToken this[int index]
        {
            get
            {
                return _token[index];
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var token in _token)
            {
                sb.Append(token.ToString());
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }

        public class WordToken
        {
            public bool Capitalize { get; set; }

            public string Value { get; private set; }

            public WordToken(string value, bool capitalize = false)
            {
                Capitalize = capitalize;
                Value = value;
            }

            public override string ToString()
            {
                return Capitalize ? Value.ToUpper() : Value;
            }
        }
    }

    public class FlyweightExerciseTests
    {
        [Test]
        public void Test_if_4th_token_is_capitalized()
        {
            var sentence = new Sentence("This is a brave new world");
            sentence[3].Capitalize = true; // starts counting at 0!
            Assert.AreEqual("This is a BRAVE new world", sentence.ToString());
        }
    }
}