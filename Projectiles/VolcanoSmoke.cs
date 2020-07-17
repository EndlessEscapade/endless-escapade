using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Buffs.Buffs;
using Terraria.ID;

namespace EEMod.Projectiles
{
    public class VolcanoSmoke : ModNPC
    {
        public override string Texture => Helpers.EmptyTexture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smoke");
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 48;
            npc.friendly = true;
            npc.noTileCollide = true;
            npc.alpha = 255;
            npc.aiStyle = -1;
            npc.lifeMax = 1000;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
        }

        public override void AI()
        {
            for (int i=0; i<2; i++)
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X + Main.rand.Next(-250, 251), npc.position.Y), 50, 50, DustID.Smoke, Scale: 1);
                Main.dust[dust].velocity = new Vector2(0, -1);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}
