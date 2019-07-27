using System;
using Autofac;
using Battleships.Core;
using Battleships.Core.Infrastructure;
using Battleships.UI.ConsoleUI.Infrastructure;

namespace Battleships.UI.ConsoleUI
{
    internal static class Program
    {
        private static void Main()
        {
            using (var container = GetContainer())
            {
                var printer = new ConsoleBoardPrinter();
                var consoleHandler = new ConsoleHandler(printer);
                var game = container.Resolve<Func<IUiHandler, IGame>>()(consoleHandler);

                game.Start();
            }
        }

        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<CoreModule>();

            return builder.Build();
        }
    }
}