using System.Linq;
using Terraria;

namespace EEMod.Extensions
{
    public static class PlayerExtensions
    {
        public static EEPlayer Interitos(this Player player) => player.GetModPlayer<EEPlayer>();

        public static float MeleeDamage(this Player player) => player.allDamage + player.meleeDamage - 1f;
        public static float RangedDamage(this Player player) => player.allDamage + player.rangedDamage - 1f;
        public static float MagicDamage(this Player player) => player.allDamage + player.magicDamage - 1f;
        public static float MinionDamage(this Player player) => player.allDamage + player.minionDamage - 1f;
        public static float ThrownDamage(this Player player) => player.allDamage + player.thrownDamage - 1f;
        public static float AverageDamage(this Player player) => player.allDamage + (player.meleeDamage + player.rangedDamage + player.magicDamage + player.minionDamage + player.thrownDamage - 5f) / 5f;

        public static bool IsUnderwater(this Player player) => Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
        public static bool InSpace(this Player player)
        {
            float x = Main.maxTilesX / 4200f;
            x *= x;
            float spaceGravityMult = (float)((player.position.Y / 16f - (60f + 10f * x)) / (Main.worldSurface / 6.0));
            return spaceGravityMult < 1f;
        }

        public static bool PillarZone(this Player player) => player.ZoneTowerStardust || player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula;
        public static bool InventoryHas(this Player player, params int[] items) => player.inventory.Any(item => items.Contains(item.type));
    }
}
