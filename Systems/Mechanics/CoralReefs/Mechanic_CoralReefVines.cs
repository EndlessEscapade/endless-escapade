using EEMod.Extensions;
using EEMod.ID;
using EEMod.Tiles.Furniture;
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
    public class CoralReefVines : Mechanic
    {
        protected override Layer DrawLayering => Layer.BehindTiles;
        public void DrawVines()
        {
            if (EESubWorlds.ChainConnections.Count > 0)
            {
                for (int i = 1; i < EESubWorlds.ChainConnections.Count - 2; i++)
                {
                    Vector2 addOn = new Vector2(0, 8);
                    Vector2 ChainConneccPos = EESubWorlds.ChainConnections[i] * 16;
                    Vector2 LastChainConneccPos = EESubWorlds.ChainConnections[i - 1] * 16;
                    Tile CurrentTile = Main.tile[(int)EESubWorlds.ChainConnections[i].X, (int)EESubWorlds.ChainConnections[i].Y];
                    Tile LastTile = Main.tile[(int)EESubWorlds.ChainConnections[i - 1].X, (int)EESubWorlds.ChainConnections[i - 1].Y];
                    bool isValid = CurrentTile.active() && LastTile.active() && Main.tileSolid[CurrentTile.type] && Main.tileSolid[LastTile.type];
                    Vector2 MidNorm = (ChainConneccPos + LastChainConneccPos) / 2;
                    Vector2 Mid = (ChainConneccPos + LastChainConneccPos) / 2 + new Vector2(0, 50 + (float)(Math.Sin(ElapsedTicks/60f + ChainConneccPos.X) * 30));
                    Vector2 lerp1 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.2f);
                    Vector2 lerp2 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.8f);
                    if (MidNorm.Y > 100 * 16 && Vector2.DistanceSquared(ChainConneccPos, LastChainConneccPos) < 40 * 16 * 40 * 16 && Vector2.DistanceSquared(Main.LocalPlayer.Center, MidNorm) < 2000 * 2000 && isValid && Collision.CanHit(lerp1, 1, 1, lerp2, 1, 1))
                    {
                        Color chosen = Color.Lerp(Color.Yellow, Color.LightGoldenrodYellow, Main.rand.NextFloat(1f));
                        Helpers.DrawBezier(ModContent.GetInstance<EEMod>().GetTexture("Projectiles/Vine"), Color.White, ChainConneccPos, LastChainConneccPos, Mid, 0.6f, MathHelper.PiOver2, true);
                        Helpers.DrawBezier(ModContent.GetInstance<EEMod>().GetTexture("Projectiles/VineLight"), Color.White, ChainConneccPos + addOn, LastChainConneccPos + addOn, Mid + addOn, 8f, MathHelper.PiOver2, true);
                        Helpers.DrawBezier(ModContent.GetInstance<EEMod>().GetTexture("Projectiles/VineLightGlow"), Color.White * Math.Abs((float)Math.Sin(Main.GameUpdateCount / 200f + ChainConneccPos.X)), ChainConneccPos + addOn, LastChainConneccPos + addOn, Mid + addOn, 8f, MathHelper.PiOver2);
                        Helpers.DrawParticlesAlongBezier(LastChainConneccPos + addOn, ChainConneccPos + addOn, Mid + addOn, 1 / 8f, chosen, 0.005f, (Vector2.UnitY).RotatedBy(Main.rand.NextFloat(6.24f)), new SlowDown(0.98f), new RotateTexture(Main.rand.NextFloat(-0.03f, 0.03f)), new SetMask(ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient")), new AfterImageTrail(1f), new RotateVelocity(Main.rand.NextFloat(-0.12f, 0.12f)), new SetLighting(chosen.ToVector3(), 0.3f), new ZigzagMotion(40f, 3f));
                    }
                }
            }
        }
        public override void OnDraw()
        {
            if(Main.worldName == KeyID.CoralReefs) DrawVines();
        }
    }
}