using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Quoridor
{
    public static class QuoridorUtilities
    {
        public const int DEFAULT_WALL_COUNT = 10;
        public const int DIMENSION = 9;
        public const int SUBDIMENSION = DIMENSION - 1;
        private static int _cellCount = (int) Math.Pow(DIMENSION, 2);
        private static int _wallCount = (int) Math.Pow(SUBDIMENSION, 2);

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

        public static QuoridorBoard ParseBoard(string boardString)
        {
            AssertValidBoardString(boardString);

            var split = boardString.Split('_');

            var output = new QuoridorBoard
            {
                Cells = ParseCellString(split[1]),
                Walls = ParseWallString(split[0])
            };

            QuoridorBoardValidator.AssertValidBoard(output);
            return output;
        }

        public static QuoridorBoard CommitActionToBoard(int serializedAction, QuoridorBoard board, Guid commitedBy)
        {
            var action = DeserializeAction(serializedAction);
            action.CommittedBy = commitedBy;

            if (IsWallAction(serializedAction))
            {
                AssertValidWallAction((QuoridorWallAction) action, board);
            }
            else 
            {
                AssertValidMoveAction((QuoridorMoveAction) action, board);
            }
        }

        private static void AssertValidWallAction(QuoridorWallAction action, QuoridorBoard board)
        {
            
        }

        private static void AssertValidMoveAction(QuoridorMoveAction action, QuoridorBoard board)
        {
            throw new NotImplementedException();
        }


        private static bool IsWallAction(int serializedAction) => serializedAction % 256 == 0;
        private static IGameAction DeserializeAction(int serializedAction)
        {
            if (IsWallAction(serializedAction)) return DeserializeWallAction(serializedAction);
            return DeserializeMoveAction(serializedAction);
        }

        private static QuoridorWallAction DeserializeWallAction(int serializedAction)
        {
            var shiftedAction = serializedAction >> 8;
            var orientation = shiftedAction % 4;
            if (orientation == 3)
            {
                throw new InvalidOperationException("3 is not a creative color");
            }
            shiftedAction >>= 2;
            var row = shiftedAction % SUBDIMENSION;
            var col = shiftedAction / SUBDIMENSION;
            return new QuoridorWallAction
            {
                Col = col,
                Row = row,
                Orientation = (WallOrientation) orientation
            };
        }
        private static QuoridorMoveAction DeserializeMoveAction(int serializedAction)
        {
            return new QuoridorMoveAction
            {
                Cell = new QuoridorCell(serializedAction % SUBDIMENSION, serializedAction / SUBDIMENSION)
            };

        }

        private static void AssertValidBoardString(string boardString)
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
        }

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
