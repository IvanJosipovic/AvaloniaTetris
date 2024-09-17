using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTetris;

public partial class GridPoint : ObservableObject
{


    [ObservableProperty]
    bool _isActive;

    [ObservableProperty]
    GridFill _fillType;


}
