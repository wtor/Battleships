using System;
using Battleships.Core;

namespace Battleships.UI.ConsoleUI.Infrastructure
{
    public class ConsoleHandler : IUiHandler
    {
        private readonly IFieldVisitor printer;

        public ConsoleHandler(IFieldVisitor printer)
        {
            this.printer = printer;
        }

        public void OnGameStarted(object sender, GameStartedEventArgs e)
        {
            Console.WriteLine("Welcome to BATTLESHIP!");
            Console.WriteLine("Are you ready to begin? Press any key to start.");

            Console.ReadKey();

            Console.Clear();
            
            e.Board.Print(printer);
            
            while (true)
            {
                Console.WriteLine("\nEnter position of a shot, eg. B3.");
                WritePrompt();

                var positionString = Console.ReadLine();

                if (!UiPosition.TryParse(positionString, out var uiPosition))
                {
                    HandleInvalidInput(e.Board);

                    continue;
                }

                e.Board.Shot(uiPosition.Position);
            }
        }

        private void HandleInvalidInput(BoardContext board)
        {
            Console.Clear();

            board.Print(printer);

            Console.WriteLine("Enter a valid position.");
        }

        public void OnGameFinished(object sender, GameFinishedEventArgs e)
        {
            Console.WriteLine("Game finished! Would you like to play again?");
            Console.WriteLine("Press [Y] key to play again or any other key to exit.");
            WritePrompt();

            var keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Y)
            {
                e.Restart();

                RefreshConsole(e.Board);

                return;
            }

            Environment.Exit(0);
        }

        public void OnAllShipsSunk(object sender, AllShipsSunkEventArgs e)
        {
            Console.WriteLine($"Congratulation! You've won the Battleship with {e.ShotsCount} shots!");
        }

        public void OnAccurateShot(object sender, AccurateShotEventArgs e)
        {
            RefreshConsole(e.Board);

            Console.WriteLine($"{e.ShipName} has been shot! Great work!");
        }

        public void OnMissedShot(object sender, MissedShotEventArgs e)
        {
            RefreshConsole(e.Board);

            Console.WriteLine("You've missed!");
        }

        public void OnRepeatedShot(object sender, RepeatedShotEventArgs e)
        {
            RefreshConsole(e.Board);

            Console.WriteLine($"You've already hit field {new UiPosition(e.Shot.Position)}!");
        }

        public void OnShipSunk(object sender, ShipSunkEventArgs e)
        {
            RefreshConsole(e.Board);

            Console.WriteLine($"{e.ShipName} has been sunk by your shot! Great!");
        }

        private static void WritePrompt()
        {
            Console.Write("> ");
        }

        private void RefreshConsole(ReadOnlyBoardContext boardContext)
        {
            Console.Clear();
            
            boardContext.Print(printer);

            Console.WriteLine();
        }
    }
}