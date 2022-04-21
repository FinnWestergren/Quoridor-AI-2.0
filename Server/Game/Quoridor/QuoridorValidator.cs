using Server.Utilities;
using System;
using System.Linq;

namespace Server.Game.Quoridor
{
    public static class QuoridorValidator
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

        public static void AssertValidAction(IGameAction action, QuoridorBoard board)
        {
            if (QuoridorUtilities.IsWallAction(action.SerializedAction))
            {
                AssertValidWallAction((QuoridorWallAction) action, board);
            }
            else
            {
                AssertValidMoveAction((QuoridorMoveAction) action, board);
            }
        }

        public static (bool value, Exception error) IsValidBoard(QuoridorBoard board)
        {
            if (board.IsValidated) return (true, null);
            var wallValidation = ValidateWallCounts(board);
            var pathValidation = PathValidator.ValidatePath(board);

            var outputBool = wallValidation.value && pathValidation.value;
            var outputError = wallValidation.error ?? pathValidation.error;
            board.IsValidated = outputBool;
            return (outputBool, outputError);
        }
        public static (bool value, Exception error) ValidateWallCounts(QuoridorBoard board)
        {
            var walls = board.Walls;
            var list = EnumerableUtilities.From2DArray(walls);
            if (list.Count(w => w != WallOrientation.None) > MAX_WALLS)
            {
                return (false, new InvalidBoardException("Maximum wall count exceeded."));
            }

            if (board.PlayerWallCounts.Values.Any(count => count > MAX_WALLS / 2 || count < 0))
            {
                return (false, new InvalidBoardException("Player has invalid wall inventory."));
            }

            return (true, null);
        }

        private static void AssertValidWallAction(QuoridorWallAction action, QuoridorBoard board)
        {
            var (isValid, error) = ValidateWallAction(action, board);
            if (!isValid) throw error;
        }

        private static void AssertValidMoveAction(QuoridorMoveAction action, QuoridorBoard board)
        {
            var (isValid, error) = ValidateMoveAction(action, board);
            if (!isValid) throw error;
        }

        public static (bool value, Exception error) ValidateWallAction(QuoridorWallAction action, QuoridorBoard board)
        {
            var wallCount = board.PlayerWallCounts[action.CommittedBy];
            if (wallCount <= 0)
            {
                return (false, new InvalidOperationException("player doesn't have any walls left"));
            }

            var wallSlot = board.Walls[action.Col, action.Row];
            if (wallSlot != WallOrientation.None)
            {
                return (false, new InvalidOperationException("wall slot occupied"));
            }
            return (true, null);
        }

        public static (bool value, Exception error) ValidateMoveAction(QuoridorMoveAction action, QuoridorBoard board)
        {
            var fromCell = board.PlayerPositions[action.CommittedBy];
            var toCell = action.Cell;
            var availableCells = board.GetAvailableDestinations(fromCell);
            if (!availableCells.Any(c => c.Equals(toCell)))
            {
                return (false, new InvalidOperationException($"Invalid Move Action {fromCell.Print()} -> {toCell.Print()}"));
            }
            return (true, null);
        }

        private static (bool value, Exception error) ValidateClearPath(QuoridorBoard board)
        {

            return (true, null);
        }
    }
}
