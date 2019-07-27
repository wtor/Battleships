using Autofac;
using Autofac.Features.AttributeFilters;
using Battleships.Core.Services;
using Battleships.Core.Ships;

namespace Battleships.Core.Infrastructure
{
    public sealed class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RandomNumberGenerator>().As<IRandomNumberGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<RandomPositionGenerator>().As<IRandomPositionGenerator>().InstancePerLifetimeScope();

            builder.RegisterType<ShipGenerationService>().As<IShipGenerationService>();

            builder.RegisterType<BattleshipFactory>().Keyed<IShipFactory>(ShipFactoryType.Battleship);
            builder.RegisterType<DestroyerFactory>().Keyed<IShipFactory>(ShipFactoryType.Destroyer);

            builder.RegisterType<Board>().As<IBoard>();

            builder.RegisterType<Game>().As<IGame>().WithAttributeFiltering();
        }
    }

    internal enum ShipFactoryType
    {
        Destroyer,
        Battleship
    }
}