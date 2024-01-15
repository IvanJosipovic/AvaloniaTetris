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

                var txtBlock = ((TextBlock)control);

                if (piece == null)
                {
                    txtBlock.Background = Brushes.Black;
                }
                else
                {
                    if (piece is Straight)
                    {
                        txtBlock.Background = Brushes.Red;
                    }
                    else if (piece is Square)
                    {
                        txtBlock.Background = Brushes.Blue;
                    }
                    else if (piece is S)
                    {
                        txtBlock.Background = Brushes.Green;
                    }
                    else if (piece is T)
                    {
                        txtBlock.Background = Brushes.Pink;
                    }
                    else if (piece is L)
                    {
                        txtBlock.Background = Brushes.Purple;
                    }
                }

                if (IsDebug)
                {
                    txtBlock.Text = $"{x},{y}";
                }
                else
                {
                    txtBlock.Text = "";
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
                    Width = 50,
                    Height = 50,
                };

                if (IsDebug)
                {
                    txt.Text = $"{x},{y}";
                }

                Controls.Add(txt);
            }
        }
    }
}
