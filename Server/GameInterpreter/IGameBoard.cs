using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameInterpreter
{
    public interface IGameBoard<TGame> where TGame : IGame
    {
        void CommitAction(IGameAction<TGame> action);
        void CommitAction(int serializedAction);
        IEnumerable<IGameAction<TGame>> GetPossibleMoves(Guid? playerId);
        GameType GetGameType();
    }

}
