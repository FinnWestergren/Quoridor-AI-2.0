using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Game;
using Server.Game.TicTacToe;
using Server.InputModels;
using Server.Players.Agent;
using Server.Services;
using Server.Utilities;
using Server.ViewModels;
using System;

namespace Server.Controllers
{
    [ApiController]
    public class TicTacToeController : ControllerBase, IGameController
    {
        private readonly GamePresentationService<TicTacToe> _presentationService;
        private readonly GameCommandService<TicTacToe> _commandService;

        public TicTacToeController(IMemoryCache memoryCache)
        {
            _presentationService = new GamePresentationService<TicTacToe>(memoryCache);
            _commandService = new GameCommandService<TicTacToe>(memoryCache);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("[controller]/CommitAction")]
        public ActionResult CommitAction(ActionInputModel action)
        {
            var game = _presentationService.GetGame(action.GameId);
            game.CommitAction(action.SerializedAction, action.PlayerId);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetGame")]
        public ActionResult GetGame(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetMinimaxMove")]
        public ActionResult GetMinimaxMove(Guid gameId, Guid playerId)
        {
            var game = _presentationService.GetGame(gameId);
            var move = GetMinimaxMove(playerId, game).action;
            if (move != null)
            {
                game.CommitAction(move.SerializedAction, playerId);
                _commandService.SaveGame(game);
            }
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetPossibleActions")]
        public ActionResult GetPossibleActions(Guid gameId, Guid? playerId = null)
        {
            var game = _presentationService.GetGame(gameId);
            var moveSet = game.GetPossibleMoves(playerId ?? game.PlayerOne);
            return new JsonResult(moveSet);
        }

        [HttpGet]
        [Route("[controller]/IsWinCondition")]
        public ActionResult IsWinCondition(Guid gameId, Guid playerId)
        {
            var game = _presentationService.GetGame(gameId);
            var playerMarker = playerId == game.PlayerOne ? PlayerMarker.X : PlayerMarker.O;
            var isWin = TicTacToeUtilities.IsWinCondition(playerMarker, game.CurrentBoard);
            return new JsonResult(isWin);
        }

        [HttpGet]
        [Route("[controller]/NewGame")]
        public ActionResult NewGame(string board = null)
        {
            var game = new TicTacToe(board);
            _commandService.SaveGame(game);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/PrintBoard")]
        public ActionResult PrintBoard(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            return new JsonResult(TicTacToeUtilities.PrintHumanReadableBoard(game.CurrentBoard));
        }

        private (IGameAction action, int nodesSearched, long time) GetMinimaxMove(Guid playerId, IGame game, bool ABPrune = true)
        {
            var agent = new MiniMaxAgent(playerId, isABPruningEnabled: ABPrune);
            var (time, result) = ActionTimer.TimeFunction(() => agent.GetNextAction(game));
            return (result, agent.NodeCount, time);
        }
    }
}