using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace EEMod.NPCs.CoralReefs
{
    public class AtlantisCore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlantis Core");
        }

        public int rippleCount = 2;
        public int rippleSize = 13;
        public int rippleSpeed = 200;
        public float distortStrength = 5;
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.alpha = 20;
            npc.lifeMax = 1000000;
            npc.width = 300;
            npc.height = 300;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.damage = 0;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            npc.ai[0] += 0.05f;
            npc.position.Y += (float)Math.Sin(npc.ai[0]) * 2;
            if (npc.life == 0)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
                {
                    Filters.Scene["EEMod:Shockwave"].Deactivate();
                }
            }
        }

        public int size = 300;
        public int sizeGrowth;
        public float num88 = 1;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Shockwave"].IsActive())
            {
                Filters.Scene.Activate("EEMod:Shockwave", npc.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(npc.Center);
                npc.ai[0] = 1;
            }
            npc.ai[1] += Math.Abs((float)Math.Sin(npc.ai[0]));
            if (npc.ai[1] >= 90)
            {
                npc.ai[1] = 0;
            }
            float progress = (90 - npc.ai[1]) / 180f;
            progress *= .3f;
            distortStrength = (npc.ai[1] * 2);
            Filters.Scene["EEMod:Shockwave"].GetShader().UseTargetPosition(npc.Center).UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
            //float num88 = ErosHandler.ShieldStrength / (float)NPC.ShieldStrengthTowerMax;
            if (size < 8000)
            {
                float scaleX = 1.9f;
                float scaleY = 1.24f;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);

                var center = npc.Center - Main.screenPosition;
                float num89 = 0f;
                DrawData drawData = new DrawData(TextureManager.Load("Images/Misc/Perlin"), center, new Rectangle(0, 0, (int)(size * scaleX), (int)(size * scaleY)), Color.White * (num88), npc.rotation, new Vector2((size / 2) * scaleX, (size / 2) * scaleY), npc.scale * (1.1f), SpriteEffects.None, 0);
                GameShaders.Misc["ForceField"].UseColor(new Vector3(1f - num89 * 0.5f));
                GameShaders.Misc["ForceField"].Apply(drawData);
                drawData.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                return;
            }
        }
    }
}
