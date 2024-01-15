using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTetris;

internal abstract partial class Piece : ObservableObject
{
    [ObservableProperty]
    bool _isActive;

    [ObservableProperty]
    int[,] _size;

    [ObservableProperty]
    int _orientation;

    [ObservableProperty]
    IBrush _color;

    [ObservableProperty]
    int _x;

    [ObservableProperty]
    int _y;

    public bool IsOnCoord(int x, int y)
    {
        return GetUsedCoords().Contains($"{x},{y}");
    }

    public HashSet<string> GetUsedCoords(int xOffset = 0, int yOffset = 0)
    {
        HashSet<string> coords = [];

        int rows = Size.GetLength(0);
        int columns = Size.GetLength(1);

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                int currentValue = Size[x, y];

                if (currentValue == 1)
                {
                    coords.Add($"{X + y + xOffset},{Y + x + yOffset}");
                }
            }
        }

        return coords;
    }

    public void MoveDown()
    {
        if (IsActive)
        {
            Y--;
        }
    }

    public void MoveLeft()
    {
        if (IsActive)
        {
            X--;
        }
    }

    public void MoveRight()
    {
        if (IsActive)
        {
            X++;
        }
    }
}

internal class Straight : Piece
{
    public Straight()
    {
        Color = Brushes.Red;
        Size = new int[,] { { 1, 1, 1, 1 } };
        X = 3;
        Y = 20;
    }
}

internal class Square : Piece
{
    public Square()
    {
        Color = Brushes.Green;
        Size = new int[,] { { 1, 1 },
                            { 1, 1 }};
        X = 4;
        Y = 21;
    }
}

internal class T : Piece
{
    public T()
    {
        Color = Brushes.Yellow;
        Size = new int[,] { { 1, 1, 1 },
                            { 0, 1, 0 }};
        X = 3;
        Y = 21;
    }
}

internal class L : Piece
{
    public L()
    {
        Color = Brushes.Blue;
        Size = new int[,] { { 1, 1, 1 },
                            { 0, 0, 1 }};
        X = 4;
        Y = 21;
    }
}

internal class S : Piece
{
    public S()
    {
        Color = Brushes.Pink;
        Size = new int[,] { { 1, 1, 0 },
                            { 0, 1, 1 }};
        X = 4;
        Y = 21;
    }
}
