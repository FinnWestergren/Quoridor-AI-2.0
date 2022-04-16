using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.TicTacToe;
using System;
using System.Linq;

namespace Tests.Game
{
    [TestClass]
    public class TicTacToeGameTests
    {
        [TestMethod]
        public void EmptyConstructorWorks()
        {
            var TTT = new TicTacToe();
            Assert.AreEqual(TicTacToeUtilities.PrintBoard(TTT.CurrentBoard), TicTacToeUtilities.PrintBoard(TicTacToeUtilities.EmptyBoard));
            Assert.IsNotNull(TTT.GameId);
            Assert.IsNotNull(TTT.PlayerOne);
            Assert.IsNotNull(TTT.PlayerTwo);
        }

        [TestMethod]
        public void FullConstructorWorks()
        {
            var p1 = Guid.NewGuid();
            var p2 = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var boardString = "XOX---XOX";
            var TTT = new TicTacToe(boardString,p1,p2,gameId);
            Assert.AreEqual(TicTacToeUtilities.PrintBoard(TTT.CurrentBoard), boardString);
            Assert.AreEqual(TTT.GameId, gameId);
            Assert.AreEqual(TTT.PlayerOne, p1);
            Assert.AreEqual(TTT.PlayerTwo, p2);
        }

        [TestMethod]
        public void GetPlayerMarker()
        {
            var TTT = new TicTacToe();
            Assert.IsTrue(TTT.GetPlayerMarker(TTT.PlayerOne) == PlayerMarker.X);
            Assert.IsTrue(TTT.GetPlayerMarker(TTT.PlayerTwo) == PlayerMarker.O);
            Assert.ThrowsException<Exception>(() => TTT.GetPlayerMarker(Guid.NewGuid()));

        }

        [TestMethod]
        public void CommitAndUndo()
        {
            var TTT = new TicTacToe();
            TTT.CommitAction(0, TTT.PlayerOne);
            Assert.IsTrue(TTT.CurrentBoard[0, 0].OccupiedBy == PlayerMarker.X);
            TTT.UndoAction();
            Assert.IsFalse(TTT.CurrentBoard[0, 0].IsOccupied);
        }

        [TestMethod]
        public void WinAndLoss()
        {
            var TTT = new TicTacToe("XXX------");
            Assert.IsTrue(TTT.GetBoardValue(TTT.PlayerOne) > 0);
            Assert.IsTrue(TTT.GetBoardValue(TTT.PlayerTwo) < 0);
            Assert.IsTrue(!TTT.GetPossibleMoves(TTT.PlayerOne).Any());
            Assert.IsTrue(!TTT.GetPossibleMoves(TTT.PlayerTwo).Any());
        }

        [TestMethod]
        public void Neutral()
        {
            var TTT = new TicTacToe("---------");
            Assert.IsTrue(TTT.GetBoardValue(TTT.PlayerOne) == 0);
            Assert.IsTrue(TTT.GetBoardValue(TTT.PlayerTwo) == 0);
        }


    }
}
