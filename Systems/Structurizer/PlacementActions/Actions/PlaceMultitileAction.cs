namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileAction : BasePlacementActionWithEntry
    {
        public PlaceMultitileAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag => 0xFFFC;

        public override void Place()
        {
        }
    }
}