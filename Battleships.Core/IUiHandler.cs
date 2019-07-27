namespace Battleships.Core
{
    public interface IUiHandler
    {
        void OnGameStarted(object sender, GameStartedEventArgs e);
        void OnGameFinished(object sender, GameFinishedEventArgs e);

        void OnAccurateShot(object sender, AccurateShotEventArgs e);
        void OnMissedShot(object sender, MissedShotEventArgs e);
        void OnRepeatedShot(object sender, RepeatedShotEventArgs e);
        void OnShipSunk(object sender, ShipSunkEventArgs e);
        void OnAllShipsSunk(object sender, AllShipsSunkEventArgs e);
    }
}