using System;
using Battleships.Core.Common;

namespace Battleships.Core
{
    public sealed class BoardContext
    {
        private readonly Func<IBoard> boardAccessor;

        public BoardContext(Func<IBoard> boardAccessor)
        {
            this.boardAccessor = boardAccessor;
        }

        public void Shot(Position position)
        {
            boardAccessor().Shot(new Shot(position));
        }

        public void Print(IFieldVisitor fieldVisitor)
        {
            fieldVisitor.Print(boardAccessor().GetPlayerVisibleFields());
        }
    }

    public sealed class ReadOnlyBoardContext
    {
        private readonly Func<IBoard> boardAccessor;

        public ReadOnlyBoardContext(Func<IBoard> boardAccessor)
        {
            this.boardAccessor = boardAccessor;
        }

        public ReadOnlyBoardContext(IBoard board) : this(() => board)
        {
        }

        public void Print(IFieldVisitor fieldVisitor)
        {
            fieldVisitor.Print(boardAccessor().GetPlayerVisibleFields());
        }
    }
}