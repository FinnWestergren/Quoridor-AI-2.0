using Microsoft.AspNetCore.Mvc;
using Server.InputModels;
using System;

namespace Server.Controllers
{
    public interface IGameController
    {
        ActionResult CommitAction(ActionInputModel action);
        ActionResult GetGame(Guid gameId);
        ActionResult GetMinimaxMove(Guid gameId, Guid playerId);
        ActionResult GetPossibleActions(Guid gameId, Guid? playerId = null);
        ActionResult IsWinCondition(Guid gameId, Guid playerId);
        ActionResult NewGame(string board = null);
        ActionResult PrintBoard(Guid gameId);
    }
}