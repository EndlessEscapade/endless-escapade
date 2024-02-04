using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items.Components;

[Autoload(Side = ModSide.Client)]
public sealed class ItemBulletCasings : ItemComponent
{
    public int CasingAmount { get; set; } = 1;
    public int CasingType { get; set; } = -1;

    public override bool? UseItem(Item item, Player player) {
        if (!Enabled || CasingType < 0 || CasingAmount <= 0) {
            return base.UseItem(item, player);
        }

        var texture = TextureAssets.Gore[CasingType].Value;
        var position = player.Center + new Vector2(12f * player.direction, -6f);

        if (player.direction == -1) {
            position -= texture.Size() / 2f + new Vector2(12f, 0f);
        }

        for (var i = 0; i < CasingAmount; i++) {
            var velocity = new Vector2(Main.rand.NextFloat(0.75f, 1f) * -player.direction, -Main.rand.NextFloat(1f, 1.5f)) + player.velocity * 0.5f;

            Gore.NewGore(player.GetSource_ItemUse(item), position, velocity, CasingType);
        }

        return base.UseItem(item, player);
    }
}
