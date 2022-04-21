﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Game;
using Server.Game.Quoridor;
using Server.InputModels;
using Server.Players.Agent;
using Server.Services;
using Server.Utilities;
using Server.ViewModels;
using System;

namespace Server.Controllers
{
    [ApiController]

    public class QuoridorController : ControllerBase, IGameController
    {
        private readonly GamePresentationService<Quoridor> _presentationService;
        private readonly GameCommandService<Quoridor> _commandService;

        public QuoridorController(IMemoryCache memoryCache)
        {
            _presentationService = new GamePresentationService<Quoridor>(memoryCache);
            _commandService = new GameCommandService<Quoridor>(memoryCache);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("[controller]/CommitAction")]
        public ActionResult CommitAction(ActionInputModel action)
        {
            var game = _presentationService.GetGame(action.GameId);
            game.CommitAction(action.SerializedAction, action.PlayerId);
            return new JsonResult(QuoridorGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetGame")]
        public ActionResult GetGame(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            return new JsonResult(QuoridorGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetMinimaxMove")]
        public ActionResult GetMinimaxMove(Guid gameId, PLAYER_ID player)
        {
            var game = _presentationService.GetGame(gameId);
            var (move, nodes, time) = GetMinimaxMove(player, game);
            if (move != null)
            {
                game.CommitAction(move.SerializedAction, player);
                _commandService.SaveGame(game);
            }
            return new JsonResult(QuoridorGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetPossibleActions")]
        public ActionResult GetPossibleActions(Guid gameId, PLAYER_ID player)
        {
            var game = _presentationService.GetGame(gameId);
            var (moveActions, wallActions) = QuoridorUtilities.GetPossibleMoves(game.CurrentBoard, player);
            return new JsonResult(new { possibleActions = new { moveActions, wallActions } });
        }

        [HttpGet]
        [Route("[controller]/IsWinCondition")]
        public ActionResult IsWinCondition(Guid gameId, PLAYER_ID player)
        {
            var game = _presentationService.GetGame(gameId);
            var isWin = QuoridorUtilities.IsWinCondition(player, game.CurrentBoard);
            return new JsonResult(isWin);
        }

        [HttpGet]
        [Route("[controller]/NewGame")]
        public ActionResult NewGame(string board = null)
        {
            var game = new Quoridor(board);
            _commandService.SaveGame(game);
            return new JsonResult(QuoridorGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/PrintBoard")]
        public ActionResult PrintBoard(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            return new JsonResult(BoardPrinter.PrintHumanReadableBoard(game.CurrentBoard));
        }

        private (IGameAction action, int nodesSearched, long time) GetMinimaxMove(PLAYER_ID player, IGame game, bool ABPrune = true)
        {
            var agent = new MiniMaxAgent(player, isABPruningEnabled: ABPrune, maxSearchDepth: 2);
            var (time, result) = ActionTimer.TimeFunction(() => agent.GetNextAction(game));
            return (result, agent.NodeCount, time);
        }
    }
}
