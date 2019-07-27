using System;
using System.Collections.Generic;
using Battleships.Core;
using Battleships.Core.Common;

namespace Battleships.UI.ConsoleUI.Infrastructure
{
    public sealed class ConsoleBoardPrinter : IFieldVisitor
    {
        private readonly string[] letters;
        private readonly Size size;

        public ConsoleBoardPrinter()
        {
            // For simplicity I assume that size won't be changed.
            size = new Size(10, 10);
            letters = new[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};
        }

        public void Print(IReadOnlyList<IField> fields)
        {
            PrintLetters();
            PrintHorizontalSeparator();

            for (var i = 0; i < fields.Count; i++)
            {
                if (i % size.Width == 0)
                {
                    WriteRowNumber(i / (int)size.Width);
                }

                fields[i].Accept(this);

                Console.Write("|");

                if (i != 0 && (i + 1) % size.Width == 0)
                {
                    Console.WriteLine();
                    PrintHorizontalSeparator();
                }
            }
        }

        private static void WriteRowNumber(int rowNumber)
        {
            var spaces = 1 - ((rowNumber + 1) / 10) ;

            Console.Write($"{new string(' ', spaces)}{rowNumber + 1}|");
        }

        private void PrintLetters()
        {
            Console.Write("  ");

            for (var i = 0; i < size.Width - 1; i++)
                Console.Write($"|{letters[i]}");

            Console.WriteLine($"|{letters[size.Width - 1]}|");
        }

        private void PrintHorizontalSeparator()
        {
            Console.Write("-");

            for (var i = 0; i <= size.Width; i++)
                Console.Write("-+");

            Console.WriteLine();
        }

        public void Visit(EmptyField emptyField)
        {
            Console.Write(" ");
        }

        public void Visit(ShipField shipField)
        {
            Console.Write(shipField.ShipSymbol);
        }

        public void Visit(MissedShotField missedField)
        {
            Console.Write("X");
        }

        public void Visit(UnknownField unknownField)
        {
            WriteWithColor("?", ConsoleColor.DarkGray);
        }

        public void Visit(ShootShipField shootShipField)
        {
            WriteWithColor(shootShipField.ShipSymbol.ToString(), ConsoleColor.Red);
        }

        private static void WriteWithColor(string text, ConsoleColor color)
        {
            var currentColor = Console.ForegroundColor;

            Console.ForegroundColor = color;

            Console.Write(text);

            Console.ForegroundColor = currentColor;
        }
    }
}