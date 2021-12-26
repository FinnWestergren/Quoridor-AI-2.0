using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Quoridor
{
    public static class QuoridorUtilities
    {
        private const int DIMENSION = 9;
        private const int SUBDIMENSION = DIMENSION - 1;
        private static int _cellCount = (int)Math.Pow(DIMENSION, 2);
        private static int _wallCount = (int)Math.Pow(SUBDIMENSION, 2);

        public static QuoridorBoard ParseBoard(string boardString)
        {
            var _expectedBoardStringLength = _cellCount + _wallCount + 1;

            if (boardString.Length != _expectedBoardStringLength)
            {
                throw new InvalidBoardException($"Expected {_expectedBoardStringLength} chars, recieved {boardString.Length} chars");
            }

            if (!boardString.Contains('_'))
            {
                throw new InvalidBoardException($"Must contain an underscore to separate walls and cells");
            }

            var split = boardString.Split('_');

            var wallString = split[0];
            var cellString = split[1];

            var walls = ParseWallString(wallString);
            var cells = ParseCellString(cellString);

            var output = new QuoridorBoard
            {
                Cells = cells,
                Walls = walls
            };
            QuoridorBoardValidator.AssertValidBoard(output);
            return output;
        }

        public static QuoridorBoard Empty2PlayerBoard = ParseBoard(
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
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
            "000010000"
        );

        public static QuoridorBoard Empty4PlayerBoard = ParseBoard(
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "00000000" +
            "_" +
            "000020000" +
            "000000000" +
            "000000000" +
            "000000000" +
            "400000003" +
            "000000000" +
            "000000000" +
            "000000000" +
            "000010000"
        );

        private static WallOrientation[,] ParseWallString(string wallString)
        {

            if (wallString.Length != _wallCount)
            {
                throw new InvalidBoardException($"expected {_wallCount} wall chars, recieved {wallString.Length} chars");
            }

            var allWalls = wallString.Select(c => c switch
            {
                '1' => WallOrientation.Vertical,
                '2' => WallOrientation.Horizontal,
                _ => WallOrientation.None
            });

            return EnumerableUtilities<WallOrientation>.To2DArray(allWalls, SUBDIMENSION);
        }

        private static QuoridorCell[,] ParseCellString(string cellString)
        {

            if (cellString.Length != _cellCount)
            {
                throw new InvalidBoardException($"expected {_cellCount} cell chars, recieved {cellString.Length} chars");
            }

            var allCells = cellString.Select((c, i) =>
            {
                int? player = c switch
                {
                    '0' => null,
                    '1' => 1,
                    '2' => 2,
                    '3' => 3,
                    '4' => 4,
                    _ => throw new InvalidBoardException("Cell string contains invalid characters")
                };
                var col = i / DIMENSION;
                var row = i % DIMENSION;
                return new QuoridorCell(row, col, player);
            });

            return EnumerableUtilities<QuoridorCell>.To2DArray(allCells, DIMENSION);
        }
    }
}
