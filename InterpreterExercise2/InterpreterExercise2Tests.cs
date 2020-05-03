using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InterpreterExercise2
{
    public class ExpressionProcessor
    {
        public Dictionary<char, int> Variables = new Dictionary<char, int>();

        public enum NextOp
        {
            Nothing,
            Plus,
            Minus
        }

        public int Calculate(string expression)
        {
            int current = 0;
            var nextOp = NextOp.Nothing;

            var parts = Regex.Split(expression, @"(?<=[+-])");

            foreach (var part in parts)
            {
                var noOp = part.Split(new[] { "+", "-" }, StringSplitOptions.RemoveEmptyEntries);
                var first = noOp[0];
                int value, z;

                if (int.TryParse(first, out z))
                    value = z;
                else if (first.Length == 1 && Variables.ContainsKey(first[0]))
                    value = Variables[first[0]];
                else return 0;

                switch (nextOp)
                {
                    case NextOp.Nothing:
                        current = value;
                        break;
                    case NextOp.Plus:
                        current += value;
                        break;
                    case NextOp.Minus:
                        current -= value;
                        break;
                }

                if (part.EndsWith("+")) nextOp = NextOp.Plus;
                else if (part.EndsWith("-")) nextOp = NextOp.Minus;
            }
            return current;
        }
    }

    public class InterpreterExercise2Tests
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