using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;


namespace AvaloniaTetris;


public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
public enum GridFill
{
    Blank,
    AsStraight,
    AsS1,
    AsS2,
    AsL1,
    AsL2,
    AsSquare,
    AsT,
    BlinkOn,
    BlinkOff
}
public abstract partial class Piece : ObservableObject
{

    [ObservableProperty]
    int[,] _shape;

    [ObservableProperty]
    int _x;

    [ObservableProperty]
    int _y;


    public GridFill FillType { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Piece(int startX, int startY)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.X = startX;
        this.Y = startY;

    }

    public List<Point> GetUsedCoords(int xOffset = 0, int yOffset = 0, bool rotate = false)
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
    public Straight(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,] { { 1, 1, 1, 1 } };
        FillType = GridFill.AsStraight;
    }
}

internal class Square : Piece
{
    public Square(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,] { { 1, 1 },
                             { 1, 1 }};

        FillType = GridFill.AsSquare;
    }
}

internal class T : Piece
{
    public T(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,] { { 1, 1, 1 },
                             { 0, 1, 0 }};

        FillType = GridFill.AsT;
    }
}

internal class L1 : Piece
{
    public L1(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,] { { 1, 1, 1 },
                             { 0, 0, 1 }};
        FillType = GridFill.AsL1;

    }
}
internal class L2 : Piece
{
    public L2(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,] { { 0, 0, 1 },
                             { 1, 1, 1 }};

        FillType = GridFill.AsL2;
    }
}


internal class S1 : Piece
{
    public S1(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 }
        };
        FillType = GridFill.AsS1;

    }
}

internal class S2 : Piece
{
    public S2(int startX, int startY) : base(startX, startY)
    {
        Shape = new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 }
        };
        FillType = GridFill.AsS2;

    }
}

