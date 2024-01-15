using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
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
        Interval = TimeSpan.FromMicroseconds(500)
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
        //RenderControls();

        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 10; x++)
            {

            }
        }
    }

    public string Greeting => "Welcome to Avalonia Tetris!";

    private void GenerateControls()
    {
        Controls.Clear();

        for (int y = 19; y >= 0; y--)
        {
            for (int x = 0; x <= 9; x++)
            {
                TextBlock txt = new()
                {
                    Background = Brushes.Red,
                    Text = $"{x},{y}",
                    Width = 50,
                    Height = 50
                };

                Controls.Add(txt);
            }
        }
    }
}
