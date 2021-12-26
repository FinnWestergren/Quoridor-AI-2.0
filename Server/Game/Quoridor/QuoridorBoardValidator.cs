using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public static class QuoridorBoardValidator
    {
        private const int MAX_WALLS = 20;

        public static void AssertValidBoard(QuoridorBoard board)
        {
            var (isValid, error) = IsValidBoard(board);
            if (!isValid)
            {
                throw error;
            }
        }
        public static (bool value, Exception error) IsValidBoard(QuoridorBoard board)
        {
            var cellValidation = ValidateCells(board.Cells);
            var wallValidation = ValidateWalls(board.Walls);
            var pathValidation = ValidateClearPath(board);

            var outputBool = wallValidation.value && cellValidation.value && pathValidation.value;
            var outputError = wallValidation.error ?? cellValidation.error ?? pathValidation.error;
            return (outputBool, outputError);

        }
        private static (bool value, Exception error) ValidateCells(QuoridorCell[,] cells)
        {
            var list = EnumerableUtilities<QuoridorCell>.From2DArray(cells);

            var p1Count = list.Where(c => c.OccupiedBy == 1).Count();
            var p2Count = list.Where(c => c.OccupiedBy == 2).Count();
            var p3Count = list.Where(c => c.OccupiedBy == 3).Count();
            var p4Count = list.Where(c => c.OccupiedBy == 4).Count();

            if (p1Count == 0 || p2Count == 0)
            {
                return (false, new InvalidBoardException("Player 1 or Player 2 doesn't exist"));
            }

            if (p3Count != p4Count || p3Count > 1)
            {
                return (false, new InvalidBoardException("Player 3 and Player 4 occupy invalid amounts of tiles."));
            }

            if (p1Count > 1 || p2Count > 1 || p3Count > 1 || p4Count > 1)
            {
                return (false, new InvalidBoardException("A player occupies 2 or more tiles at once."));
            }

            return (true, null);
        }
        private static (bool value, Exception error) ValidateWalls(WallOrientation[,] walls)
        {
            var list = EnumerableUtilities<WallOrientation>.From2DArray(walls);
            if (list.Count(w => w != WallOrientation.None) > MAX_WALLS)
            {
                return (false, new InvalidBoardException("Maximum wall count exceeded."));
            }
            return (true, null);
        }

        private static (bool value, Exception error) ValidateClearPath(QuoridorBoard board)
        {
            return (true, null);
        }
    }
}
