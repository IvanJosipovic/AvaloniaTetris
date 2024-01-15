using Avalonia;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTetris;

public partial class Game : ObservableObject
{
    private Dictionary<Point, Piece> Points { get; set; } = [];

    [ObservableProperty]
    private int _level = 1;

    [ObservableProperty]
    private int _score = 0;

    [ObservableProperty]
    private int _lines = 0;

    [ObservableProperty]
    private bool _isActive = true;

    private DispatcherTimer? timer;

    private Piece? activePiece;

    private void Timer_Tick(object? sender, EventArgs e)
    {
        MoveDown();
    }

    private bool CanMovePieceDown()
    {
        // Get coords for one row down
        var coords = activePiece.GetUsedCoords(0,-1);

        // Piece has reached the end
        if (coords.Any(p => p.Y < 0))
        {
            return false;
        }

        // Check if there is a conflict
        return !coords.Any(Points.ContainsKey);
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
        newPiece.IsActive = true;
        activePiece = newPiece;
    }

    // Public

    public void Start()
    {
        if (timer == null)
        {
            AddNewPiece();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }

    public Piece? GetAtCoords(int x, int y)
    {
        var pt = new Point(x, y);
        if (Points.TryGetValue(pt, out var value))
        {
            return value;
        }

        if (activePiece?.GetUsedCoords().Contains(pt) == true)
        {
            return activePiece;
        }

        return null;
    }

    // Game Controls

    public void MoveLeft()
    {
        var coords = activePiece?.GetUsedCoords(-1, 0);

        // Check for out of bounds
        if (coords.Any(point => point.X < 0))
        {
            return;
        }

        // Check if peice has a conflict
        if (coords.Any(x => Points.ContainsKey(x)))
        {
            return;
        }

        activePiece.MoveLeft();

    }

    public void MoveRight()
    {
        var coords = activePiece?.GetUsedCoords(1,0);

        // Check for out of bounds
        if (coords.Any(point => point.X > 9))
        {
            return;
        }

        // Check if piece has a conflict
        if (coords.Any(x => Points.ContainsKey(x)))
        {
            return;
        }

        activePiece.MoveRight();
    }

    public void MoveDown()
    {
        if (CanMovePieceDown())
        {
            activePiece.MoveDown();
        }
        else
        {
            activePiece.IsActive = false;

            // Check if piece failed to go to first row
            // Means end game
            if (activePiece.Y > 19)
            {
                IsActive = false;
                timer?.Stop();
                return;
            }

            foreach (var point in activePiece.GetUsedCoords())
            {
                Points.Add(point, activePiece);
            }

            AddNewPiece();
            MoveDown();
        }
    }

    public void Rotate() {
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

        if (coords.Any(point => point.Y > 20))
        {
            return;
        }

        if (coords.Any(point => point.Y < 0))
        {
            return;
        }

        // Check if piece has a conflict
        if (coords.Any(x => Points.ContainsKey(x)))
        {
            return;
        }

        activePiece.Rotate();
    }

    public void Pause()
    {
        if (timer?.IsEnabled == true)
        {
            timer?.Stop();
        }
        else
        {
            timer?.Start();
        }
    }

    public void Restart()
    {
        Level = 1;
        Score = 0;
        Lines = 0;

        Points.Clear();

        AddNewPiece();
    }
}
