using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTetris;

public partial class GridPoint : ObservableObject
{
    [ObservableProperty]
    int _indexColor;

    [ObservableProperty]
    bool _isActive;
}
