namespace EEMod.Systems.Structurizer.PlacementActions
{
    public interface IRepeatedPlacementAction : IPlacementAction
    {
        ushort RepetitionCount { get; }
    }
}