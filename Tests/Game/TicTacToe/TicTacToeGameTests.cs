using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game;
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
        }

        [TestMethod]
        public void FullConstructorWorks()
        {
            var gameId = Guid.NewGuid();
            var boardString = "XOX---XOX";
            var TTT = new TicTacToe(boardString, gameId);
            Assert.AreEqual(TicTacToeUtilities.PrintBoard(TTT.CurrentBoard), boardString);
            Assert.AreEqual(TTT.GameId, gameId);
        }

        [TestMethod]
        public void GetPlayerMarker()
        {
            var TTT = new TicTacToe();
            Assert.IsTrue(TTT.GetPlayerMarker(PLAYER_ID.PLAYER_ONE) == PlayerMarker.X);
            Assert.IsTrue(TTT.GetPlayerMarker(PLAYER_ID.PLAYER_TWO) == PlayerMarker.O);
            Assert.ThrowsException<Exception>(() => TTT.GetPlayerMarker(PLAYER_ID.NONE));

        }

        [TestMethod]
        public void CommitAndUndo()
        {
            var TTT = new TicTacToe();
            TTT.CommitAction(0, PLAYER_ID.PLAYER_ONE);
            Assert.IsTrue(TTT.CurrentBoard[0, 0].OccupiedBy == PlayerMarker.X);
            TTT.UndoAction();
            Assert.IsFalse(TTT.CurrentBoard[0, 0].IsOccupied);
        }

        [TestMethod]
        public void WinAndLoss()
        {
            var TTT = new TicTacToe("XXX------");
            Assert.IsTrue(TTT.GetBoardValue(PLAYER_ID.PLAYER_ONE) > 0);
            Assert.IsTrue(TTT.GetBoardValue(PLAYER_ID.PLAYER_TWO) < 0);
            Assert.IsTrue(!TTT.GetPossibleMoves(PLAYER_ID.PLAYER_ONE).Any());
            Assert.IsTrue(!TTT.GetPossibleMoves(PLAYER_ID.PLAYER_TWO).Any());
        }

        [TestMethod]
        public void Neutral()
        {
            var TTT = new TicTacToe("---------");
            Assert.IsTrue(TTT.GetBoardValue(PLAYER_ID.PLAYER_ONE) == 0);
            Assert.IsTrue(TTT.GetBoardValue(PLAYER_ID.PLAYER_TWO) == 0);
        }


    }
}
