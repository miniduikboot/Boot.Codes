using Impostor.Api.Events;
using Impostor.Api.Games;
using Microsoft.Extensions.Logging;
using System;

namespace Boot.Codes.Handlers
{
    public class GameEventListener : IEventListener
    {
        private readonly IGameCodeManager _gameCodeManager;

        public GameEventListener(IGameCodeManager gameCodeManager)
        {
            this._gameCodeManager = gameCodeManager;
        }

        [EventListener(EventPriority.Highest)]
        public void OnGameCreated(IGameCreationEvent e) => e.GameCode = _gameCodeManager.Get();

        [EventListener(EventPriority.Highest)]
        public void OnGameDestroyed(IGameDestroyedEvent e) => _gameCodeManager.Release(e.Game.Code);
    }
}
