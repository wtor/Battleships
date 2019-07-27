using System.Collections.Generic;
using Battleships.Core.Common;
using Battleships.Core.Ships;

namespace Battleships.Core
{
    public interface IField
    {
        Position Position { get; }

        void Accept(IFieldVisitor visitor);
    }

    public interface IFieldVisitor
    {
        void Print(IReadOnlyList<IField> fields);

        void Visit(EmptyField emptyField);

        void Visit(ShipField shipField);

        void Visit(MissedShotField missedField);
        
        void Visit(UnknownField unknownField);
        
        void Visit(ShootShipField shootShipField);
    }

    public abstract class BaseField : IField
    {
        public Position Position { get; }

        public abstract void Accept(IFieldVisitor visitor);

        protected BaseField(Position position)
        {
            Position = position;
        }
    }

    public sealed class EmptyField : BaseField
    {
        public EmptyField(Position position) : base(position)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public sealed class ShipField : BaseField
    {
        public string ShipName { get; }
        public char ShipSymbol { get; }

        public ShipField(Position position, IShip ship) : base(position)
        {
            ShipName = ship.Name;
            ShipSymbol = ship.Symbol;
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public sealed class MissedShotField : BaseField
    {
        public MissedShotField(Position position) : base(position)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);;
        }
    }

    public sealed class UnknownField : BaseField
    {
        public UnknownField(Position position) : base(position)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public sealed class ShootShipField : BaseField 
    {
        public string ShipName { get; }
        public char ShipSymbol { get; }

        public ShootShipField(Position position, IShip shipToShot) : base(position)
        {
            ShipName = shipToShot.Name;
            ShipSymbol = shipToShot.Symbol;
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}