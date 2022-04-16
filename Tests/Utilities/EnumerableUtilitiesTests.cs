using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Utilities
{
    [TestClass]
    public class EnumerableUtilitiesTests
    {

        [TestMethod]
        public void ToSquareArray()
        {
            var testEnumerable = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var square = EnumerableUtilities.ToSquareArray(testEnumerable);
            Assert.AreEqual(0, square[0, 0]);
            Assert.AreEqual(1, square[1, 0]);
            Assert.AreEqual(2, square[2, 0]);
            Assert.AreEqual(3, square[0, 1]);
        }

        [TestMethod]
        public void FromSquareArray()
        {
            var expected = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var square = EnumerableUtilities.ToSquareArray(expected);
            var actual = EnumerableUtilities.From2DArray(square).ToList();
            for(var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
