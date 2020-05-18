using NUnit.Framework;
using System;
using System.Numerics;

namespace StrategyExercise
{
    public interface IDiscriminantStrategy
    {
        double CalculateDiscriminant(double a, double b, double c);
    }

    public class OrdinaryDiscriminantStrategy : IDiscriminantStrategy
    {
        // todo
        public double CalculateDiscriminant(double a, double b, double c)
        {
            return b * b - 4 * a * c;
        }
    }

    public class RealDiscriminantStrategy : IDiscriminantStrategy
    {
        // todo (return NaN on negative discriminant!)
        public double CalculateDiscriminant(double a, double b, double c)
        {
            var d = b * b - 4 * a * c;
            return d >= 0 ? d : double.NaN;
        }
    }

    public class QuadraticEquationSolver
    {
        private readonly IDiscriminantStrategy strategy;

        public QuadraticEquationSolver(IDiscriminantStrategy strategy)
        {
            this.strategy = strategy;
        }

        public Tuple<Complex, Complex> Solve(double a, double b, double c)
        {
            var d = strategy.CalculateDiscriminant(a, b, c);
            var cd = new Complex(d, 0);

            var x1 = (-b + Complex.Sqrt(cd)) / (2 * a);
            var x2 = (-b - Complex.Sqrt(cd)) / (2 * a);

            return Tuple.Create(x1, x2);
        }
    }

    public class StrategyExerciseTests
    {
        [Test]
        public void Test_calculation()
        {
            var a = 2.2;
            var b = 4.1;
            var c = 5.0;

            var solver = new QuadraticEquationSolver(new OrdinaryDiscriminantStrategy());

            var result = solver.Solve(a, b, c);

            Assert.NotNull(result);
        }
    }
}