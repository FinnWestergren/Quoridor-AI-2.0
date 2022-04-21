using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Quoridor
{
    public class QuoridorBoard
    {
        public bool IsValidated { get; set; } = false;
        public WallOrientation[,] Walls { get; set; }
        public QuoridorCell[,] Cells { get; set; }
        public Dictionary<PLAYER_ID, int> PlayerWallCounts { get; set; }
        public Dictionary<PLAYER_ID, QuoridorCell> PlayerPositions
        {
            get
            {
                if (_playerPositions == null)
                {
                    EvaluatePlayerPositions();
                }
                return _playerPositions;
            }
            set
            {
                _playerPositions = value;
            }
        }

        private Dictionary<PLAYER_ID, QuoridorCell> _playerPositions = null;

        public IEnumerable<QuoridorCell> GetAvailableDestinations(QuoridorCell fromCell, bool concernedAboutOccupied = true)
        {
            var (row, col) = (fromCell.Row, fromCell.Col);

            var left = Math.Max(col - 1, 0);
            var top = Math.Max(row - 1, 0);
            var right = Math.Min(col, QuoridorUtilities.SUBDIMENSION - 1);
            var bottom = Math.Min(row, QuoridorUtilities.SUBDIMENSION - 1);

            var isBlockedLeft =
                col == 0 ||
                Walls[left, top] == WallOrientation.Vertical ||
                Walls[left, bottom] == WallOrientation.Vertical;

            var isBlockedRight =
                col == QuoridorUtilities.DIMENSION - 1 ||
                Walls[right, top] == WallOrientation.Vertical ||
                Walls[right, bottom] == WallOrientation.Vertical;

            var isBlockedTop =
                row == 0 ||
                Walls[right, top] == WallOrientation.Horizontal ||
                Walls[left, top] == WallOrientation.Horizontal;

            var isBlockedBottom =
                row == QuoridorUtilities.DIMENSION - 1 ||
                Walls[right, bottom] == WallOrientation.Horizontal ||
                Walls[left, bottom] == WallOrientation.Horizontal;

            var possible = new List<QuoridorCell>();
            if (!isBlockedLeft) possible.Add(Cells[col - 1, row]);
            if (!isBlockedRight) possible.Add(Cells[col + 1, row]);
            if (!isBlockedTop) possible.Add(Cells[col, row - 1]);
            if (!isBlockedBottom) possible.Add(Cells[col, row + 1]);

            if (concernedAboutOccupied)
            {
                var occupiedCell = possible.FirstOrDefault(c => c.IsOccupied);
                if (occupiedCell != null)
                {
                    possible.Remove(occupiedCell);
                    possible.AddRange(GetAvailableDestinations(occupiedCell, false));
                }
            }

            return possible;
        }

        private IEnumerable<WallSlot> _cachedEmptyWallSlots = null;

        public IEnumerable<WallSlot> GetEmptyWallSlots()
        {
            if (_cachedEmptyWallSlots == null)
            {
                _cachedEmptyWallSlots = EvaluateEmptyWallSlots().ToList();
            }
            return _cachedEmptyWallSlots;
        }

        private IEnumerable<WallSlot> EvaluateEmptyWallSlots()
        {
            foreach (var col in Enumerable.Range(0, QuoridorUtilities.SUBDIMENSION))
            {
                foreach (var row in Enumerable.Range(0, QuoridorUtilities.SUBDIMENSION))
                {
                    if (Walls[col, row] == WallOrientation.None)
                    {
                        var left = col - 1;
                        var top = row - 1;
                        var right = col + 1;
                        var bottom = row + 1;

                        var isBlockedLeft =
                            col > 0 &&
                            Walls[left, row] == WallOrientation.Horizontal;

                        var isBlockedRight =
                            col < QuoridorUtilities.SUBDIMENSION - 1 &&
                            Walls[right, row] == WallOrientation.Horizontal;

                        var isBlockedTop =
                            row > 0 &&
                            Walls[col, top] == WallOrientation.Vertical;

                        var isBlockedBottom =
                            row < QuoridorUtilities.SUBDIMENSION - 1 &&
                            Walls[col, bottom] == WallOrientation.Vertical;

                        if (!isBlockedLeft && !isBlockedRight) yield return new WallSlot { Col = col, Row = row, Orientation = WallOrientation.Horizontal };
                        if (!isBlockedTop && !isBlockedBottom) yield return new WallSlot { Col = col, Row = row, Orientation = WallOrientation.Vertical };
                    }
                }
            }
        }
        public void SetPlayerPosition(PLAYER_ID committedBy, QuoridorCell cell)
        {
            var oldCell = PlayerPositions[committedBy];
            Cells[oldCell.Col, oldCell.Row].OccupiedBy = 0;
            PlayerPositions[committedBy] = cell;
            Cells[cell.Col, cell.Row].OccupiedBy = committedBy;
        }

        private void EvaluatePlayerPositions()
        {
            _playerPositions = new Dictionary<PLAYER_ID, QuoridorCell>();
            foreach (var cell in Cells)
            {
                if (cell.IsOccupied)
                {
                    _playerPositions.Add(cell.OccupiedBy, cell);
                }
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as QuoridorBoard;
            var selfWallList = EnumerableUtilities.From2DArray(Walls).ToArray();
            var otherWallList = EnumerableUtilities.From2DArray(other.Walls).ToArray();
            var selfCellList = EnumerableUtilities.From2DArray(Cells).ToArray();
            var otherCellList = EnumerableUtilities.From2DArray(other.Cells).ToArray();

            for (var i = 0; i < selfWallList.Length; i ++)
            {
                if (selfWallList[i] != otherWallList[i]) return false;
            }

            for (var i = 0; i < selfCellList.Length; i++)
            {
                if (!selfCellList[i].Equals(otherCellList[i])) return false;
            }

            if (!PlayerPositions[PLAYER_ID.PLAYER_ONE].Equals(other.PlayerPositions[PLAYER_ID.PLAYER_ONE])) return false;
            if (!PlayerPositions[PLAYER_ID.PLAYER_TWO].Equals(other.PlayerPositions[PLAYER_ID.PLAYER_TWO])) return false;
            if (!PlayerWallCounts[PLAYER_ID.PLAYER_ONE].Equals(other.PlayerWallCounts[PLAYER_ID.PLAYER_ONE])) return false;
            if (!PlayerWallCounts[PLAYER_ID.PLAYER_TWO].Equals(other.PlayerWallCounts[PLAYER_ID.PLAYER_TWO])) return false;

            return true;
        }

        public QuoridorBoard Copy()
        {
            var newCells = EnumerableUtilities.From2DArray(Cells)
                .Select(c => new QuoridorCell(c.Row, c.Col, c.OccupiedBy));
            var newWalls = EnumerableUtilities.From2DArray(Walls);
            return new QuoridorBoard
            {
                Cells = EnumerableUtilities.ToSquareArray(newCells),
                Walls = EnumerableUtilities.ToSquareArray(newWalls),
                PlayerWallCounts = new Dictionary<PLAYER_ID, int>(PlayerWallCounts)
            };
        }
    }
}
