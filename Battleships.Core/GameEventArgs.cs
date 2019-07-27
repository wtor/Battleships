using System;

namespace Battleships.Core
{
    public sealed class GameStartedEventArgs : EventArgs
    {
        public BoardContext Board { get; }

        public GameStartedEventArgs(BoardContext board)
        {
            Board = board;
        }
    }

    public sealed class GameFinishedEventArgs : EventArgs
    {
        private readonly Action restartGame;

        public ReadOnlyBoardContext Board { get; }

        public GameFinishedEventArgs(Action restartGame, ReadOnlyBoardContext board)
        {
            Board = board;
            this.restartGame = restartGame;
        }

        public void Restart()
        {
            restartGame();
        }
    }
}