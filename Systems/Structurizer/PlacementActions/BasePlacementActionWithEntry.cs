namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BasePlacementActionWithEntry : BasePlacementAction
    {
        public override ushort EntryData { get; }

        protected BasePlacementActionWithEntry(ushort entry)
        {
            EntryData = entry;
        }
    }
}