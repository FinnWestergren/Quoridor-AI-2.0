﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game
{
    public interface IGameAction
    {
        int SerializedAction { get; }
        PLAYER_ID CommittedBy { get; set; }
    }
}
