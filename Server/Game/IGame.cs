﻿using System;

namespace Server.Game
{
    public interface IGame
    {
        void CommitAction(int serializedAction, Guid playerId);
        void UndoAction();
        void Print();
        string GameType();
    }

}
