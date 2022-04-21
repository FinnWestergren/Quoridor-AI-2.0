using Microsoft.AspNetCore.Mvc;
using Server.Game;
using Server.InputModels;
using System;

namespace Server.Controllers
{
    public interface IGameController
    {
        ActionResult CommitAction(ActionInputModel action);
        ActionResult GetGame(Guid gameId);
        ActionResult GetMinimaxMove(Guid gameId, PLAYER_ID playerId);
        ActionResult GetPossibleActions(Guid gameId, PLAYER_ID playerId);
        ActionResult IsWinCondition(Guid gameId, PLAYER_ID playerId);
        ActionResult NewGame(string board = null);
        ActionResult PrintBoard(Guid gameId);
    }
}