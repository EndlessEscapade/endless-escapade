using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Extensions;
using EEMod.Backgrounds;

namespace EEMod.NPCs.CoralReefs
{
    public class SeaSlug : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sea Slug");
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 4;
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

            SpawnModBiomes = new int[1] { ModContent.GetInstance<EEBestiaryBiome>().Type }; // Associates this NPC with the ExampleSurfaceBiome in Bestiary
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
            if (NPC.ai[0] == 0)
            {
                NPC.frame = new Rectangle(0, Main.rand.Next(4) * 18, 32, 18);
                NPC.ai[0] = 1;
            }

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("EEMod/NPCs/CoralReefs/SeaSlugGlow").Value;
            Main.spriteBatch.Draw(glow, NPC.Center.ForDraw(), NPC.frame, Color.White, NPC.rotation, Vector2.Zero, 1f, (NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);
        }
    }
}