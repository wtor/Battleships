using Battleships.Core.Common;

namespace Battleships.UI.ConsoleUI.Infrastructure
{
    public class UiPosition
    {
        public Position Position { get; }

        public UiPosition(Position position)
        {
            Position = position;
        }

        public static bool TryParse(string input, out UiPosition uiPosition)
        {
            input = input.Trim();

            if (input.Length != 2 && input.Length != 3)
            {
                uiPosition = null;
                return false;
            }

            var letter = char.ToUpper(input[0]);
            var stringNumber = input.Substring(1, input.Length - 1);

            if ('A' <= letter && letter <= 'J' && uint.TryParse(stringNumber, out var number) && 1 <= number && number <= 10)
            {
                var letterValue = (uint) (letter - 'A');

                var position = new Position(number - 1, letterValue);
                uiPosition = new UiPosition(position);
                return true;
            }

            uiPosition = null;
            return false;
        }

        public override string ToString()
        {
            var letter = char.ConvertFromUtf32((int) ('A' + Position.Y));
            var number = Position.X + 1;

            return $"{letter}{number}";
        }
    }
}