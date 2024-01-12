using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTetris
{
    internal partial class Game : ObservableObject
    {
        private readonly ObservableCollection<Piece> Pieces = [];

        private Piece? activePiece;

        private Canvas canvas;

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

            RenderCanvas();
        }

        private bool MovePieceDown()
        {
            activePiece.Y++;

            return true;
        }

        private void AddNewPiece()
        {
            // Pick random Piece
            activePiece = new Straight();
            Pieces.Add(activePiece);
        }

        private void RenderCanvas()
        {
            canvas.Children.Clear();
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    var activePiece = Pieces.FirstOrDefault(p => p.IsOnCoord(x, y));

                    var rect = new Rectangle()
                    {
                        Fill = activePiece == null ? Brushes.Black : activePiece.Color,
                        Height = 10,
                        Width = 10,
                        [Canvas.LeftProperty] = x * 10,
                        [Canvas.TopProperty] = y * 10,
                    };
                    canvas.Children.Add(rect);
                }
            }
        }

        // System

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public async Task Start()
        {
            while (true)
            {
                Loop();
                await Task.Delay(1000);
            }
        }


        // Game Controls

        public void MoveLeft()
        {
            if (activePiece?.X > 0)
            {
                activePiece.X--;
            }
        }

        public void MoveRight()
        {
            if (activePiece?.X < 20)
            {
                activePiece.X++;
            }
        }

        public void MoveDown() { }

        public void Rotate() { }

        public void Pause() { }
    }
}
