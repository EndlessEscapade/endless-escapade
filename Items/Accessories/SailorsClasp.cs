using EEMod.Projectiles.Hooks;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class SailorsClasp : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Clasp");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AmethystHook);
            Item.shootSpeed = 18f;
            Item.shoot = ModContent.ProjectileType<SailorsClaspProjectile>();
        }
    }
}