using NUnit.Framework;
using System;

namespace InterpreterExercise
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public interface IExpression
    {
        int Value { get; }
    }

    [DebuggerDisplay("{Value}")]
    public class Constant : IExpression
    {
        public Constant(string text) : this(int.Parse(text))
        {
        }

        public Constant(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    [DebuggerDisplay("{Value}")]
    public class Variable : IExpression
    {
        protected Variable(int value)
        {
            Value = value;
        }

        public static Variable Create(string text, Dictionary<char, int> map)
        {
            if (text.Length != 1)
            {
                throw new ArgumentException("Parameter is not a single character!", paramName: text);
            }

            return new Variable(map[text[0]]);
        }

        public int Value { get; }
    }

    [DebuggerDisplay("{Value}")]
    public class BinaryOperation : IExpression
    {
        public const char SubstractionKey = '-';
        public const char AdditionKey = '+';
        public IExpression Left;
        public IExpression Right;

        public enum Type { Undetermined, Addition, Substraction }

        public Type MyType = Type.Undetermined;

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
                    case Type.Undetermined:
                        return Left.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public class ExpressionProcessor
    {
        public Dictionary<char, int> Variables = new Dictionary<char, int>();
        public List<char> Operations = new List<char>
        {
            BinaryOperation.AdditionKey,
            BinaryOperation.SubstractionKey
        };

        public int Calculate(string expression)
        {
            var tokens = Lex(expression);
            var value = Calculate(tokens);
            return value;
        }

        private bool IsOperation(string text)
        {
            if (text.Length != 1)
                return false;

            var c = text[0];
            return Operations.Any(op => op == c);
        }

        private bool IsVariable(string text)
        {
            if (text.Length != 1)
                return false;

            var c = text[0];
            return Variables.ContainsKey(c);
        }

        private bool IsConstant(string text)
        {
            int result;
            return int.TryParse(text, out result);
        }

        private bool IsUnsupported(string text)
        {
            return !IsOperation(text) && !IsVariable(text) && !IsConstant(text);
        }

        private IExpression ToOperand(string token)
        {
            if (IsVariable(token))
            {
                return Variable.Create(token, Variables);
            }
            else if (IsConstant(token))
            {
                return new Constant(token);
            }

            throw new NotSupportedException($"Unsupported expression type {token}!");
        }

        private BinaryOperation.Type ToOperationType(string token)
        {
            if (token == BinaryOperation.AdditionKey.ToString())
            {
                return BinaryOperation.Type.Addition;
            }
            else if (token == BinaryOperation.SubstractionKey.ToString())
            {
                return BinaryOperation.Type.Substraction;
            }
            else
            {
                throw new NotSupportedException($"Unsupported operation type {token}!");
            }
        }

        private int Calculate(IReadOnlyList<string> tokens)
        {
            var result = 0;

            if (!tokens.Any(IsUnsupported))
            {
                result = ToOperand(tokens[0]).Value;

                for (int index = 1; index < tokens.Count; index++)
                {
                    var token = tokens[index];
                    if (IsOperation(token))
                    {
                        var operation = ToOperationType(token);
                        var operand = ToOperand(tokens[index + 1]).Value;
                        switch (operation)
                        {
                            case BinaryOperation.Type.Addition:
                                result += operand;
                                break;
                            case BinaryOperation.Type.Substraction:
                                result -= operand;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"Unsupported operation type {operation}!");
                        }

                        index++; // advance
                    }
                }
            }

            return result;
        }

        private List<string> Lex(string input)
        {
            var result = new List<string>();

            for (int i = 0; i < input.Length; ++i)
            {
                var candidate = input[i];

                if (Operations.Any(op => op == candidate))
                {
                    result.Add(candidate.ToString());
                }
                else
                {
                    var sb = new StringBuilder(candidate.ToString());
                    int j = i + 1;

                    for (; j < input.Length; ++j)
                    {
                        var c = input[j];

                        if (Operations.Any(op => op == c))
                        {
                            result.Add(sb.ToString());
                            result.Add(c.ToString());
                            break;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }

                    if (j == input.Length)
                    {
                        result.Add(sb.ToString());
                    }

                    i = j;
                }                
            }

            return result;
        }
    }

    public class InterpreterExerciseTests
    {
        private readonly ExpressionProcessor _processor = new ExpressionProcessor();

        [Test]
        public void One_plus_2_plus_3_should_return_6()
        {
            var input = "1+2+3";
            var value = _processor.Calculate(input);
            Assert.AreEqual(6, value);
        }

        [Test]
        public void One_plus_2_plus_xy_should_return_0()
        {
            var input = "1+2+xy";
            var value = _processor.Calculate(input);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void Ten_minus_2_minus_x_should_return_5()
        {
            _processor.Variables.Add('x', 3);

            var input = "10-2-x";
            var value = _processor.Calculate(input);
            Assert.AreEqual(5, value);
        }
    }
}