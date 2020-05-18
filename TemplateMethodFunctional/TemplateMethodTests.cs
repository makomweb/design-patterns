using NUnit.Framework;
using System;
using System.Diagnostics;

namespace TemplateMethodFunctional
{
    public static class GameTemplate
    {
        public static string Run(
            Action start,
            Action takeTurn,
            Func<bool> haveWinner,
            Func<int> winningPlayer)
        {
            start();
            while (!haveWinner())
                takeTurn();

            var result = $"Player {winningPlayer()} wins.";
            Debug.WriteLine(result);
            return result;
        }
    }

    public class TemplateMethodTests
    {
        [Test]
        public void Test1()
        {
            var numberOfPlayers = 2;
            int currentPlayer = 0;
            int turn = 1, maxTurns = 10;

            void Start()
            {
                Debug.WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
            }

            bool HaveWinner()
            {
                return turn == maxTurns;
            }

            void TakeTurn()
            {
                Debug.WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
                currentPlayer = (currentPlayer + 1) % numberOfPlayers;
            }

            int WinningPlayer()
            {
                return currentPlayer;
            }

            var result = GameTemplate.Run(Start, TakeTurn, HaveWinner, WinningPlayer);

            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}