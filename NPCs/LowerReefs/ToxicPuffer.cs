using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.LowerReefs
{
    public class ToxicPuffer : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Puffer");
        }

        public override void SetDefaults()
        {
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.lifeMax = 38;
            NPC.defense = 2;

            NPC.buffImmune[BuffID.Confused] = true;

            NPC.width = 26;
            NPC.height = 26;

            NPC.noGravity = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.ToxicPufferBanner>();
        }

        public bool big;

        public override void AI()
        {
            bool isBig = false;
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (Vector2.Distance(NPC.Center, player.Center) <= 320)
                {
                    isBig = true;
                }
            }

            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (!isBig)
            {
                big = false;
                NPC.width = 26;
                NPC.height = 26;
                NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * 2;
            }
            if (isBig)
            {
                big = true;
                NPC.width = 62;
                NPC.height = 48;
                NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * 1;
            }

            NPC.rotation = NPC.velocity.X / 5;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/CoralReefs/ToxicPufferSmall").Value;
            Vector2 offset = Vector2.Zero;
            if (big)
            {
                texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/CoralReefs/ToxicPuffer").Value;
                Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + offset, new Rectangle(0, 0, 62, 48), drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            if (!big)
            {
                offset = new Vector2(8, 16);
                Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + offset, new Rectangle(0, 0, 26, 26), drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            return false;
        }
    }
}