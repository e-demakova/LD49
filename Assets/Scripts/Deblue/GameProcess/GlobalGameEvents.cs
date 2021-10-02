using System;
using System.Collections.Generic;
using Deblue.ObservingSystem;

namespace Deblue.GameProcess
{
    public interface IGlobalGameEvents
    {
        IReadOnlyHandler<GameOver> GameOver { get; }
        IReadOnlyHandler<NewGame> NewGame { get; }
        IReadOnlyHandler<NewGameSession> NewGameSession { get; }
        IReadOnlyHandler<EndGameSession> EndGameSession { get; }
    }

    public class GlobalGameEvents : IGlobalGameEvents
    {
        public Handler<GameOver> GameOverHandler = new Handler<GameOver>();
        public Handler<NewGame> NewGameHandler = new Handler<NewGame>();
        public Handler<NewGameSession> NewGameSessionHandler = new Handler<NewGameSession>();
        public Handler<EndGameSession> EndGameSessionHandler = new Handler<EndGameSession>();

        public IReadOnlyHandler<GameOver> GameOver => GameOverHandler;
        public IReadOnlyHandler<NewGame> NewGame => NewGameHandler;
        public IReadOnlyHandler<NewGameSession> NewGameSession => NewGameSessionHandler;
        public IReadOnlyHandler<EndGameSession> EndGameSession => EndGameSessionHandler;
    }
}