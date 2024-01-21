using EndlessEscapade.Common.Items.Components;
using EndlessEscapade.Content.Gores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items;

public sealed class QuadBarrelShotgun : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
        return entity.type == ItemID.QuadBarrelShotgun;
    }

    public override void SetDefaults(Item entity) {
        if (!entity.TryGetGlobalItem(out ItemBulletCasings component)) {
            return;
        }

        component.Enabled = true;
        component.CasingAmount = 4;
        component.CasingType = ModContent.GoreType<ShellCasing>();
    }
}
