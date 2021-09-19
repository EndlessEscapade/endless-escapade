using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.NPCs.CoralReefs
{
    public class SeaSlug : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sea Slug");
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.friendly = true;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.lifeMax = 5;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;

            NPC.aiStyle = 67;

            NPC.width = 32;
            NPC.height = 18;
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
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/CoralReefs/SeaSlug").Value;
            Main.spriteBatch.Draw(tex, NPC.position.ForDraw(), new Rectangle(0, variation * 18, 32, 18), Lighting.GetColor((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)), NPC.rotation, Vector2.Zero, 1f, (NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("EEMod/NPCs/CoralReefs/SeaSlugGlow").Value;
            Main.spriteBatch.Draw(glow, NPC.position.ForDraw(), new Rectangle(0, variation * 18, 32, 18), Color.White, NPC.rotation, Vector2.Zero, 1f, (NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);
        }

        private readonly int variation = Main.rand.Next(4);
    }
}