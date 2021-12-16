using Server.GameInterpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Players.Agent
{
    public interface IAgent<TGame> : IPlayer<TGame> where TGame : IGame
    {
        public IGameAction<TGame> GetNextAction();
        public int GetValueOfAction(IGameAction<TGame> action);
    }
}
