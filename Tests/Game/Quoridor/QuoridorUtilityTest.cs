using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game;
using Server.Game.Quoridor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Game
{
    [TestClass]
    public  class QuoridorUtilityTest
    {
        private string _simpleTestBoard =
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "|0000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "_" +
            "000020000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000010000" +
            "_A9";


        private string _complexTestBoard =
            "00000000" +
            "00000000" +
            "00000|00" +
            "00000000" +
            "-0-0-|00" +
            "00000000" +
            "00|00000" +
            "00000000" +
            "_" +
            "000000000" +
            "000000000" +
            "000020000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000010000" +
            "_87";

        [TestMethod]
        public void ParsesSimpleBoard()
        {
            var board = QuoridorUtilities.ParseBoard(_simpleTestBoard);
            Assert.AreEqual(10, board.PlayerWallCounts[PLAYER_ID.PLAYER_ONE]);
            Assert.AreEqual(9, board.PlayerWallCounts[PLAYER_ID.PLAYER_TWO]);
        }

        [TestMethod]
        public void ParsesComplexBoard()
        {
            var board = QuoridorUtilities.ParseBoard(_complexTestBoard);
            Assert.AreEqual(board.PlayerWallCounts[PLAYER_ID.PLAYER_ONE], 8);
            Assert.AreEqual(board.PlayerWallCounts[PLAYER_ID.PLAYER_TWO], 7);
            var distP2 = PathValidator.GetDistanceForPlayer(board, PLAYER_ID.PLAYER_TWO);
            Assert.AreEqual(10, distP2);
        }

        [TestMethod]
        public void MoveActionSerialization()
        {
            var action = new QuoridorMoveAction
            {
                Cell = new QuoridorCell(3, 5)
            };

            var deserialized = QuoridorUtilities.DeserializeMoveAction(action.SerializedAction);
            Assert.AreEqual(action.SerializedAction, deserialized.SerializedAction);
        }

        [TestMethod]
        public void WallActionSerialization()
        {
            var action = new QuoridorWallAction
            {
                Row = 5,
                Col = 3,
                Orientation = WallOrientation.Horizontal

            };

            var deserialized = QuoridorUtilities.DeserializeWallAction(action.SerializedAction);
            Assert.AreEqual(action.SerializedAction, deserialized.SerializedAction);
        }
    }
}
