using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

namespace AvaloniaTetris;

internal partial class Game : ObservableObject
{
    [ObservableProperty]
    private bool _isActive;

    private Timer? timer;

    private readonly ObservableCollection<Piece> Pieces = [];

    private Piece? activePiece;

    private void Loop()
    {
        if (activePiece == null)
        {
            AddNewPiece();
        }

        if (CanMovePieceDown())
        {
            activePiece.MoveDown();
        }
        else
        {
            activePiece.IsActive = false;
            AddNewPiece();
        }
    }

    private bool CanMovePieceDown()
    {
        // Piece has reached the end
        if (activePiece.Y == 0)
        {
            return false;
        }

        // Check if there is a conflict
        var coords = activePiece.GetUsedCoords(0,-1);

        var existing = Pieces.Where(x => !x.IsActive).SelectMany(x => x.GetUsedCoords());

        return !coords.Any(x => existing.Contains(x));
    }

    Random randomPiece = new();

    private void AddNewPiece()
    {
        // Pick random Piece
        Piece? newPiece;
        switch (randomPiece.Next(1, 6))
        {
            case 1:
                newPiece = new Straight();
                break;
            case 2:
                newPiece = new Square();
                break;
            case 3:
                newPiece = new T();
                break;
            case 4:
                newPiece = new L();
                break;
            case 5:
                newPiece = new S();
                break;
            default:
                throw new Exception();
        }

        newPiece.IsActive = true;
        Pieces.Add(newPiece);
        activePiece = newPiece;
    }


    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        Loop();
    }

    // Public

    public void Start()
    {
        if (timer == null)
        {
            timer = new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
    }

    public Piece? GetAtCoords(int x, int y)
    {
        foreach (var piece in Pieces.ToList())
        {
            if (piece.IsOnCoord(x, y))
            {
                return piece;
            }
        }

        return null;
    }

    // Game Controls

    public void MoveLeft()
    {
        // check if there is a conflict

        if (activePiece?.X > 0)
        {
            activePiece.X--;
        }
    }

    public void MoveRight()
    {
        // check if there is a conflict

        if (activePiece?.X < 9)
        {
            activePiece.X++;
        }
    }

    public void MoveDown() { }

    public void Rotate() {
        // check if there is a conflict
    }

    public void Pause()
    {
        timer?.Stop();
    }
}
