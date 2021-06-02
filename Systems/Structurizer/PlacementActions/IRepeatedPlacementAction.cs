namespace EEMod.Systems.Structurizer.PlacementActions
{
    public interface IRepeatedPlacementAction<out TPlacementAction> : IPlacementAction
        where TPlacementAction : IPlacementAction
    {
        ushort Flag { get; }

        byte RepetitionCount { get; }

        TPlacementAction PlacementAction { get; }
    }
}