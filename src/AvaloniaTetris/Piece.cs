﻿using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AvaloniaTetris;

public abstract partial class Piece : ObservableObject
{
    [ObservableProperty]
    bool _isActive;

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

    public void Rotate()
    {
        if (IsActive)
        {
            Shape = RotateMatrixCounterClockwise(Shape);
        }
    }
}

internal class Straight : Piece
{
    public Straight()
    {
        Shape = new int[,] { { 1, 1, 1, 1 } };
        X = 3;
        Y = 20;
    }
}

internal class Square : Piece
{
    public Square()
    {
        Shape = new int[,] { { 1, 1 },
                            { 1, 1 }};
        X = 4;
        Y = 21;
    }
}

internal class T : Piece
{
    public T()
    {
        Shape = new int[,] { { 1, 1, 1 },
                            { 0, 1, 0 }};
        X = 3;
        Y = 21;
    }
}

internal class L : Piece
{
    public L()
    {
        Shape = new int[,] { { 1, 1, 1 },
                            { 0, 0, 1 }};
        X = 4;
        Y = 21;
    }
}

internal class S : Piece
{
    public S()
    {
        Shape = new int[,] { { 1, 1, 0 },
                            { 0, 1, 1 }};
        X = 4;
        Y = 21;
    }
}
