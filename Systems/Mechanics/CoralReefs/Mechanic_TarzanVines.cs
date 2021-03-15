using EEMod.Extensions;
using EEMod.Systems;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class TarzanVines : Mechanic
    {
        public bool bufferVariable;
        public float rotGoto;
        public float rotationBuffer;
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (int index in VerletHelpers.EndPointChains)
            {
                var vec = Verlet.Points[index].point;
                if ((vec - Main.LocalPlayer.Center).LengthSquared() < 100 * 100)
                {
                    float lerp = 1f - (vec - Main.LocalPlayer.Center).LengthSquared() / (100 * 100);

                    if (EEMod.Inspect.Current)
                    {
                        if ((vec - Main.LocalPlayer.Center).LengthSquared() < 20 * 20)
                        {
                            if (Main.LocalPlayer.fullRotation != 0)
                            {
                                Main.LocalPlayer.fullRotation = 0;
                            }
                            if (Main.LocalPlayer.controlLeft)
                            {
                                Verlet.Points[index].point.X -= 0.3f;
                            }
                            if (Main.LocalPlayer.controlRight)
                            {
                                Verlet.Points[index].point.X += 0.3f;
                            }
                            if (index > 0)
                                Main.LocalPlayer.fullRotation = ((Verlet.Points[index - 1].point - Verlet.Points[index].point).ToRotation() + (float)Math.PI / 2f) * 0.45f;
                        }
                        if (EEMod.Inspect.JustPressed)
                        {
                            Verlet.Points[index].point.X += Main.LocalPlayer.velocity.X * 1.5f;
                        }
                        Main.LocalPlayer.velocity = (vec - Main.LocalPlayer.Center) / (1 + (vec - Main.LocalPlayer.Center).LengthSquared() / 2000f);
                        Main.LocalPlayer.gravity = 0f;
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine = true;
                    }
                    else
                    {
                        Helpers.DrawAdditive(ModContent.GetInstance<EEMod>().GetTexture("Masks/Extra_49"), vec.ForDraw(), Color.Green * lerp, lerp * 0.2f);
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine = false;
                    }
                    if (Main.LocalPlayer.controlUseItem)
                    {
                        Verlet.Points[index].point = Main.LocalPlayer.Center;
                    }
                }
                Lighting.AddLight(vec, new Vector3(235, 166, 0) / 500);
            }
        }

        public override void OnUpdate()
        {
            #region Spawning particles
            if (bufferVariable != Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine)
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine)
                {
                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(1f));
                    for (int i = 0; i < 20; i++)
                    {
                        EEMod.Particles.Get("Main").SpawnParticles(Main.LocalPlayer.Center, default, 1, Color.White, new Spew(6.14f, 1f, Vector2.One / 2f, 0.98f));
                    }
                }
                if (!Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine)
                {
                    if (Main.LocalPlayer.velocity.X > 0)
                    {
                        rotGoto = -6.28f;
                    }
                    else
                    {
                        rotGoto = 6.28f;
                    }
                }
            }
            #endregion
            if (!Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine)
            {
                rotationBuffer += (rotGoto - rotationBuffer) / 12f;
                if (Math.Abs(6.28f - rotationBuffer) > 0.01f)
                {
                    Main.LocalPlayer.fullRotation = rotationBuffer;
                    Main.LocalPlayer.fullRotationOrigin = new Vector2(Main.LocalPlayer.width / 2f, Main.LocalPlayer.height / 2f);
                }
                else if (Main.LocalPlayer.fullRotation != 0)
                {
                    Main.LocalPlayer.fullRotation = 0;
                }
            }
            else
            {
                rotationBuffer = 0f;
            }
            bufferVariable = Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine;
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }
        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}