using System.Collections.Generic;

namespace AvaloniaTetris.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IEnumerable<GridPoint> OrderedPoints => OrderPoints();
    public IEnumerable<GridPoint> OrderedNextPoints => OrderNextPoints();

    public Game Game { get; set; } = new Game();

    public MainViewModel()
    {
        OrderPoints();

        Game.Start();
    }

    private IEnumerable<GridPoint> OrderPoints()
    {
        for (int y = 19; y >= 0; y--)
        {
            for (int x = 0; x <= 9; x++)
            {
                yield return Game.Positions[x, y];
            }
        }
    }

    private IEnumerable<GridPoint> OrderNextPoints()
    {
        for (int y = 1; y >= 0; y--)
        {
            for (int x = 0; x <= 3; x++)
            {
                yield return Game.NextPiecePositions[x, y];
            }
        }
    }
}
