using NUnit.Framework;
using System.Diagnostics;

namespace TemplateMethod
{
    public abstract class Game
    {
        public string Run()
        {
            Start();
            while (!HaveWinner)
                TakeTurn();
            return $"Player {WinningPlayer} wins.";
        }

        protected abstract void Start();
        protected abstract bool HaveWinner { get; }
        protected abstract void TakeTurn();
        protected abstract int WinningPlayer { get; }

        protected int currentPlayer;
        protected readonly int numberOfPlayers;

        public Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }
    }

    public class Chess : Game
    {
        public Chess() : base(2)
        {
        }

        protected override void Start()
        {
            Debug.WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
        }

        protected override bool HaveWinner => _turn == _maxTurns;

        protected override void TakeTurn()
        {
            Debug.WriteLine($"Turn {_turn++} taken by player {currentPlayer}.");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }

        protected override int WinningPlayer => currentPlayer;

        private int _maxTurns = 10;
        private int _turn = 1;
    }

    public class TemplateMethodTests
    {
        [Test]
        public void Test1()
        {
            var game = new Chess();

            Assert.False(string.IsNullOrEmpty(game.Run()));
        }
    }
}