using System.Collections.Generic;
namespace AvaloniaTetris.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IEnumerable<GridPoint> MainGridPoints => ReverseFlattenGrid(Game.Positions);
    public IEnumerable<GridPoint> NextPieceGridPints => ReverseFlattenGrid(Game.NextPiecePositions);


    public Game Game { get; set; } = new Game();

    public MainViewModel()
    {

        Game.Start();
    }


    private IEnumerable<GridPoint> ReverseFlattenGrid(GridPoint[,] grid)
    {
        for (int y = grid.GetLength(1) - 1; y >= 0; y--)
        //for (int y = 0; y <grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                yield return grid[x, y];
            }
        }
    }
}
