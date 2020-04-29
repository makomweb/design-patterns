using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public class Token
    {
        public enum Type { Integer, Plus, Minus, Lparen, Rparen }

        public Type MyType { get; set; }
        public string Text { get; set; }

        public Token(Type myType, string text)
        {
            MyType = myType;
            Text = text;
        }

        public override string ToString()
        {
            return $"`{Text}`";
        }
    }

    public static class Logic
    {
        public static List<Token> Lex(string input)
        {
            var result = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+': result.Add(new Token(Token.Type.Plus, "+"));
                        break;
                    case '-': result.Add(new Token(Token.Type.Minus, "-"));
                        break;
                    case '(': result.Add(new Token(Token.Type.Lparen, "("));
                        break;
                    case ')': result.Add(new Token(Token.Type.Rparen, ")"));
                        break;
                    default:
                        {
                            var sb = new StringBuilder(input[i].ToString());
                            for (int j = i + 1; j < input.Length; ++j)
                            {
                                var c = input[j];

                                if (char.IsDigit(c))
                                {
                                    sb.Append(c);
                                    ++i;
                                }
                                else
                                {
                                    result.Add(new Token(Token.Type.Integer, sb.ToString()));
                                    break;
                                }
                            }
                        }
                        break;
                }
            }

            return result;
        }

        public static string AsString(this IEnumerable<Token> tokens)
        {
            return string.Join(" ", tokens);
        }
    }


    public class InterpreterTests
    {
        [Test]
        public void Test_tokenizing()
        {
            // Tokens: ( 13 + 4 ) - ( 12 ...
            var input = "(13+4)-(12+1)";
            var tokens = Logic.Lex(input);

            Assert.True(tokens.Any());
            var joined = tokens.AsString();
            Assert.False(string.IsNullOrEmpty(joined));
        }
    }
}