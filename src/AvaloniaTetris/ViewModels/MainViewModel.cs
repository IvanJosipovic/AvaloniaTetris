using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace AvaloniaTetris.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    bool _isDebug;

    public ObservableCollection<Control> Controls { get; set; } = [];

    private readonly DispatcherTimer timer = new()
    {
        Interval = TimeSpan.FromMicroseconds(100)
    };

    public Game Game { get; set; }

    public MainViewModel()
    {
        GenerateControls();

        Game = new Game();

        Game.Start();

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

                var piece = Game.GetAtCoords(x, y);

                var border = ((Border)control);

                if (piece == null)
                {
                    border.Background = Brushes.Black;
                }
                else
                {
                    if (piece is Straight)
                    {
                        border.Background = Brushes.Red;
                    }
                    else if (piece is Square)
                    {
                        border.Background = Brushes.Blue;
                    }
                    else if (piece is S)
                    {
                        border.Background = Brushes.Green;
                    }
                    else if (piece is T)
                    {
                        border.Background = Brushes.Pink;
                    }
                    else if (piece is L)
                    {
                        border.Background = Brushes.Purple;
                    }
                }

                if (IsDebug)
                {
                    ((TextBlock)border.Child).Text = $"{x},{y}";
                }
                else
                {
                    ((TextBlock)border.Child).Text = "";
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
                Border border = new();
                border.Background = Brushes.Black;
                border.BorderBrush = Brushes.White;
                border.BorderThickness = Thickness.Parse("1");

                TextBlock txt = new()
                {
                    Width = 50,
                    Height = 50,
                };

                if (IsDebug)
                {
                    txt.Text = $"{x},{y}";
                }

                border.Child = txt;

                Controls.Add(border);
            }
        }
    }
}
