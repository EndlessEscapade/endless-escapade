using EndlessEscapade.Common.Items.Components;
using EndlessEscapade.Content.Gores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items.Guns;

public sealed class ChainGun : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
        return entity.type == ItemID.ChainGun;
    }

    public override void SetDefaults(Item entity) {
        entity.TryEnableComponent<ItemBulletCasings>(c => { c.CasingType = ModContent.GoreType<BulletCasing>(); });
    }
}
