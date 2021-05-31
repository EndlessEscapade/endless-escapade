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
    public class ThinAquamarineBamboo : Mechanic
    {
        protected override Layer DrawLayering => Layer.BehindTiles;

        public void DrawThinCrystalBamboo()
        {
            if (EESubWorlds.ThinCrystalBambooLocations.Count > 0)
            {
                Texture2D stalk = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/CrystalBambooThin");
                Texture2D tip = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/CrystalBambooThinTip");

                for (int i = 0; i < EESubWorlds.ThinCrystalBambooLocations.Count - 2; i += 2)
                {
                    Vector2 begin = (EESubWorlds.ThinCrystalBambooLocations[i] * 16) + new Vector2(8, 8);
                    Vector2 end = EESubWorlds.ThinCrystalBambooLocations[i + 1] * 16;

                    Tile root = Framing.GetTileSafely((int)EESubWorlds.ThinCrystalBambooLocations[i].X, (int)EESubWorlds.ThinCrystalBambooLocations[i].Y);

                    bool isValid = root.active() && Main.tileSolid[root.type] && root.slope() == 0;

                    if (isValid)
                    {
                        Vector2 Mid = (begin + end) / 2f;

                        float rot = (begin - end).ToRotation();

                        if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Mid) < 2000 * 2000)
                        {
                            Color color = Lighting.GetColor((int)EESubWorlds.ThinCrystalBambooLocations[i].X, (int)EESubWorlds.ThinCrystalBambooLocations[i].Y);

                            Helpers.DrawChain(stalk, end, begin, 0, 1, color);

                            Main.spriteBatch.Draw(tip, end.ForDraw(), tip.Bounds, color, rot + 3.14f, new Vector2(-16, 6), 1f, SpriteEffects.None, 0f);

                            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.004f));
                            Helpers.DrawParticlesAlongLine(end, begin, 0.04f, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), 0.0005f, new Spew(6.14f, 1f, Vector2.One / 4f, 0.99f), new RotateVelocity(0.02f), new AfterImageTrail(.8f), new SimpleBrownianMotion(0.1f));
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
            if(Main.worldName == KeyID.CoralReefs) DrawThinCrystalBamboo();
        }
    }
}