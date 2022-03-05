using System;

namespace Server.InputModels
{
    public class ActionInputModel
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int SerializedAction { get; set; }
    }
}
