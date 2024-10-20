using EndlessEscapade.Common.Movement;

namespace EndlessEscapade.Common.Items;

public sealed class FlipperGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
        return entity.type == ItemID.Flipper;
    }

    public override void UpdateEquip(Item item, Player player) {
        base.UpdateEquip(item, player);

        player.accFlipper = false;

        if (!player.TryGetModPlayer(out SwimmingPlayer swimmingPlayer)) {
            return;
        }

        swimmingPlayer.GetMovementSpeed() += 0.5f;
        swimmingPlayer.GetMovementAcceleration() += 0.1f;
    }
}
