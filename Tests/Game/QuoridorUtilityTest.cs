using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            "00000000" +
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
            Guid p1 = Guid.NewGuid();
            Guid p2 = Guid.NewGuid();
            var board = QuoridorUtilities.ParseBoard(_simpleTestBoard, p1, p2);
            Assert.AreEqual(10, board.PlayerWallCounts[p1]);
            Assert.AreEqual(9, board.PlayerWallCounts[p2]);
        }

        [TestMethod]
        public void ParsesComplexBoard()
        {
            Guid p1 = Guid.NewGuid();
            Guid p2 = Guid.NewGuid();
            var board = QuoridorUtilities.ParseBoard(_complexTestBoard, p1, p2);
            Assert.AreEqual(board.PlayerWallCounts[p1], 8);
            Assert.AreEqual(board.PlayerWallCounts[p2], 7);
            var distP2 = PathValidator.GetDistanceForPlayer(board, p2);
            var print = QuoridorUtilities.PrintHumanReadableBoard(board);
            Assert.AreEqual(10, distP2);
        }
    }
}
