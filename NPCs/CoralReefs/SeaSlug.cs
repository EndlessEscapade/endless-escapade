using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.NPCs.CoralReefs
{
    public class SeaSlug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sea Slug");
            Main.npcCatchable[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.friendly = true;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 5;

            npc.lavaImmune = false;
            npc.noTileCollide = false;

            npc.width = 32;
            npc.height = 18;
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

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = ModContent.GetTexture("EEMod/NPCs/CoralReefs/SeaSlug");
            Main.spriteBatch.Draw(tex, npc.position.ForDraw(), new Rectangle(0, variation * 18, 32, 18), Lighting.GetColor((int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)), npc.rotation, Vector2.Zero, 1f, (npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D glow = ModContent.GetTexture("EEMod/NPCs/CoralReefs/SeaSlugGlow");
            Main.spriteBatch.Draw(glow, npc.position.ForDraw(), new Rectangle(0, variation * 18, 32, 18), Color.White, npc.rotation, Vector2.Zero, 1f, (npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);
        }

        private readonly int variation = Main.rand.Next(4);
    }
}