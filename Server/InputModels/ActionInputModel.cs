using Server.Game;
using System;

namespace Server.InputModels
{
    public class ActionInputModel
    {
        public Guid GameId { get; set; }
        public PLAYER_ID PlayerId { get; set; }
        public int SerializedAction { get; set; }
    }
}
