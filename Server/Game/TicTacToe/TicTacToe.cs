using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server.Game.TicTacToe
{
    public class TicTacToe : IGame
    {
        private readonly Stack<TicTacToeCell[,]> _history;
        public Guid GameId { get; set; }
        public PLAYER_ID WhosTurn { get; set; }


        public TicTacToe(string boardString = null, PLAYER_ID? whosTurn = null)
        {
            _history = new Stack<TicTacToeCell[,]>();
            WhosTurn = whosTurn ?? PLAYER_ID.PLAYER_ONE;
            var board = boardString == null ? TicTacToeUtilities.EmptyBoard : TicTacToeUtilities.ParseBoard(boardString);
            _history.Push(board);
            GameId = Guid.NewGuid();
        }

        public TicTacToe(string boardString, Guid gameId)
        {
            _history = new Stack<TicTacToeCell[,]>();
            _history.Push(TicTacToeUtilities.ParseBoard(boardString));
            GameId = gameId;
        }

        public void CommitAction(int serializedAction, PLAYER_ID playerId, bool skipValidation = false)
        {
            if(WhosTurn != playerId)
            {
                throw new Exception("You can't go twice in a row!");
            }
            if (IsGameOver())
            {
                throw new Exception("Game's already over.");
            }
            var playerMarker = GetPlayerMarker(playerId);
            var next = TicTacToeUtilities.CommitActionToBoard(playerMarker, serializedAction, CurrentBoard);
            _history.Push(next);
            ToggleWhosTurn();
        }

        public void UndoAction()
        {
            _history.Pop();
            ToggleWhosTurn();
        }

        public void Print() => TicTacToeUtilities.PrintBoard(CurrentBoard);

        public TicTacToeCell[,] CurrentBoard => _history.Peek();

        public PlayerMarker GetPlayerMarker(PLAYER_ID player)
        {
            if (player == PLAYER_ID.PLAYER_ONE) return PlayerMarker.X;
            if (player == PLAYER_ID.PLAYER_TWO) return PlayerMarker.O;
            throw new Exception($"player value {player} not valid");
        }

        public string GameType() => "TicTacToe";

        public IEnumerable<IGameAction> GetPossibleMoves(PLAYER_ID playerId)
        {
            if (GetBoardValue(playerId) != 0)
            {
                return new List<IGameAction>(); // game over
            }
            var openCells = TicTacToeUtilities.GetOpenCells(CurrentBoard);
            return openCells.Select(c => new TicTacToeAction(c, playerId));
        }

        public int GetBoardValue(PLAYER_ID playerId)
        {
            var playerMarker = GetPlayerMarker(playerId);
            var enemyMarker = playerMarker == PlayerMarker.X ? PlayerMarker.O : PlayerMarker.X;
            var openCellsCount = TicTacToeUtilities.GetOpenCells(CurrentBoard).Count();
            var win = TicTacToeUtilities.IsWinCondition(playerMarker, CurrentBoard);
            if (win) return 1 + openCellsCount; // favor early wins
            var loss = TicTacToeUtilities.IsWinCondition(enemyMarker, CurrentBoard);
            if (loss) return -1 - openCellsCount; // avoid early loses
            return 0;
        }

        public bool IsFull() => !TicTacToeUtilities.GetOpenCells(CurrentBoard).Any();
        public bool IsTie() => !TicTacToeUtilities.GetOpenCells(CurrentBoard).Any() && !(TicTacToeUtilities.IsWinCondition(PlayerMarker.X, CurrentBoard) || TicTacToeUtilities.IsWinCondition(PlayerMarker.O, CurrentBoard));
        public bool IsGameOver() => IsFull() || TicTacToeUtilities.IsWinCondition(PlayerMarker.X, CurrentBoard) || TicTacToeUtilities.IsWinCondition(PlayerMarker.O, CurrentBoard);
        private void ToggleWhosTurn()
        {
            WhosTurn = WhosTurn == PLAYER_ID.PLAYER_ONE ? PLAYER_ID.PLAYER_TWO : PLAYER_ID.PLAYER_ONE;
        }
    }
}
