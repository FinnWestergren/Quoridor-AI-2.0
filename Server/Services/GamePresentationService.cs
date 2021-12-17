using Microsoft.Extensions.Caching.Memory;
using Server.Game;
using Server.Game.TicTacToe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class GamePresentationService<TGame> where TGame : IGame
    {

        private readonly IMemoryCache _memoryCache;

        public GamePresentationService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TGame GetGame(Guid gameId)
        {
            _memoryCache.TryGetValue(gameId, out TGame game);

            if(game == null)
            {
                throw new Exception("Game does not exist!");
            }

            return game;
        }
    }
}
