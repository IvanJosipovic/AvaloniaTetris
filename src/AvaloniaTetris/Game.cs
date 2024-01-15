using Avalonia;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

namespace AvaloniaTetris;

public partial class Game : ObservableObject
{
    private Dictionary<Point, Piece> Points { get; set; } = [];

    [ObservableProperty]
    private bool _isActive = true;

    private DispatcherTimer? timer;

    private Piece? activePiece;

    private void Loop()
    {
        if (activePiece == null)
        {
            AddNewPiece();
        }

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
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        Loop();
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
        if (activePiece?.X == 0)
        {
            return;
        }

        var coords = activePiece?.GetUsedCoords(-1, 0);

        if (coords.Any(x => Points.ContainsKey(x)))
        {
            return;
        }

        activePiece.X--;

    }

    public void MoveRight()
    {
        var coords = activePiece?.GetUsedCoords();

        if (coords.Any(point => point.X == 9))
        {
            return;
        }

        coords = activePiece?.GetUsedCoords(1, 0);

        if (coords.Any(x => Points.ContainsKey(x)))
        {
            return;
        }

        activePiece.X++;
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
            foreach (var point in activePiece.GetUsedCoords())
            {
                Points.Add(point, activePiece);
            }
            if (activePiece.Y > 19)
            {
                IsActive = false;
                timer?.Stop();
                return;
            }
            AddNewPiece();
        }
    }

    public void Rotate() {
        // check if there is a conflict
    }

    public void Pause()
    {
        timer?.Stop();
    }
}
