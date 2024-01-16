using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTetris;

public partial class GridPoint : ObservableObject
{
    [ObservableProperty]
    int _type;

    [ObservableProperty]
    bool _isActive;
}
