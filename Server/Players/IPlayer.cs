using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Players
{
    public interface IPlayer
    {
        public Guid PlayerId { get; set; }
    }
}
