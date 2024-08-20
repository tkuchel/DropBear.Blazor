#region

using DropBear.Blazor.Components.Bases;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

public sealed partial class ProgressCircle : DropBearComponentBase
{
    [Parameter] public int Progress { get; set; }
    [Parameter] public int Size { get; set; } = 60;

    private static int ViewBoxSize => 60;
    private static int Radius => (ViewBoxSize / 2) - 3;
    private static double Circumference => 2 * Math.PI * Radius;
    private double Offset => Circumference - (Progress / 100.0 * Circumference);

    protected override void OnParametersSet()
    {
        // Ensure Progress is between 0 and 100
        Progress = Math.Clamp(Progress, 0, 100);
    }
}
