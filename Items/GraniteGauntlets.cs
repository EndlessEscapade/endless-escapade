/*using EEMod.Buffs.Buffs;
using EEMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class GraniteGauntlets : EEItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Granite Gauntlets");
        }

        public override void SetStaticDefaults()
        {
            item.useTime = 120;
            item.useAnimation = 120;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(silver: 20);
            item.useStyle = ItemUseStyleID.HoldingUp;

            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<GraniteGauntletsShield>();
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(ModContent.BuffType<RechargingGauntlets>());
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            base.UseItemHitbox(player, ref hitbox, ref noHitbox);
        }
    }
}*/