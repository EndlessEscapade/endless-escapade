using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.CoralGolem
{
    public class CoralGolem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Golem");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 7)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 8)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.lavaImmune = true;
            npc.friendly = false;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.lifeMax = 50000;
            npc.defense = 40;
            npc.damage = 95;
            npc.value = Item.buyPrice(0, 8, 0, 0);
            npc.noTileCollide = true;
            npc.width = 250;
            npc.height = 230;
            drawOffsetY = 40;

            npc.npcSlots = 24f;
            npc.knockBackResist = 0f;

            musicPriority = MusicPriority.BossMedium;

            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }

            //music = Compatibilities.EEMusic?.GetSoundSlot(SoundType.Music, "Sounds/Music/Precursors") ?? MusicID.Boss3;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/Bosses/CoralGolem/CoralGolemGlow"), npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}