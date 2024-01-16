using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvaloniaTetris.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    bool _isDebug;

    public ObservableCollection<Control> Controls { get; set; } = [];

    public Game Game { get; set; } = new Game();

    public MainViewModel()
    {
        GenerateControls();

        Game.Start();
    }

    private void GenerateControls()
    {
        Controls.Clear();

        for (int y = 19; y >= 0; y--)
        {
            for (int x = 0; x <= 9; x++)
            {
                Border border = new()
                {
                    BorderBrush = Brushes.White,
                    BorderThickness = Thickness.Parse("1"),
                    Background = Brushes.Black
                };

                var binding = new Binding($"Game.Positions[{x},{y}].Type")
                {
                    Converter = IntToColorConverter.Instance
                };

                border.Bind(Border.BackgroundProperty, binding);

                TextBlock txt = new()
                {
                    Width = 50,
                    Height = 50,
                };

                border.Child = txt;

                if (IsDebug)
                {
                    txt.Text = $"{x},{y}";
                }

                Controls.Add(border);
            }
        }
    }
}
