using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaTetris;

public partial class Game : ObservableObject
{

    public static int NextPieceGridHorizontalDimension { get; set; } = 4;
    public static int NextPieceVerticalDimension { get; set; } = 2;

    public static int MainGridHorizontalDimension { get; set; } = 12;
    public static int MainGridVerticalDimension { get; set; } = 30;
    public Game()
    {

        for (int y = 0; y < MainGridVerticalDimension; y++)
        {
            for (int x = 0; x < MainGridHorizontalDimension; x++)
            {
                Positions[x, y] = new GridPoint();
            }
        }

        for (int y = 0; y < NextPieceVerticalDimension; y++)
        {
            for (int x = 0; x < NextPieceGridHorizontalDimension; x++)
            {
                NextPiecePositions[x, y] = new GridPoint();
            }
        }
    }

    [ObservableProperty]
    private GridPoint[,] _positions = new GridPoint[MainGridHorizontalDimension, MainGridVerticalDimension];

    [ObservableProperty]
    private GridPoint[,] _nextPiecePositions = new GridPoint[NextPieceGridHorizontalDimension, NextPieceVerticalDimension];


    [ObservableProperty]
    private int _level;

    [ObservableProperty]
    private int _score;

    [ObservableProperty]
    private int _lines;

    [ObservableProperty]
    private bool _isActive = true;

    [ObservableProperty]
    private int _speed;

    [ObservableProperty]
    private bool _showGrid;



    private readonly object _lock = new();

    private DispatcherTimer? timer;

    private Piece? activePiece;

    List<int> rowsToRemove = new List<int>();
    
    int blinkStep = 0;

    private const int maxBlinks = 5;

    readonly Random randomPieceGenerator = new();

    private Queue<Piece> pieces = new Queue<Piece>();




    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (rowsToRemove.Count > 0)
            DoBlinkRemovingLines();
        else
            MoveDown();
    }



    private void AddNewPiece()
    {
        while (pieces.Count < 2)
        {
            int p = 0;
            p = randomPieceGenerator.Next(0, 7); // Pick random Piece, comment for fast test with one piece

            Piece? newPiece = p switch
            {
                0 => new Straight(MainGridHorizontalDimension / 2 - 2, MainGridVerticalDimension - 1),
                1 => new Square(MainGridHorizontalDimension / 2 - 1, MainGridVerticalDimension - 2),
                2 => new T(MainGridHorizontalDimension / 2 - 1, MainGridVerticalDimension - 2),
                3 => new L1(MainGridHorizontalDimension / 2 - 1, MainGridVerticalDimension - 2),
                4 => new L2(MainGridHorizontalDimension / 2 - 1, MainGridVerticalDimension - 2),
                5 => new S2(MainGridHorizontalDimension / 2 - 1, MainGridVerticalDimension - 2),
                6 => new S1(MainGridHorizontalDimension / 2 - 1, MainGridVerticalDimension - 2),

                _ => throw new Exception(),
            };
            pieces.Enqueue(newPiece);
        }
        activePiece = pieces.Dequeue();
    }

    private void ClearActivePiece()
    {

        foreach (var point in activePiece?.GetUsedCoords())
        {
            var pos = Positions[point.X, point.Y];
            pos.FillType = GridFill.Blank;
            pos.IsActive = false;
        }

    }

    private void SetActivePiece(bool isActive = true)
    {

        foreach (var point in activePiece?.GetUsedCoords())
        {
            var pos = Positions[point.X, point.Y];
            pos.FillType = activePiece.FillType;
            pos.IsActive = isActive;
        }

        var nextPiece = pieces.Peek();
        for (int y = 0; y < NextPieceVerticalDimension; y++)
        {
            for (int x = 0; x < NextPieceGridHorizontalDimension; x++)
            {
                NextPiecePositions[x, y].FillType = 0;
            }
        }
        foreach (var point in nextPiece.GetUsedCoords(-nextPiece.X, -nextPiece.Y))
        {
            var pos = NextPiecePositions[point.X, point.Y];
            pos.FillType = nextPiece.FillType;
        }
    }



    private void CheckRowsToRemove()
    {
        rowsToRemove.Clear();
        for (int y = 0; y < MainGridVerticalDimension; y++)
        {
            bool hasToBeRemoved = true;
            for (int x = 0; x < MainGridHorizontalDimension; x++)
            {
                var pos = Positions[x, y];
                hasToBeRemoved &= !((pos.FillType == GridFill.Blank) && !pos.IsActive);
            }
            if (hasToBeRemoved)
            {
                rowsToRemove.Add(y);
            }
        }
        if (rowsToRemove.Count > 0)
            timer.Interval = TimeSpan.FromMilliseconds(60); // Time of Blinking
    }


    private void DoBlinkRemovingLines()
    {
        if (blinkStep < maxBlinks)
        {

            var fc = blinkStep % 2 == 0 ? GridFill.BlinkOn : GridFill.BlinkOff;
            foreach (var row in rowsToRemove)
            {
                for (int x = 0; x < MainGridHorizontalDimension; x++)
                {
                    Positions[x, row].FillType = fc;


                }
            }
            blinkStep++;

        }
        else
        {

            //Remove rows and push down others
            foreach (var row in rowsToRemove.OrderDescending())
            {
                for (int yy = row + 1; yy < MainGridVerticalDimension; yy++)
                {
                    for (int x = 0; x < MainGridHorizontalDimension; x++)
                    {
                        var old = Positions[x, yy - 1];

                        var newOb = Positions[x, yy];

                        if (!old.IsActive && !newOb.IsActive)
                        {
                            old.FillType = newOb.FillType;
                            old.IsActive = false;
                        }
                    }
                }
            }
            Lines += rowsToRemove.Count();
            Level = Lines / 10;
            Score += MainGridHorizontalDimension * (Level + 1);
            SetGameSpeed();
            rowsToRemove.Clear();
            blinkStep = 0;

        }
    }


    private void SetGameSpeed()
    {


        timer.Interval = Level switch
        {
            0 => TimeSpan.FromMilliseconds(800),
            1 => TimeSpan.FromMilliseconds(716),
            2 => TimeSpan.FromMilliseconds(633.33),
            3 => TimeSpan.FromMilliseconds(550),
            4 => TimeSpan.FromMilliseconds(466.66),
            5 => TimeSpan.FromMilliseconds(383.33),
            6 => TimeSpan.FromMilliseconds(300),
            7 => TimeSpan.FromMilliseconds(216.66),
            8 => TimeSpan.FromMilliseconds(133.33),
            9 => TimeSpan.FromMilliseconds(100),
            10 or 11 or 12 => TimeSpan.FromMilliseconds(85),
            13 or 14 or 15 => TimeSpan.FromMilliseconds(65),
            16 or 17 or 18 => TimeSpan.FromMilliseconds(50),
            19 or 20 or 21 or 22 or 23 or 24 or 25 or 26 or 27 or 28 => TimeSpan.FromMilliseconds(33.3),
            _ => TimeSpan.FromMilliseconds(16.67),
        };

        Speed = (int)timer.Interval.TotalMilliseconds;
    }

    // Public

    public void Start()
    {
        if (timer == null)
        {
            AddNewPiece();
            SetActivePiece();

            timer = new DispatcherTimer();
            SetGameSpeed();
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }

    // Game Controls

    [RelayCommand]
    void MoveLeft()
    {
        if (!IsActive) { return; }

        lock (_lock)
        {
            var coords = activePiece?.GetUsedCoords(-1, 0);

            // Check for out of bounds
            if (coords.Any(point => point.X < 0))
            {
                return;
            }

            // Check if piece has a conflict
            if (coords.Any(pt => Positions[pt.X, pt.Y].FillType != GridFill.Blank && !Positions[pt.X, pt.Y].IsActive))
            {
                return;
            }

            ClearActivePiece();
            activePiece?.MoveLeft();
            SetActivePiece();
        }
    }

    [RelayCommand]
    void MoveRight()
    {
        if (!IsActive) { return; }

        lock (_lock)
        {
            var coords = activePiece?.GetUsedCoords(1, 0);

            // Check for out of bounds
            if (coords.Any(point => point.X >= MainGridHorizontalDimension))
            {
                return;
            }

            // Check if piece has a conflict
            if (coords.Any(pt => Positions[pt.X, pt.Y].FillType != GridFill.Blank && !Positions[pt.X, pt.Y].IsActive))
            {
                return;
            }

            ClearActivePiece();
            activePiece?.MoveRight();
            SetActivePiece();
        }
    }

    [RelayCommand]
    void MoveDown()
    {
        if (!IsActive) { return; }

        lock (_lock)
        {
            // Get coords for one row down
            var coords = activePiece?.GetUsedCoords(0, -1);

            // Check if we have reached the end or there is a conflict
            if (!coords.Any(p => p.Y < 0) && !coords.Any(pt => Positions[pt.X, pt.Y].FillType != GridFill.Blank && !Positions[pt.X, pt.Y].IsActive))
            {
                ClearActivePiece();
                activePiece?.MoveDown();
                SetActivePiece();
            }
            else
            {
                int offSet = activePiece?.Shape.GetLength(1) > 0 ? 2 : 1;
                // Check if piece failed to go to first row
                // Means end game
                if (activePiece?.Y == MainGridVerticalDimension - offSet)
                {
                    IsActive = false;
                    timer?.Stop();
                    return;
                }

                SetActivePiece(false);

                AddNewPiece();
                SetActivePiece();
                CheckRowsToRemove();
            }

        }
    }

    [RelayCommand]
    void Rotate()
    {
        if (!IsActive) { return; }

        lock (_lock)
        {
            var coords = activePiece?.GetUsedCoords(0, 0, true);

            // Check for out of bounds
            if (coords.Any(point => point.X < 0))
            {
                return;
            }

            if (coords.Any(point => point.X > MainGridHorizontalDimension - 1))
            {
                return;
            }

            if (coords.Any(point => point.Y > MainGridVerticalDimension - 1))
            {
                return;
            }

            if (coords.Any(point => point.Y < 0))
            {
                return;
            }

            // Check if piece has a conflict
            if (coords.Any(pt => Positions[pt.X, pt.Y].FillType != GridFill.Blank && !Positions[pt.X, pt.Y].IsActive))
            {
                return;
            }

            ClearActivePiece();
            activePiece?.Rotate();
            SetActivePiece();
        }
    }

    [RelayCommand]
    void Pause()
    {
        if (timer?.IsEnabled == true)
        {
            IsActive = false;
            timer?.Stop();
        }
        else
        {
            IsActive = true;
            timer?.Start();
        }
    }

    [RelayCommand]
    void Restart()
    {
        Level = 0;
        Score = 0;
        Lines = 0;

        IsActive = true;

        timer?.Start();

        for (int y = 0; y < MainGridVerticalDimension; y++)
        {
            for (int x = 0; x < MainGridHorizontalDimension; x++)
            {
                var pos = Positions[x, y];
                pos.FillType = 0;
                pos.IsActive = false;
            }
        }

        AddNewPiece();
        SetActivePiece();
    }
}
