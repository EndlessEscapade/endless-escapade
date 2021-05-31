using EEMod.Extensions;
using EEMod.ID;
using EEMod.Systems;
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
    public class AquamarineZiplines : Mechanic
    {
        protected override Layer DrawLayering => Layer.BehindTiles;

        public void DrawAquamarineZiplines()
        {
            if (EESubWorlds.AquamarineZiplineLocations.Count > 0)
            {
                for (int i = 1; i < EESubWorlds.AquamarineZiplineLocations.Count - 2; i++)
                {
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.004f));

                    Vector2 addOn = new Vector2(0, 8);

                    Vector2 ChainConneccPos = EESubWorlds.AquamarineZiplineLocations[i] * 16;
                    Vector2 LastChainConneccPos = EESubWorlds.AquamarineZiplineLocations[i - 1] * 16;

                    Tile CurrentTile = Framing.GetTileSafely((int)EESubWorlds.AquamarineZiplineLocations[i].X, (int)EESubWorlds.AquamarineZiplineLocations[i].Y);
                    Tile LastTile = Framing.GetTileSafely((int)EESubWorlds.AquamarineZiplineLocations[i - 1].X, (int)EESubWorlds.AquamarineZiplineLocations[i - 1].Y);

                    bool isValid = CurrentTile.active() && LastTile.active() && Main.tileSolid[CurrentTile.type] && Main.tileSolid[LastTile.type];

                    Vector2 MidNorm = (ChainConneccPos + LastChainConneccPos) / 2;
                    Vector2 Mid = (ChainConneccPos + LastChainConneccPos) / 2;

                    Vector2 lerp1 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.1f);
                    Vector2 lerp2 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.9f);

                    float rot = (ChainConneccPos - LastChainConneccPos).ToRotation();

                    if (Vector2.DistanceSquared(Main.LocalPlayer.Center, MidNorm) < 2000 * 2000 && isValid &&
                        !Framing.GetTileSafely((int)Mid.X / 16, (int)Mid.Y / 16).active()
                        && !Framing.GetTileSafely((int)lerp1.X / 16, (int)lerp1.Y / 16).active()
                        && !Framing.GetTileSafely((int)lerp2.X / 16, (int)lerp2.Y / 16).active()
                        && Collision.CanHit(lerp1, 1, 1, lerp2, 1, 1))
                    {
                        Texture2D a = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/CrystalVineThick");
                        Texture2D b = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/CrystalVineDangleThick");
                        Texture2D d = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/CrystalVineDangleMid");

                        Vector2 addonB = new Vector2(0, b.Height / 2f).RotatedBy(rot);
                        Vector2 addonD = new Vector2(0, d.Height / 2f).RotatedBy(rot);
                        Vector2 addonBR = new Vector2(0, b.Height / 2f).RotatedBy(rot + 3.14f);
                        Vector2 addonDR = new Vector2(0, d.Height / 2f).RotatedBy(rot + 3.14f);

                        Vector2 AddonB(float z)
                        {
                            int choice = (int)(z * 87) % 7 % 2;
                            return choice == 0 ? addonB : addonBR;
                        }
                        Vector2 AddonD(float z)
                        {
                            int choice = (int)(z * 87) % 7 % 2;
                            return choice == 0 ? addonD : addonDR;
                        }

                        if (i % 2 == 0)
                        {
                            Helpers.DrawChain(d, ChainConneccPos, LastChainConneccPos, (float)Math.PI, 1.5f, RandRotationFunc, AddonD, Lighting.GetColor((int)(MidNorm.X / 16), (int)(MidNorm.Y / 16)));
                            Helpers.DrawChain(a,ChainConneccPos, LastChainConneccPos, 0, 1, Lighting.GetColor((int)(MidNorm.X / 16), (int)(MidNorm.Y / 16)));

                            Helpers.DrawParticlesAlongLine(LastChainConneccPos, ChainConneccPos, 0.04f, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), 0.0005f, new Spew(6.14f, 1f, Vector2.One / 4f, 0.99f), new RotateVelocity(0.02f), new AfterImageTrail(.8f), new SimpleBrownianMotion(0.1f));
                        }
                        else
                        {
                            Helpers.DrawChain(b, ChainConneccPos, LastChainConneccPos, (float)Math.PI, 3.5f, RandRotationFunc, AddonB, Lighting.GetColor((int)(MidNorm.X / 16), (int)(MidNorm.Y / 16)));
                            Helpers.DrawChain(a, ChainConneccPos, LastChainConneccPos, 0, 1, Lighting.GetColor((int)(MidNorm.X / 16), (int)(MidNorm.Y / 16)));

                            Helpers.DrawParticlesAlongLine(LastChainConneccPos, ChainConneccPos, 0.04f, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), 0.0005f, new Spew(6.14f, 1f, Vector2.One / 4f, 0.99f), new RotateVelocity(0.02f), new AfterImageTrail(.8f), new SimpleBrownianMotion(0.1f));
                        }
                    }
                }
            }
        }
        
        public float RandRotationFunc(float i)
        {
            int choice = (int)(i * 87) % 7 % 2;

            return choice == 1 ? 0 : (float)Math.PI;
        }
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            if(Main.worldName == KeyID.CoralReefs) DrawAquamarineZiplines();
        }
    }
}