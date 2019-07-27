using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.AttributeFilters;
using Battleships.Core.Common;
using Battleships.Core.Infrastructure;
using Battleships.Core.Services;
using Battleships.Core.Ships;

namespace Battleships.Core
{
    public interface IGame
    {
        void Start();
    }

    public sealed class Game : IGame
    {
        private const int BattleshipsCount = 1;
        private const int DestroyersCount = 2;
        private const int BoardDimension = 10;

        private readonly Size boardSize;

        private readonly IShipGenerationService shipGenerationService;
        private readonly Func<Size, IReadOnlyList<IShip>, IBoard> boardFactory;
        private readonly Func<IShipFactory> battleshipFactory;
        private readonly Func<IShipFactory> destroyerFactory;
        private readonly IUiHandler uiHandler;
        private IBoard board;

        public event EventHandler<GameStartedEventArgs> GameStarted;
        public event EventHandler<GameFinishedEventArgs> GameFinished;

        public Game(
            Func<Size, IShipGenerationService> shipGenerationServiceFactory,
            Func<Size, IReadOnlyList<IShip>, IBoard> boardFactory,
            [KeyFilter(ShipFactoryType.Battleship)] Func<IShipFactory> battleshipFactory,
            [KeyFilter(ShipFactoryType.Destroyer)] Func<IShipFactory> destroyerFactory,
            IUiHandler uiHandler
            )
        {
            this.boardSize = new Size(BoardDimension, BoardDimension);
            
            this.shipGenerationService = shipGenerationServiceFactory(boardSize);
            this.boardFactory = boardFactory;
            this.battleshipFactory = battleshipFactory;
            this.destroyerFactory = destroyerFactory;
            this.uiHandler = uiHandler;

            this.GameStarted += uiHandler.OnGameStarted;
            this.GameFinished += uiHandler.OnGameFinished;

            this.board = GetNewBoard();
        }

        public void Start()
        {
            GameStarted?.Invoke(this, new GameStartedEventArgs(new BoardContext(() => board)));
        }

        private IBoard GetNewBoard()
        {
            var newBoard = boardFactory(boardSize, BuildShips());

            newBoard.AccurateShot += uiHandler.OnAccurateShot;
            newBoard.ShipSunk += uiHandler.OnShipSunk;
            newBoard.MissedShot += uiHandler.OnMissedShot;
            newBoard.RepeatedShot += uiHandler.OnRepeatedShot;
            newBoard.AllShipsSunk += uiHandler.OnAllShipsSunk;
            newBoard.AllShipsSunk += OnAllShipsSunk;

            return newBoard;
        }

        private void DetachBoardHandlers()
        {
            board.AccurateShot -= uiHandler.OnAccurateShot;
            board.ShipSunk -= uiHandler.OnShipSunk;
            board.MissedShot -= uiHandler.OnMissedShot;
            board.RepeatedShot -= uiHandler.OnRepeatedShot;
            board.AllShipsSunk -= uiHandler.OnAllShipsSunk;
            board.AllShipsSunk -= OnAllShipsSunk;
        }

        private void OnAllShipsSunk(object sender, AllShipsSunkEventArgs e)
        {
            GameFinished?.Invoke(this, new GameFinishedEventArgs(RestartGame, new ReadOnlyBoardContext(() => board)));
        }

        private IReadOnlyList<IShip> BuildShips()
        {
            var battleShips = shipGenerationService.GenerateShips(battleshipFactory(), BattleshipsCount, Array.Empty<IShip>());
            var destroyers = shipGenerationService.GenerateShips(destroyerFactory(), DestroyersCount, battleShips);

            return battleShips.Concat(destroyers).ToList();
        }

        private void RestartGame()
        {
            DetachBoardHandlers();
            board = GetNewBoard();
        }
    }
}