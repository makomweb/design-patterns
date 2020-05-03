using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public interface IElement
    {
        int Value { get; }
    }

    public class Integer : IElement
    {
        public Integer(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class BinaryOperation : IElement
    {
        public enum Type { Addition, Substraction }

        public Type MyType;

        public IElement Left, Right;

        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Substraction:
                        return Left.Value - Right.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

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

        public static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOperation();

            bool haveLHS = false;

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        var integer = new Integer(int.Parse(token.Text));
                        if (!haveLHS)
                        {
                            result.Left = integer;
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = integer;
                        }
                        break;
                    case Token.Type.Plus:
                        result.MyType = BinaryOperation.Type.Addition;
                        break;
                    case Token.Type.Minus:
                        result.MyType = BinaryOperation.Type.Substraction;
                        break;
                    case Token.Type.Lparen:
                        {
                            int j = i;
                            for (; j < tokens.Count; ++j)
                            {
                                if (tokens[j].MyType == Token.Type.Rparen)
                                {
                                    break;
                                }
                            }

                            var subExpr = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                            var elem = Parse(subExpr);
                            if (!haveLHS)
                            {
                                result.Left = elem;
                                haveLHS = true;
                            }
                            else
                            {
                                result.Right = elem;
                            }

                            i = j; // advance
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
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

            var parsed = Logic.Parse(tokens);
            var value = parsed.Value;

            Debug.WriteLine($"{input} = {value}");

            Assert.AreEqual(4, value);
        }        
    }
}