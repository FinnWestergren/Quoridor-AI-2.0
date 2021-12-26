using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class Quoridor
    {
        private readonly Stack<QuoridorBoard> _history;
        private Dictionary<int, Guid> _playerIds;
        public Guid GameId { get; set; }

        public Quoridor(string boardString)
        {

        }

    }
}
