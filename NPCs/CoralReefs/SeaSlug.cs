using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class SeaSlug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sea Slug");
            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 67;

            npc.friendly = true;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 5;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            item.stack = 2;

            try
            {
                var npcCenter = npc.Center.ToTileCoordinates();
                Tile tile = Main.tile[npcCenter.X, npcCenter.Y];
                if (!WorldGen.SolidTile(npcCenter.X, npcCenter.Y) && tile.liquid == 0)
                {
                    tile.liquid = (byte)Main.rand.Next(50, 150);
                    tile.lava(true);
                    tile.honey(false);
                    WorldGen.SquareTileFrame(npcCenter.X, npcCenter.Y, true);
                }
            }
            catch
            {
                return;
            }
        }

        private readonly int variation = Main.rand.Next(3);

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 18 * variation;
        }
    }
}