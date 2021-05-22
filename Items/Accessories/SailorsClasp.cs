using EEMod.Projectiles.Hooks;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class SailorsClasp : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Clasp");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.AmethystHook);
            item.shootSpeed = 18f;
            item.shoot = ModContent.ProjectileType<SailorsClaspProjectile>();
        }
    }
}