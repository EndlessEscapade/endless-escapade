namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceHalfBrickAction : BasePlacementActionWithEntry
    {
        public PlaceHalfBrickAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag => 0xFFEE;

        public override void Place(ref int i, ref int j)
        {

        }
    }
}