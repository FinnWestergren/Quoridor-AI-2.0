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
        private static int _wallCountCount = 2;
        private static int _underscoreCount = 2;

        public static string Empty2PlayerBoardString =
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
            "000010000" + 
            "_AA";

        public static QuoridorBoard ParseBoard(string boardString, Guid p1, Guid p2)
        {
            AssertValidBoardString(boardString);

            var split = boardString.Split('_');

            var output = new QuoridorBoard
            {
                PlayerWallCounts = ParseWallCounts(split[1], p1, p2),
                Cells = ParseCellString(split[1], p1, p2),
                Walls = ParseWallString(split[0]),
                PlayerOne = p1,
                PlayerTwo = p2
            };

            QuoridorValidator.AssertValidBoard(output);
            return output;
        }

        public static QuoridorBoard TryCommitActionToBoard(int serializedAction, QuoridorBoard board, Guid commitedBy)
        {
            var action = DeserializeAction(serializedAction);
            action.CommittedBy = commitedBy;
            QuoridorValidator.AssertValidAction(action, board);
            var newBoard = CommitAction(action, board);
            QuoridorValidator.AssertValidBoard(newBoard);
            return newBoard;
        }

        private static QuoridorBoard CommitAction(IGameAction action, QuoridorBoard board)
        {
            var newBoard = CopyBoard(board);
            if (QuoridorUtilities.IsWallAction(action.SerializedAction))
            {
                var wallAction = (QuoridorWallAction) action;
                newBoard.PlayerWallCounts[action.CommittedBy] = newBoard.PlayerWallCounts[action.CommittedBy] - 1;
                newBoard.Walls[wallAction.Row, wallAction.Col] = wallAction.Orientation;
            }
            else
            {
                var moveAction = (QuoridorMoveAction)action;
                newBoard.SetPlayerPosition(moveAction.CommittedBy, moveAction.Cell);
            }
            return newBoard;
        }

        private static QuoridorBoard CopyBoard(QuoridorBoard board)
        {
            var newCells = EnumerableUtilities<QuoridorCell>.From2DArray(board.Cells)
                .Select(c => new QuoridorCell(c.Row, c.Col));
            var newWalls = EnumerableUtilities<WallOrientation>.From2DArray(board.Walls);
            return new QuoridorBoard
            {
                Cells = EnumerableUtilities<QuoridorCell>.To2DArray(newCells, DIMENSION),
                Walls = EnumerableUtilities<WallOrientation>.To2DArray(newWalls, DIMENSION),
                PlayerWallCounts = new Dictionary<Guid, int>(board.PlayerWallCounts),
                PlayerPositions = new Dictionary<Guid, QuoridorCell>(board.PlayerPositions),
                PlayerOne = board.PlayerOne,
                PlayerTwo = board.PlayerTwo
            };
        }

        public static bool IsWallAction(int serializedAction) => serializedAction % 256 == 0;
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
            var _expectedBoardStringLength = _cellCount + _wallCount + _wallCountCount + _underscoreCount;

            if (boardString.Length != _expectedBoardStringLength)
            {
                throw new InvalidBoardException($"Expected {_expectedBoardStringLength} chars, recieved {boardString.Length} chars");
            }

            if (boardString.Split('_').Length != 3)
            {
                throw new InvalidBoardException($"Must contain underscores to separate walls, cells, and wall counts");
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

        private static QuoridorCell[,] ParseCellString(string cellString, Guid p1, Guid p2)
        {

            if (cellString.Length != _cellCount)
            {
                throw new InvalidBoardException($"expected {_cellCount} cell chars, recieved {cellString.Length} chars");
            }

            var allCells = cellString.Select((c, i) =>
            {
                Guid? player = c switch
                {
                    '0' => null,
                    '1' => p1,
                    '2' => p2,
                    _ => throw new InvalidBoardException("Cell string contains invalid characters")
                };
                var col = i / DIMENSION;
                var row = i % DIMENSION;
                return new QuoridorCell(row, col, player);
            });

            return EnumerableUtilities<QuoridorCell>.To2DArray(allCells, DIMENSION);
        }

        private static Dictionary<Guid, int> ParseWallCounts(string wallCounts, Guid p1, Guid p2)
            => new Dictionary<Guid, int>
            {
                { p1, Convert.ToInt32("" + wallCounts[0], 16) },
                { p2, Convert.ToInt32("" + wallCounts[1], 16) }
            };
    }
}
