using Impostor.Api.Events;
using Impostor.Api.Games;
using Microsoft.Extensions.Logging;
using System;

namespace Boot.Codes.Handlers
{
    public class GameEventListener : IEventListener
    {
        private readonly ILogger<GameEventListener> _logger;

        private static string[] names = {
            // TODO: add your own names here
        };

        private Random rng = new Random();

        public GameEventListener(ILogger<GameEventListener> logger)
        {
            _logger = logger;
        }

        [EventListener]
        public void OnGameCreated(IGameCreationEvent e)
        {
            // TODO: detect if game code is in use and retry until the dict is exhausted
            var index = rng.Next(0, names.Length);
            var gameCode = GameCode.From(names[index]);
            _logger.LogInformation("Setting room code to {gameCode}", gameCode);
            if (!gameCode.IsInvalid)
            {
                _logger.LogInformation("Code was valid");
                e.GameCode = gameCode;
            }
        }
    }
}
