using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public static class PathValidator
    {
        public static (bool value, Exception error) ValidatePath(QuoridorBoard board)
        {
            var p1Clear = IsPathClearForPlayer(board, board.PlayerOne);
            var p2Clear = IsPathClearForPlayer(board, board.PlayerTwo);
            return (p1Clear && p2Clear, p1Clear && p2Clear ? null : new InvalidBoardException("Path not clear for one or more players"));
        }
        private static bool IsPathClearForPlayer(QuoridorBoard board, Guid player)
        {
            var yDestination = player == board.PlayerOne ? 0 : QuoridorUtilities.DIMENSION - 1;
            return BFS(board.PlayerPositions[player], yDestination, board);
        }

        // Naive approach just to get things started. I have a feeling I'll need to optimize this.
        private static bool BFS(QuoridorCell start, int yDest, QuoridorBoard board)
        {
            var queue = new Queue<QuoridorCell>();
            var visited = new List<int>();
            void push (QuoridorCell cell) {
                queue.Enqueue(cell);
                visited.Add(cell.SerializedCell(QuoridorUtilities.DIMENSION));
            }

            push(start);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var avaliableDestinations = board.GetAvailableDestinations(current)
                    .Where(c => !visited.Contains(c.SerializedCell(QuoridorUtilities.DIMENSION)))
                    .OrderBy(c => Math.Abs(yDest - c.Row)); // gg ez
                foreach (var cell in avaliableDestinations) {
                    if (cell.Row == yDest) return true;
                    push(cell);
                }
            }
            return false;
        }
    }
}
