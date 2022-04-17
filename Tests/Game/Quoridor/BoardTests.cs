using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.Quoridor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Game.Quoridor
{
    [TestClass]
    public class BoardTests
    {

        private string _testBoard =
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

        private string _testBoard2 =
            "00000000" +
            "-0000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "_" +
            "000000000" +
            "000020000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000010000" +
            "_87";

        [TestMethod]
        public void TestEquals()
        {
            var board1 = QuoridorUtilities.ParseBoard(_testBoard);
            var board2 = QuoridorUtilities.ParseBoard(_testBoard);
            Assert.IsTrue(board1.Equals(board2));
        }

        [TestMethod]
        public void TestCopy()
        {
            var board1 = QuoridorUtilities.ParseBoard(_testBoard2);
            var board2 = board1.Copy();
            Assert.IsTrue(board1.Equals(board2));
        }
    }
}
