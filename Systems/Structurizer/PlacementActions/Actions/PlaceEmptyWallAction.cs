namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class EmptyWallAction : BasePlacementAction
    {
        public override ushort Flag => 0xFFF2;

        public override void Place()
        {
        }
    }
}