﻿using Server.Utilities;
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
                PlayerWallCounts = ParseWallCounts(split[2], p1, p2),
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

        public static (IEnumerable<QuoridorMoveAction> moveActions, IEnumerable<QuoridorWallAction> wallActions) GetPossibleMoves(QuoridorBoard board, Guid playerId)
        {
            var moveActions = board.GetAvailableDestinations(board.PlayerPositions[playerId]).Select(cell => new QuoridorMoveAction
            {
                Cell = cell,
                CommittedBy = playerId
            });

            if (board.PlayerWallCounts[playerId] <= 0) return (moveActions, new List<QuoridorWallAction>());
            var slots = board.GetEmptyWallSlots().ToList();
            var wallActions = slots
                .Select(ws => new QuoridorWallAction
                {
                    Col = ws.Col,
                    Row = ws.Row,
                    Orientation = ws.Orientation,
                    CommittedBy = playerId
                })
                .Where(action =>
                {
                    if(CheckWallTouching(action, board))
                    {
                        var newBoard = CommitAction(action, board);
                        return QuoridorValidator.IsValidBoard(newBoard).value; // very expensive. I would say we want to check if this is even a necessary check before performing.
                    }
                    return true;

                });

            return (moveActions, wallActions);
        }

        private static bool CheckWallTouching(QuoridorWallAction action, QuoridorBoard board)
        {
            if (CheckWallTouchingEdge(action, board)) return true;
            var col = action.Col;
            var row = action.Row;
            var left = col + 1;
            var right = col - 1;
            var bottom = row + 1;
            var top = row - 1;

            bool withinBounds(int val)
            {
                return val <= SUBDIMENSION - 1 && val >= 0;
            }

            if (action.Orientation == WallOrientation.Horizontal)
            {
                if (board.Walls[left, row] == WallOrientation.Vertical || board.Walls[right, row] == WallOrientation.Vertical) return true;
                if (withinBounds(left - 1) && board.Walls[left - 1, row] == WallOrientation.Horizontal) return true;
                if (withinBounds(right + 1) && board.Walls[right + 1, row] == WallOrientation.Horizontal) return true;
                if (withinBounds(top) && board.Walls[col, top] == WallOrientation.Vertical) return true;
                if (withinBounds(bottom) && board.Walls[col, bottom] == WallOrientation.Vertical) return true;
            }

            if (action.Orientation == WallOrientation.Vertical)
            {
                if (board.Walls[col, top] == WallOrientation.Vertical || board.Walls[col, bottom] == WallOrientation.Horizontal) return true;
                if (withinBounds(top - 1) && board.Walls[col, top - 1] == WallOrientation.Vertical) return true;
                if (withinBounds(bottom + 1) && board.Walls[col, bottom + 1] == WallOrientation.Vertical) return true;
                if (withinBounds(right) && board.Walls[right, row] == WallOrientation.Horizontal) return true;
                if (withinBounds(left) && board.Walls[left, row] == WallOrientation.Horizontal) return true;
            }
            return false;
        }

        private static bool CheckWallTouchingEdge(QuoridorWallAction action, QuoridorBoard board)
        {
            if (action.Orientation == WallOrientation.Horizontal)
            {
                if (action.Col == 0 || action.Col == SUBDIMENSION - 1)
                {
                    return true;
                }
            }

            if (action.Orientation == WallOrientation.Vertical)
            {
                if (action.Row == 0 || action.Row == SUBDIMENSION - 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsWinCondition(Guid playerId, QuoridorBoard board)
        {
            var pos = board.PlayerPositions[playerId];
            if (board.PlayerOne == playerId)
            {
                return pos.Row == 0;
            }
            if (board.PlayerTwo == playerId)
            {
                return pos.Row == DIMENSION - 1;
            }
            throw new Exception("Invalid Player Id");
        }

        public static string PrintHumanReadableBoard(QuoridorBoard board)
        {
            var output = "";
            foreach (int row in Enumerable.Range(0, DIMENSION))
            {
                var nextRow = "\n";
                foreach (int col in Enumerable.Range(0, DIMENSION))
                {
                    var cell = board.Cells[col, row];
                    if (cell.OccupiedBy == null) output += "[ ]";
                    else if (cell.OccupiedBy == board.PlayerOne) output += "[1]";
                    else if (cell.OccupiedBy == board.PlayerTwo) output += "[2]";

                    var dests = board.GetAvailableDestinations(cell, false);
                    if (col < SUBDIMENSION)
                    {
                        var cellRight = board.Cells[col + 1, row];
                        if (!dests.Contains(cellRight)) output += "|";
                        else output += " ";
                    }
                    if (row < SUBDIMENSION)
                    {
                        var cellDown = board.Cells[col, row + 1];
                        if (!dests.Contains(cellDown)) nextRow += "--- ";
                        else nextRow += "    ";
                    }
                }
                output += nextRow + "\n";
            }
            return output;
        }

        private static QuoridorBoard CommitAction(IGameAction action, QuoridorBoard board)
        {
            var newBoard = CopyBoard(board);
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

        private static QuoridorBoard CopyBoard(QuoridorBoard board)
        {
            var newCells = EnumerableUtilities<QuoridorCell>.From2DArray(board.Cells)
                .Select(c => new QuoridorCell(c.Row, c.Col));
            var newWalls = EnumerableUtilities<WallOrientation>.From2DArray(board.Walls);
            return new QuoridorBoard
            {
                Cells = EnumerableUtilities<QuoridorCell>.ToSquareArray(newCells, DIMENSION),
                Walls = EnumerableUtilities<WallOrientation>.ToSquareArray(newWalls, DIMENSION),
                PlayerWallCounts = new Dictionary<Guid, int>(board.PlayerWallCounts),
                PlayerPositions = new Dictionary<Guid, QuoridorCell>(board.PlayerPositions),
                PlayerOne = board.PlayerOne,
                PlayerTwo = board.PlayerTwo
            };
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

            return EnumerableUtilities<WallOrientation>.ToSquareArray(allWalls, SUBDIMENSION);
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
                var col = i % DIMENSION;
                var row = i / DIMENSION;
                return new QuoridorCell(row, col, player);
            });

            return EnumerableUtilities<QuoridorCell>.ToSquareArray(allCells, DIMENSION);
        }

        private static Dictionary<Guid, int> ParseWallCounts(string wallCounts, Guid p1, Guid p2)
            => new Dictionary<Guid, int>
            {
                { p1, Convert.ToInt32("" + wallCounts[0], 16) },
                { p2, Convert.ToInt32("" + wallCounts[1], 16) }
            };
    }
}
