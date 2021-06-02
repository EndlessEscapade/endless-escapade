namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceLavaAction : BasePlacementActionWithLiquid
    {
        public PlaceLavaAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag => 0xFFFA;

        public override void Place(ref int i, ref int j)
        {

        }
    }
}