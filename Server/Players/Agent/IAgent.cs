using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Players.Agent
{
    public interface IAgent : IPlayer
    {
        public IGameAction GetNextAction(IGame game);
    }
}
