using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.TicTacToe;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Game
{
    [TestClass]
    public class TicTacToeUtilityTests
    {

        [TestMethod]
        public void ParsesBoard()
        {
            var board = TicTacToeUtilities.ParseBoard("XOX---XOX");
            Assert.IsTrue(board.Length == 9);
            Assert.IsTrue(board[0,1].OccupiedBy == PlayerMarker.O);
            Assert.IsTrue(board[0,2].OccupiedBy == PlayerMarker.X);
            Assert.IsTrue(board[1,2].OccupiedBy == PlayerMarker.None);
        }

        [TestMethod]
        public void CommitsActionImmutably()
        {
            var board = TicTacToeUtilities.ParseBoard("XOX---XOX");
            var (row, col) = (1, 2);
            var nextboard = TicTacToeUtilities.CommitActionToBoard(PlayerMarker.X, 5, board);
            Assert.IsTrue(board[row, col].OccupiedBy == PlayerMarker.None);
            Assert.IsTrue(nextboard[row, col].OccupiedBy == PlayerMarker.X);
        }

        [TestMethod]
        public void DeserializesCell()
        {
            var a = 0;
            var b = 8;
            var cellA = TicTacToeUtilities.DeserializeCell(a);
            var cellB = TicTacToeUtilities.DeserializeCell(b);
            Assert.IsTrue(cellA.Col == 0 && cellA.Row == 0);
            Assert.IsTrue(cellB.Col == 2 && cellB.Row == 2);
        }

        [TestMethod]
        public void ReturnsOpenCells()
        {
            var board = TicTacToeUtilities.ParseBoard("XOX---XOX");
            var openCells = TicTacToeUtilities.GetOpenCells(board);
            Assert.IsTrue(openCells.Count() == 3);
            Assert.IsTrue(!openCells.Any(c => c.IsOccupied));
        }

        [TestMethod]
        public void CatchesInvalidActions()
        {
            var board = TicTacToeUtilities.ParseBoard("XOX---XOX");
            var action1 = new Cell(0,0,PlayerMarker.X);
            Assert.IsFalse(TicTacToeUtilities.IsValidAction(action1, board).valid);
            var action2 = new Cell(1,1,PlayerMarker.None);
            Assert.IsFalse(TicTacToeUtilities.IsValidAction(action2, board).valid);
        }

        [TestMethod]
        public void AllowsValidActions()
        {
            var board = TicTacToeUtilities.ParseBoard("XOX---XOX");
            var action = new Cell(1, 1, PlayerMarker.X);
            Assert.IsTrue(TicTacToeUtilities.IsValidAction(action, board).valid);
        }

        [TestMethod]
        public void PrintsBoard()
        {
            var inString = "XOX---XOX";
            var board = TicTacToeUtilities.ParseBoard(inString);
            var outString = TicTacToeUtilities.PrintBoard(board);
            Assert.AreEqual(inString, outString);
        }

        [TestMethod]
        public void WinConditions()
        {
            TestWinConditionForPlayer(PlayerMarker.X);
            TestWinConditionForPlayer(PlayerMarker.O);
        }

        private static void TestWinConditionForPlayer(PlayerMarker p)
        {
            bool process(string b)
            {
                var board = TicTacToeUtilities.ParseBoard(b);
                return TicTacToeUtilities.IsWinCondition(p, board);
            }


            var winningBoardStrings = new List<string>
            {
                $"{p}{p}{p}------",
                $"---{p}{p}{p}---",
                $"------{p}{p}{p}",
                $"{p}---{p}---{p}",
                $"--{p}-{p}-{p}--",
                $"{p}--{p}--{p}--",
                $"-{p}--{p}--{p}-",
                $"--{p}--{p}--{p}"
            };

            var nonWinningBoardStrings = new List<string>
            {
                $"{p}{p}-------",
                $"---{p}{p}----",
                $"-------{p}{p}"
            };

            Assert.IsTrue(winningBoardStrings.All(s => process(s)));
            Assert.IsTrue(nonWinningBoardStrings.All(s => !process(s)));
        }
    }
}
