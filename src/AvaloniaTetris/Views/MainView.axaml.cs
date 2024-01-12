using Avalonia.Controls;

namespace AvaloniaTetris.Views;

public partial class MainView : UserControl
{
    Game game;

    public MainView()
    {
        InitializeComponent();

        game = new Game();

        game.SetCanvas(canvas);

        _ = game.Start();
    }
}
