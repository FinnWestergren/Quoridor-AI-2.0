using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game
{
    public class InvalidBoardException : Exception
    {
        public InvalidBoardException(string error) : base($"Invalid Quoridor Board String: {error}") { }
    }
}
