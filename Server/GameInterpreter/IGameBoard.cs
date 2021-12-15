using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameInterpreter
{
    interface IGameBoard
    {
        IEnumerable<GameAction> GetPossibleMoves();
    }
}
