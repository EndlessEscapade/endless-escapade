using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.CoralGolem
{
    public class CoralGolem : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Golem");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 7)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 8)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.lavaImmune = true;
            // NPC.friendly = false;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 50000;
            NPC.defense = 40;
            NPC.damage = 95;
            NPC.value = Item.buyPrice(0, 8, 0, 0);
            NPC.noTileCollide = true;
            NPC.width = 250;
            NPC.height = 230;
            DrawOffsetY = 40;

            NPC.npcSlots = 24f;
            NPC.knockBackResist = 0f;

            //musicPriority = MusicPriority.BossMedium;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }

            //music = Compatibilities.EEMusic?.GetSoundSlot(SoundType.Music, "Sounds/Music/Precursors") ?? MusicID.Boss3;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Bosses/CoralGolem/CoralGolemGlow").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}