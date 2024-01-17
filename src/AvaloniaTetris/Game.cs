using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;

namespace AvaloniaTetris;

public partial class Game : ObservableObject
{
    public Game()
    {
        for (int y = 0; y <= 19; y++)
        {
            for (int x = 0; x <= 9; x++)
            {
                Positions[x, y] = new GridPoint();
            }
        }
    }

    [ObservableProperty]
    private GridPoint[,] _positions = new GridPoint[10,20];

    [ObservableProperty]
    private int _level = 1;

    [ObservableProperty]
    private int _score = 0;

    [ObservableProperty]
    private int _lines = 0;

    [ObservableProperty]
    private bool _isActive = true;

    private readonly object _lock = new();

    private DispatcherTimer? timer;

    private Piece? activePiece;

    private void Timer_Tick(object? sender, EventArgs e)
    {
        MoveDown();
    }

    readonly Random randomPiece = new();

    private void AddNewPiece()
    {
        // Pick random Piece
        Piece? newPiece = randomPiece.Next(1, 6) switch
        {
            1 => new Straight(),
            2 => new Square(),
            3 => new T(),
            4 => new L(),
            5 => new S(),
            _ => throw new Exception(),
        };
        activePiece = newPiece;
    }

    private void ClearActivePiece()
    {
        foreach (var point in activePiece.GetUsedCoords())
        {
            var pos = Positions[(int)point.X, (int)point.Y];
            pos.Type = 0;
            pos.IsActive = false;
        }
    }

    private void SetActivePiece(bool isActive = true)
    {
        int shape = 0;
        if (activePiece is Straight)
        {
            shape = 1;
        }
        else if (activePiece is Square)
        {
            shape = 2;
        }
        else if (activePiece is S)
        {
            shape = 3;
        }
        else if (activePiece is T)
        {
            shape = 4;
        }
        else if (activePiece is L)
        {
            shape = 5;
        }

        foreach (var point in activePiece.GetUsedCoords())
        {
            var pos = Positions[(int)point.X, (int)point.Y];
            pos.Type = shape;
            pos.IsActive = isActive;
        }
    }

    private void RemoveFullRow()
    {
        //Check if a row needs to be removed
        for (int y = 0; y <= 19; y++)
        {
            Top:
            bool removeRow = true;

            for (int x = 0; x <= 9; x++)
            {
                var pos = Positions[x, y];

                if (pos.Type == 0 && !pos.IsActive)
                {
                    removeRow = false;
                    break;
                }
            }

            if (removeRow)
            {
                Score += 100;
                Lines++;

                for (int yy = y + 1; yy <= 19; yy++)
                {
                    for (int x = 0; x <= 9; x++)
                    {
                        var old = Positions[x, yy - 1];

                        var newOb = Positions[x, yy];

                        if (!old.IsActive && !newOb.IsActive)
                        {
                            old.Type = newOb.Type;
                            old.IsActive = false;
                        }
                    }
                }

                goto Top;
            }
        }
    }

    // Public

    public void Start()
    {
        if (timer == null)
        {
            AddNewPiece();
            SetActivePiece();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
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
            if (coords.Any(pt => Positions[(int)pt.X, (int)pt.Y].Type > 0 && !Positions[(int)pt.X, (int)pt.Y].IsActive))
            {
                return;
            }

            ClearActivePiece();
            activePiece.MoveLeft();
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
            if (coords.Any(point => point.X > 9))
            {
                return;
            }

            // Check if piece has a conflict
            if (coords.Any(pt => Positions[(int)pt.X, (int)pt.Y].Type > 0 && !Positions[(int)pt.X, (int)pt.Y].IsActive))
            {
                return;
            }

            ClearActivePiece();
            activePiece.MoveRight();
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
            var coords = activePiece.GetUsedCoords(0, -1);

            // Check if we have reached the end or there is a conflict
            if (!coords.Any(p => p.Y < 0) && !coords.Any(pt => Positions[(int)pt.X, (int)pt.Y].Type > 0 && !Positions[(int)pt.X, (int)pt.Y].IsActive))
            {
                ClearActivePiece();
                activePiece.MoveDown();
                SetActivePiece();
            }
            else
            {
                // Check if piece failed to go to first row
                // Means end game
                if (activePiece.Y == 19)
                {
                    IsActive = false;
                    timer?.Stop();
                    return;
                }

                SetActivePiece(false);

                AddNewPiece();
                SetActivePiece();
                RemoveFullRow();
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

            if (coords.Any(point => point.X > 9))
            {
                return;
            }

            if (coords.Any(point => point.Y > 19))
            {
                return;
            }

            if (coords.Any(point => point.Y < 0))
            {
                return;
            }

            // Check if piece has a conflict
            if (coords.Any(pt => Positions[(int)pt.X, (int)pt.Y].Type > 0 && !Positions[(int)pt.X, (int)pt.Y].IsActive))
            {
                return;
            }

            ClearActivePiece();
            activePiece.Rotate();
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
        Level = 1;
        Score = 0;
        Lines = 0;

        IsActive = true;

        timer?.Start();

        for (int y = 0; y <= 19; y++)
        {
            for (int x = 0; x <= 9; x++)
            {
                var pos = Positions[x, y];
                pos.Type = 0;
                pos.IsActive = false;
            }
        }

        AddNewPiece();
        SetActivePiece();
    }
}
