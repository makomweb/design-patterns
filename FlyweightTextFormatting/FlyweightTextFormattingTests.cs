using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace FlyweightTextFormatting
{
    public class FormattedText
    {
        private readonly string _plainText;
        private bool[] capitalize;

        public FormattedText(string plainText)
        {
            _plainText = plainText;
            capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (var i = start; i <= end; i++)
            {
                capitalize[i] = true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < _plainText.Length; i++)
            {
                var c = _plainText[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }

            return sb.ToString();
        }
    }

    public class BetterFormattedText
    {
        private string _plainText;
        private List<TextRange> _formatting = new List<TextRange>();
        public BetterFormattedText(string plainText)
        {
            _plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            _formatting.Add(range);

            return range;
        }

        public class TextRange
        {
            public int Start, End;

            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < _plainText.Length; i++)
            {
                var c = _plainText[i];
                foreach (var range in _formatting)
                {
                    if (range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }

                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }

    public class FlyweightTextFormattingTests
    {
        [Test]
        public void Test1()
        {
            var ft = new FormattedText("This is a brave new world");
            ft.Capitalize(10, 15); // capitalize 'brave'
            Assert.AreEqual("This is a BRAVE new world", ft.ToString());
        }

        [Test]
        public void Test2()
        {
            var bft = new BetterFormattedText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;
            Assert.AreEqual("This is a BRAVE new world", bft.ToString());
        }
    }
}