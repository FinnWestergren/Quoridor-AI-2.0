﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game
{
    public interface IGameAction
    {
        int SerializedAction { get; }
        int CommittedBy { get; set; }
    }
}
