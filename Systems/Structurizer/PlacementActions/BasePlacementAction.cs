namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BasePlacementAction : IPlacementAction
    {
        public abstract ushort Flag { get; }

        public virtual ushort EntryData { get; } = 0;

        public virtual byte StyleData { get; } = 0;

        public virtual byte AlternateStyleData { get; } = 0;

        public abstract void Place();
    }
}