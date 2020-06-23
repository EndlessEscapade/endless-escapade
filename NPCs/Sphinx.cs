using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs
{
    public class Sphinx : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sphinx");
            //Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            npc.width = 816;
            npc.height = 508;
            npc.damage = 12;
            npc.defense = 0;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.alpha = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
              {
                  Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
                  for (int k = 0; k < npc.oldPos.Length; k++)
                  {
                      Texture2D Trail = Main.npcTexture[npc.type];
                      Color lightColor = drawColor;
                      Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                      Color color = npc.GetAlpha(lightColor) * ((npc.oldPos.Length - k) / (float)npc.oldPos.Length);
                      spriteBatch.Draw(Trail, drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
                  }
                  return true;
              }
        public override void NPCLoot()
        {
            // this is still pretty useless to do
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<QuartzGem>(), Main.rand.Next(1, 3));
        }
    }
}
