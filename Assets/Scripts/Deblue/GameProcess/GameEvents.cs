namespace Deblue.GameProcess
{
    public readonly struct GameStart
    {
    }

    public readonly struct GameOver
    {
    }

    public readonly struct NewGame
    {
    }

    public readonly struct NewGameSession
    {
    }

    public readonly struct EndGameSession
    {
    }
    
    public readonly struct GamePauseChange
    {
        public readonly bool IsPaused;

        public GamePauseChange(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}