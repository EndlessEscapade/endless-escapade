namespace EEMod.Systems.Structurizer.PlacementActions
{
    public interface IRepeatedPlacementAction<out TPlacementAction> : IPlacementAction
        where TPlacementAction : IPlacementAction
    {
        ushort RepetitionCount { get; }

        TPlacementAction PlacementAction { get; }
    }
}