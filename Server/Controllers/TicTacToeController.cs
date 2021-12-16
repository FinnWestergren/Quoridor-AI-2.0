﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.GameInterpreter;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    public class TicTacToeController : ControllerBase
    {
        private readonly ILogger<TicTacToeController> _logger;

        public TicTacToeController(ILogger<TicTacToeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/ValidateBoard")]
        public ActionResult<IEnumerable<string>> ValidateBoard(string board)
        {
            var parsedBoard = new TicTacToeBoard(board);
            return new JsonResult(parsedBoard.BoardString());
        }

        [HttpGet]
        [Route("[controller]/GetPossibleMoves")]
        public ActionResult<IEnumerable<string>> GetPossibleMoves(string board)
        {
            var parsedBoard = new TicTacToeBoard(board);
            return new JsonResult(parsedBoard.GetPossibleMoves(null).Select(m => m.SerializedAction().ToString()));
        }


        [HttpGet]
        [Route("[controller]/CommitAction")]
        public ActionResult<IEnumerable<string>> CommitAction(string board, int serializedAction)
        {
            var parsedBoard = new TicTacToeBoard(board);
            parsedBoard.CommitAction(serializedAction);
            return new JsonResult(parsedBoard.BoardString());
        }
    }
}
