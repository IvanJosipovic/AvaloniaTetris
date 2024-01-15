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

    private readonly DispatcherTimer timer = new DispatcherTimer()
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
    }

    public string Greeting => "Welcome to Avalonia Tetris!";

    private void GenerateControls()
    {
        Controls.Clear();

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                Rectangle myRectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb((byte)(y * x * 20), 0, 0)),
                    Width = 50,
                    Height = 50,
                    [Grid.ColumnProperty] = y,
                    [Grid.RowProperty] = x
                };
                Controls.Add(myRectangle);
            }
        }
    }
}
