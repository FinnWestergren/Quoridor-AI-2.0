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

        public static QuoridorBoard ParseBoard(string boardString)
        {
            AssertValidBoardString(boardString);

            var split = boardString.Split('_');

            var output = new QuoridorBoard
            {
                PlayerWallCounts = ParseWallCounts(split[2]),
                Cells = ParseCellString(split[1]),
                Walls = ParseWallString(split[0])
            };

            QuoridorValidator.AssertValidBoard(output);
            return output;
        }

        public static QuoridorBoard TryCommitActionToBoard(int serializedAction, QuoridorBoard board, PLAYER_ID committedBy, bool skipValidation = false)
        {

            var action = DeserializeAction(serializedAction);
            action.CommittedBy = committedBy;
            if (skipValidation) return CommitAction(action, board);
            QuoridorValidator.AssertValidAction(action, board);
            var newBoard = CommitAction(action, board);
            QuoridorValidator.AssertValidBoard(newBoard);
            return newBoard;
        }

        public static (IEnumerable<QuoridorMoveAction> moveActions, IEnumerable<QuoridorWallAction> wallActions) GetPossibleMoves(QuoridorBoard board, PLAYER_ID player)
        {
            var moveActions = board.GetAvailableDestinations(board.PlayerPositions[player]).Select(cell => new QuoridorMoveAction
            {
                Cell = cell,
                CommittedBy = player
            });

            if (board.PlayerWallCounts[player] <= 0) return (moveActions, new List<QuoridorWallAction>());
            var slots = board.GetEmptyWallSlots();
            var wallActions = slots
                .Select(ws => new QuoridorWallAction
                {
                    Col = ws.Col,
                    Row = ws.Row,
                    Orientation = ws.Orientation,
                    CommittedBy = player
                })
                .Where(action =>
                {
                    if(EdgeChecker.CheckWallTouching(action, board)) // pre-check if it's even possible that this is blocking
                    {
                        var newBoard = CommitAction(action, board);
                        return QuoridorValidator.IsValidBoard(newBoard).value; // if the wall is touching we have to make sure that this is not blocking
                    }
                    return true;
                });

            return (moveActions, wallActions);
        }
        public static bool IsWinCondition(PLAYER_ID player, QuoridorBoard board)
        {
            var pos = board.PlayerPositions[player];
            if (player == PLAYER_ID.PLAYER_ONE)
            {
                return pos.Row == 0;
            }
            if (player == PLAYER_ID.PLAYER_TWO)
            {
                return pos.Row == DIMENSION - 1;
            }
            throw new Exception("Invalid Player Id");
        }


        private static QuoridorBoard CommitAction(IGameAction action, QuoridorBoard board)
        {
            var newBoard = board.Copy();
            if (IsWallAction(action.SerializedAction))
            {
                var wallAction = (QuoridorWallAction) action;
                newBoard.PlayerWallCounts[action.CommittedBy] = newBoard.PlayerWallCounts[action.CommittedBy] - 1;
                newBoard.Walls[wallAction.Col, wallAction.Row] = wallAction.Orientation;
            }
            else
            {
                var moveAction = (QuoridorMoveAction) action;
                newBoard.SetPlayerPosition(moveAction.CommittedBy, moveAction.Cell);
            }
            return newBoard;
        }

        public static bool IsWallAction(int serializedAction) => serializedAction >= QuoridorWallAction.WALL_SERIALIZATION_FACTOR;
        private static IGameAction DeserializeAction(int serializedAction)
        {
            if (IsWallAction(serializedAction)) return DeserializeWallAction(serializedAction);
            return DeserializeMoveAction(serializedAction);
        }

        public static QuoridorWallAction DeserializeWallAction(int serializedAction)
        {
            var orientation = serializedAction / QuoridorWallAction.WALL_SERIALIZATION_FACTOR;
            var remainder = serializedAction % QuoridorWallAction.WALL_SERIALIZATION_FACTOR;
            var col = remainder / DIMENSION;
            var row = remainder % DIMENSION;
            if (orientation == 3)
            {
                throw new InvalidOperationException("3 is not a creative color");
            }
            return new QuoridorWallAction
            {
                Col = col,
                Row = row,
                Orientation = (WallOrientation) orientation
            };
        }
        public static QuoridorMoveAction DeserializeMoveAction(int serializedAction)
        {
            var col = serializedAction / DIMENSION;
            var row = serializedAction % DIMENSION;
            return new QuoridorMoveAction
            {
                Cell = new QuoridorCell(row, col)
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
                '|' => WallOrientation.Vertical,
                '-' => WallOrientation.Horizontal,
                '0' => WallOrientation.None,
                _ => throw new InvalidBoardException("Wall string contains invalid character(s)")
            });

            return EnumerableUtilities.ToSquareArray(allWalls);
        }

        private static QuoridorCell[,] ParseCellString(string cellString)
        {

            if (cellString.Length != _cellCount)
            {
                throw new InvalidBoardException($"expected {_cellCount} cell chars, recieved {cellString.Length} chars");
            }

            var allCells = cellString.Select((c, i) =>
            {
                PLAYER_ID player = c switch
                {
                    '0' => 0,
                    '1' => PLAYER_ID.PLAYER_ONE,
                    '2' => PLAYER_ID.PLAYER_TWO,
                    _ => throw new InvalidBoardException("Cell string contains invalid characters")
                };
                var col = i % DIMENSION;
                var row = i / DIMENSION;
                return new QuoridorCell(row, col, player);
            });

            return EnumerableUtilities.ToSquareArray(allCells);
        }

        private static Dictionary<PLAYER_ID, int> ParseWallCounts(string wallCounts)
            => new Dictionary<PLAYER_ID, int>
            {
                { PLAYER_ID.PLAYER_ONE, Convert.ToInt32("" + wallCounts[0], 16) },
                { PLAYER_ID.PLAYER_TWO, Convert.ToInt32("" + wallCounts[1], 16) }
            };
    }
}
