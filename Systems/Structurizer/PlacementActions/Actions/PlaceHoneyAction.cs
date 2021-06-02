namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceHoneyAction : BasePlacementActionWithLiquid
    {
        public PlaceHoneyAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag => 0xFFF9;

        public override void Place(ref int i, ref int j)
        {

        }
    }
}