using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class ToxicPuffer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Puffer");
        }

        public override void SetDefaults()
        {
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 38;
            npc.defense = 2;

            npc.buffImmune[BuffID.Confused] = true;

            npc.width = 62;
            npc.height = 48;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }

        public bool big;

        public override void AI()
        {
            bool isBig = false;
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (Vector2.DistanceSquared(npc.position, player.position) <= (160*160))
                    isBig = true;
            }

            if (isBig == false)
            {
                big = false;
                npc.width = 26;
                npc.height = 26;
            }
            if (isBig == true)
            {
                big = true;
                npc.width = 62;
                npc.height = 48;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.PufferSmall;
            Vector2 offset = new Vector2(0, 0);
            if (big)
                texture = TextureCache.PufferBig;
            if (!big)
                offset = new Vector2(8, 16);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + offset, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }
}