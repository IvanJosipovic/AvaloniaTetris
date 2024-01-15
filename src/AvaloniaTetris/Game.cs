using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Timers;

namespace AvaloniaTetris
{
    internal partial class Game : ObservableObject
    {
        //private int[,] board = new int[10, 20];

        private Timer? timer;

        public readonly ObservableCollection<Piece> Pieces = [];

        private Piece? activePiece;

        private void Loop()
        {
            if (activePiece == null)
            {
                AddNewPiece();
            }
            else if (!MovePieceDown())
            {
                activePiece.IsActive = false;
                activePiece = null;
            }
        }

        private bool MovePieceDown()
        {
            if (activePiece.Y == 20)
            {
                return false;
            }

            // Check if there is a conflict


            activePiece.Y++;

            return true;
        }

        private void AddNewPiece()
        {
            // Pick random Piece
            activePiece = new Straight();
            Pieces.Add(activePiece);
        }

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

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Loop();
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

            if (activePiece?.X < 20)
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
}
