using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AvaloniaTetris;

public abstract partial class Piece : ObservableObject
{
    [ObservableProperty]
    int[,] _shape;

    [ObservableProperty]
    int _x;

    [ObservableProperty]
    int _y;

    public List<Point> GetUsedCoords(double xOffset = 0, double yOffset = 0, bool rotate = false)
    {
        List<Point> coords = [];

        var newShape = Shape;

        if (rotate)
        {
            newShape = RotateMatrixCounterClockwise(Shape);
        }

        int rows = newShape.GetLength(0);
        int columns = newShape.GetLength(1);

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                int currentValue = newShape[x, y];

                if (currentValue == 1)
                {
                    coords.Add(new Point(X + y + xOffset, Y + x + yOffset));
                }
            }
        }

        return coords;
    }

    static int[,] RotateMatrixCounterClockwise(int[,] oldMatrix)
    {
        int[,] newMatrix = new int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }

    public void MoveDown()
    {
        Y--;
    }

    public void MoveLeft()
    {
        X--;
    }

    public void MoveRight()
    {
        X++;
    }

    public void Rotate()
    {
        Shape = RotateMatrixCounterClockwise(Shape);
    }
}

internal class Straight : Piece
{
    public Straight()
    {
        Shape = new int[,] { { 1, 1, 1, 1 } };
        X = 3;
        Y = 19;
    }
}

internal class Square : Piece
{
    public Square()
    {
        Shape = new int[,] { { 1, 1 },
                             { 1, 1 }};
        X = 4;
        Y = 18;
    }
}

internal class T : Piece
{
    public T()
    {
        Shape = new int[,] { { 1, 1, 1 },
                             { 0, 1, 0 }};
        X = 3;
        Y = 18;
    }
}

internal class L1 : Piece
{
    public L1()
    {
        Shape = new int[,] { { 1, 1, 1 },
                             { 0, 0, 1 }};
        X = 4;
        Y = 18;
    }
}
internal class L2 : Piece
{
    public L2()
    {
        Shape = new int[,] { { 0, 0, 1 },
                             { 1, 1, 1 }};
        X = 4;
        Y = 18;
    }
}


internal class S1 : Piece
{
    public S1()
    {
        Shape = new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 }
        };
        X = 4;
        Y = 18;
    }
}

internal class S2 : Piece
{
    public S2()
    {
        Shape = new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 }
        };
        X = 4;
        Y = 18;
    }
}
