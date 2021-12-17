using Microsoft.Extensions.Caching.Memory;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class GameCommandService<TGame> where TGame : IGame
    {

        private readonly IMemoryCache _memoryCache;
        public GameCommandService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddGame(TGame game)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(new TimeSpan(TimeSpan.TicksPerHour));

            _memoryCache.Set(game.GameId, game, cacheEntryOptions);
        }
    }
}
