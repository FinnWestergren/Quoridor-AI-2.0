using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server.Game.TicTacToe
{
    public class TicTacToe : IGame
    {
        private readonly Stack<TicTacToeCell[,]> _history;
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }
        public Guid GameId { get; set; }
        private Guid? _whoWentLast = null;

        public TicTacToe(string boardString = null)
        {
            _history = new Stack<TicTacToeCell[,]>();
            var board = boardString == null ? TicTacToeUtilities.EmptyBoard : TicTacToeUtilities.ParseBoard(boardString);
            _history.Push(board);
            PlayerOne = Guid.NewGuid();
            PlayerTwo = Guid.NewGuid();
            GameId = Guid.NewGuid();
        }

        public TicTacToe(string boardString, Guid playerOne, Guid playerTwo, Guid gameId)
        {
            _history = new Stack<TicTacToeCell[,]>();
            _history.Push(TicTacToeUtilities.ParseBoard(boardString));
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            GameId = gameId;
        }

        public void CommitAction(int serializedAction, Guid playerId, bool skipValidation = false)
        {
            if(_whoWentLast == playerId)
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
            _whoWentLast = playerId;
        }

        public Guid GetEnemyId(Guid playerId) => playerId == PlayerOne ? PlayerTwo : PlayerOne;

        public void UndoAction()
        {
            _history.Pop();
            if (_whoWentLast != null) _whoWentLast = GetEnemyId((Guid) _whoWentLast);
        }

        public void Print() => TicTacToeUtilities.PrintBoard(CurrentBoard);

        public TicTacToeCell[,] CurrentBoard => _history.Peek();

        public PlayerMarker GetPlayerMarker(Guid playerId)
        {
            if (playerId == PlayerOne) return PlayerMarker.X;
            if (playerId == PlayerTwo) return PlayerMarker.O;
            throw new Exception($"player ID {playerId} not registered");
        }

        public string GameType() => "TicTacToe";

        public IEnumerable<IGameAction> GetPossibleMoves(Guid playerId)
        {
            if (GetBoardValue(playerId) != 0)
            {
                return new List<IGameAction>(); // game over
            }
            var openCells = TicTacToeUtilities.GetOpenCells(CurrentBoard);
            return openCells.Select(c => new TicTacToeAction(c, playerId));
        }

        public int GetBoardValue(Guid playerId)
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
    }
}
