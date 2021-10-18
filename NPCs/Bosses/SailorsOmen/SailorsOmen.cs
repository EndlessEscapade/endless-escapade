using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.NPCs.Bosses.SailorsOmen
{
    public class SailorsOmen : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Omen");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.friendly = true;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.alpha = 0;
            NPC.lifeMax = 1000000;
            NPC.width = 40;
            NPC.height = 40;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.damage = 0;
            NPC.knockBackResist = 0f;
        }

        public List<OmenParticle> omenParticles = new List<OmenParticle>();

        public override void AI()
        {
            omenParticles.Add(new OmenParticle(NPC.Center, Vector2.UnitY.RotatedByRandom(1.57f) * 2));

            for (int i = 0; i < omenParticles.Count; i++)
            {
                if (omenParticles[i] != null) omenParticles[i].Update();

                if (omenParticles[i] != null && omenParticles[i].timeLeft < -60)
                {
                    omenParticles[i].position = Vector2.Zero;
                    omenParticles[i].dead = true;
                    omenParticles.RemoveAt(i);
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/SailorsOmen/OmenMagic2").Value;

            Color outlineColor = Color.DarkCyan;

            //Layer 0, navy radial mask

            foreach (OmenParticle p in omenParticles)
            {
                if (!p.dead)
                {
                    Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, p.position - Main.screenPosition, Color.DarkCyan * MathHelper.Clamp((p.timeLeft + 30) / 30f, 0f, 1f), 0.25f, 0f);
                }
            }

            //Layer 1, navy outline
            /*Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.White.CurrentTechnique.Passes[0].Apply();
            EEMod.White.Parameters["alpha"].SetValue(1f);
            EEMod.White.Parameters["color"].SetValue(new Vector3(outlineColor.R, outlineColor.G, outlineColor.B) / 255f);

            foreach (OmenParticle p in omenParticles)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 4;
                    spriteBatch.Draw(tex, p.position + offsetPositon - Main.screenPosition, null, Color.White, 0f, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                }
            }*/

            //Layer 2, cyan fade inline
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            outlineColor = Color.Cyan;

            foreach (OmenParticle p in omenParticles)
            {
                if (!p.dead)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        EEMod.WhiteOutline.CurrentTechnique.Passes[0].Apply();
                        EEMod.WhiteOutline.Parameters["color"].SetValue((new Vector3(outlineColor.R, outlineColor.G, outlineColor.B) / 255f));
                        EEMod.WhiteOutline.Parameters["alpha"].SetValue(1f);

                        Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                        spriteBatch.Draw(tex, p.position + offsetPositon - Main.screenPosition, null, Color.White, 0f, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                    }
                }
            }

            //Layer 3, white ball
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (OmenParticle p in omenParticles)
            {
                if(!p.dead)
                    spriteBatch.Draw(tex, p.position - Main.screenPosition, null, Color.White, 0f, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            }
        }

        public class OmenParticle
        {
            public Vector2 position;
            public Vector2 velocity;

            public float timeLeft;

            public bool dead;

            public OmenParticle(Vector2 _position, Vector2 _velocity)
            {
                position = _position;
                velocity = _velocity;

                timeLeft = 60;
            }

            public void Update()
            {
                position += velocity;

                timeLeft--;
            }
        }
    }
}