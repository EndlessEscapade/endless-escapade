using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class KelpvineCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpvine Cannon");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.useAnimation = 24;
            item.useTime = 24;

            item.shootSpeed = 8f;
            item.width = 44;
            item.height = 26;

            item.rare = ItemRarityID.Green;

            item.shoot = ModContent.ProjectileType<KelpHookProj>();

            item.autoReuse = false; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item11;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}