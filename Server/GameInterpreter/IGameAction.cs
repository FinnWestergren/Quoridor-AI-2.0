﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameInterpreter
{
    public interface IGameAction<T>
    {
        int SerializedAction();
    }
}
