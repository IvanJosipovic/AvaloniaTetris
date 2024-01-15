using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;

namespace AvaloniaTetris.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Control> Controls { get; set; } = [];

    private readonly DispatcherTimer timer = new()
    {
        Interval = TimeSpan.FromMicroseconds(100)
    };

    Game game;

    public MainViewModel()
    {
        GenerateControls();

        game = new Game();

        game.Start();

        timer.Tick += Timer_Tick;

        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        int count = 0;
        for (int y = 19; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                var control = Controls[count++];

                var piece = game.GetAtCoords(x, y);

                if (piece == null)
                {
                    ((TextBlock)control).Background = Brushes.Black;
                } else
                {
                    if (piece is Straight)
                    {
                        ((TextBlock)control).Background = Brushes.Red;
                    }
                    else if (piece is Square)
                    {
                        ((TextBlock)control).Background = Brushes.Blue;
                    }
                    else if (piece is S)
                    {
                        ((TextBlock)control).Background = Brushes.Green;
                    }
                    else if (piece is T)
                    {
                        ((TextBlock)control).Background = Brushes.Pink;
                    }
                    else if (piece is L)
                    {
                        ((TextBlock)control).Background = Brushes.Purple;
                    }
                }
            }
        }
    }

    private void GenerateControls()
    {
        Controls.Clear();

        for (int y = 19; y >= 0; y--)
        {
            for (int x = 0; x <= 9; x++)
            {
                TextBlock txt = new()
                {
                    Background = Brushes.Black,
                    Text = $"{x},{y}",
                    Width = 50,
                    Height = 50,
                };

                Controls.Add(txt);
            }
        }
    }
}
