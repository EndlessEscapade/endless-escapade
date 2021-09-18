using System.Linq;
using Terraria;

namespace EEMod.Extensions
{
    public static class PlayerExtensions
    {
        public static EEPlayer EEPlayer(this Player player) => player.GetModPlayer<EEPlayer>();

        public static bool IsAlive(this Player player) => player?.active is true && !(player.dead || player.ghost);

        public static bool IsUnderwater(this Player player) => Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);

        public static bool InSpace(this Player player)
        {
            float x = Main.maxTilesX / 4200f;
            x *= x;
            float spaceGravityMult = (float)((player.position.Y / 16f - (60f + 10f * x)) / (Main.worldSurface / 6.0));
            return spaceGravityMult < 1f;
        }

        public static long GetSavings(this Player player) //Thanks Dire
        {
            long inv = Utils.CoinsCount(out _, player.inventory, new int[]
            {
        58, //Mouse item
        57, //Ammo slots
        56,
        55,
        54
            });
            int[] empty = new int[0];
            long piggy = Utils.CoinsCount(out _, player.bank.item, empty);
            long safe = Utils.CoinsCount(out _, player.bank2.item, empty);
            long forge = Utils.CoinsCount(out _, player.bank3.item, empty);
            return Utils.CoinsCombineStacks(out _, new long[]
            {
        inv,
        piggy,
        safe,
        forge
            });
        }

        public static bool PillarZone(this Player player) => player.ZoneTowerStardust || player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula;

        public static bool InventoryHas(this Player player, params int[] items) => items.Any(itemType => player.HasItem(itemType));//player.inventory.Any(item => items.Contains(item.type));
    }
}