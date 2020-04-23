using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyweightExercise
{
    public class Sentence
    {
        private readonly Dictionary<string, WordToken> _tokens =
            new Dictionary<string, WordToken>();

        public Sentence(string plainText)
        {
            var token = plainText.Split(' ');

            foreach (var t in token)
            {
                _tokens.Add(t, new WordToken());
            }
        }

        public WordToken this[int index]
        {
            get
            {
                return _tokens[_tokens.Keys.ToList()[index]];
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var pair in _tokens)
            {
                sb.Append(pair.Value.Capitalize ? pair.Key.ToUpper() : pair.Key);
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }

        public class WordToken
        {
            public bool Capitalize { get; set; }
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