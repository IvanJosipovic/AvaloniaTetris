using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTetris
{
    internal abstract partial class Piece : ObservableObject
    {
        [ObservableProperty]
        public bool _isActive;

        [ObservableProperty]
        public int[,] _size;

        [ObservableProperty]
        public int _orientation;

        [ObservableProperty]
        public IBrush _color;

        [ObservableProperty]
        int _x;

        [ObservableProperty]
        int _y;

        public bool IsOnCoord(int x, int y)
        {
            // Y is beyond our Piece
            if (y > Y)
            {
                return false;
            }

            return false;
        }
    }

    internal class Straight : Piece
    {
        public Straight()
        {
            Color = Brushes.Red;
            Size = new int[,] { { 1, 1, 1, 1 } };
            X = 3;
            Y = 0;
        }
    }

    internal class Square : Piece
    {
        public Square()
        {
            Color = Brushes.Green;
            Size = new int[,] { { 1, 1 },
                                { 1, 1 }};
            X = 4;
            Y = 0;
        }
    }

    internal class T : Piece
    {
        public T()
        {
            Color = Brushes.Yellow;
            Size = new int[,] { { 1, 1, 1 },
                                { 0, 1, 0 }};
            X = 3;
            Y = 0;
        }
    }

    internal class L : Piece
    {
        public L()
        {
            Color = Brushes.Blue;
            Size = new int[,] { { 1, 1, 1 },
                                { 0, 0, 1 }};
            X = 4;
            Y = 0;
        }
    }

    internal class S : Piece
    {
        public S()
        {
            Color = Brushes.Pink;
            Size = new int[,] { { 1, 1, 0 },
                                { 0, 1, 1 }};
            X = 4;
            Y = 0;
        }
    }
}
