using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.Quoridor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Game
{
    [TestClass]
    public class EdgeCheckerTests
    {

        private string _testBoard =
            "0-000000" +
            "00000000" +
            "00-00000" +
            "00000|00" +
            "00000000" +
            "00000|00" +
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

        private QuoridorBoard Init(string boardStr)
        {

            Guid p1 = Guid.NewGuid();
            Guid p2 = Guid.NewGuid();
            return QuoridorUtilities.ParseBoard(boardStr, p1, p2);
        }

        private QuoridorWallAction CreateAction(int x, int y, WallOrientation o) => new QuoridorWallAction
        {
            Col = x,
            Row = y,
            Orientation = o
        };

        [TestMethod]
        public void CheckEveryEdgeVert()
        {
            var board = Init(_testBoard);
            var actionsToTest = new List<QuoridorWallAction>
            {
                CreateAction(1, 1, WallOrientation.Vertical),
                CreateAction(2, 1, WallOrientation.Vertical),
                CreateAction(3, 1, WallOrientation.Vertical),
                CreateAction(1, 2, WallOrientation.Vertical),
                CreateAction(3, 2, WallOrientation.Vertical),
                CreateAction(1, 3, WallOrientation.Vertical),
                CreateAction(2, 3, WallOrientation.Vertical),
                CreateAction(3, 3, WallOrientation.Vertical),
            };

            var matches = actionsToTest.Where(a => EdgeChecker.CheckWallTouching(a, board));
            Assert.AreEqual(2, matches.Count());
        }


        [TestMethod]
        public void CheckEveryEdgeHoriz()
        {
            var board = Init(_testBoard);
            var actionsToTest = new List<QuoridorWallAction>
            {
                CreateAction(4, 4, WallOrientation.Horizontal),
                CreateAction(5, 4, WallOrientation.Horizontal),
                CreateAction(6, 4, WallOrientation.Horizontal),
                CreateAction(4, 5, WallOrientation.Horizontal),
                CreateAction(6, 5, WallOrientation.Horizontal),
                CreateAction(4, 6, WallOrientation.Horizontal),
                CreateAction(5, 6, WallOrientation.Horizontal),
                CreateAction(6, 6, WallOrientation.Horizontal),
            };

            var matches = actionsToTest.Where(a => EdgeChecker.CheckWallTouching(a, board));
            Assert.AreEqual(3, matches.Count());
        }
    }
}
