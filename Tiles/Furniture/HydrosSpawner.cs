using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Materials;
using EEMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using EEMod.Items.Placeables.Ores;
using System;
using EEMod.Prim;
using EEMod.Projectiles.CoralReefs;
using EEMod.NPCs.Bosses.Hydros;

namespace EEMod.Tiles.Furniture
{
    public class HydrosSpawner : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.Direction = TileObjectDirection.None;

            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Shrine to the Seahorse King");
            AddMapEntry(new Color(44, 193, 139), name);
            DustType = DustID.Clentaminator_Cyan;
            DisableSmartCursor = false;
            MinPick = 0;
            MineResist = 1f;
        }

        public int oresGiven = 0;
        public bool spawningHydros = false;
        public int cooldown = 0;

        public Vector2[] positions = new Vector2[3] { new Vector2(-4, -12), new Vector2(52, -28), new Vector2(108, -12) };
        public int[] frames = new int[3];
        public int frameCounter;
        public int cutsceneTicks;

        public override void Load()
        {
            oresGiven = 0;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0) 
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }

                frameCounter++;
                if(frameCounter >= 1)
                {
                    frameCounter = 0;

                    for(int l = 0; l < frames.Length; l++)
                    {
                        frames[l]++;

                        if (frames[l] >= 10) frames[l] = 0;
                    }
                }

                for (int l = 0; l < oresGiven; l++) 
                {
                    if (l == 1) continue;

                    Vector2 orig = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f);

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * 4f;

                        spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig + initRot.RotatedBy((Main.GameUpdateCount / 30f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.Gold * 0.5f);
                    }

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig, new Rectangle(0, frames[l] * 28, 28, 28), Color.White);
                }


                if (spawningHydros) cutsceneTicks+=2;

                if(cutsceneTicks < 60 && oresGiven >= 2) //Default state
                {
                    int l = 1;

                    Vector2 orig1 = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f);

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * 4f;

                        spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1 + initRot.RotatedBy((Main.GameUpdateCount / 30f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.Gold * 0.5f);
                    }

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1, new Rectangle(0, frames[l] * 28, 28, 28), Color.White);

                    if (Main.rand.NextBool(30))
                    {
                        int orig = Main.rand.Next(oresGiven);
                        int target = orig + Main.rand.Next(1, oresGiven);

                        if (target >= oresGiven) target -= oresGiven;

                        int lightningproj = Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i * 16, j * 16) + positions[orig] + new Vector2(12, 12), Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), 20, 2.5f);

                        if (Main.netMode != NetmodeID.Server)
                        {
                            PrimSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj]));
                        }

                        TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                        zappy.target = new Vector2(i * 16, j * 16) + positions[target] + new Vector2(12, 12);
                    } //-96
                }
                else if(cutsceneTicks < 120 && oresGiven >= 2) //Raising up lythen ore
                {
                    Vector2 vec1 = new Vector2(52, -28);
                    Vector2 vec2 = new Vector2(52, -124);

                    int l = 1;

                    positions[l] = Vector2.SmoothStep(vec1, vec2, (cutsceneTicks - 60) / 60f);

                    Vector2 orig = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f);

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * 4f;

                        spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig + initRot.RotatedBy((Main.GameUpdateCount / 30f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.Gold * 0.5f);
                    }

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig, new Rectangle(0, frames[l] * 28, 28, 28), Color.White);
                } 
                else if(cutsceneTicks < 180 && oresGiven >= 2) //Starting to rapidly zap lythen ore
                {
                    int l = 1;

                    Vector2 orig1 = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f);

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * 4f;

                        spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1 + initRot.RotatedBy((Main.GameUpdateCount / 30f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.Gold * 0.5f);
                    }

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1, new Rectangle(0, frames[l] * 28, 28, 28), Color.White);

                    if (Main.rand.NextBool(10))
                    {
                        int target = 1;
                        int orig = target + Main.rand.Next(1, 3);

                        if (orig >= oresGiven) orig -= oresGiven;

                        int lightningproj = Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i * 16, j * 16) + positions[orig] + new Vector2(12, 12), Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), 20, 2.5f);

                        if (Main.netMode != NetmodeID.Server)
                        {
                            PrimSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj]));
                        }

                        TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                        zappy.target = new Vector2(i * 16, j * 16) + positions[target] + new Vector2(12, 12);
                    } //-96
                }
                else if (cutsceneTicks < 240 && oresGiven >= 2) //Lythen becomes lightning-ified
                {
                    int l = 1;

                    Vector2 orig1 = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f * (1 - ((cutsceneTicks - 180) / 60f)));

                    //Lythen layer

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    ApplyIntroShader(((cutsceneTicks - 180) / 60f), new Vector2(24, 24));

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * (1 - ((cutsceneTicks - 180) / 60f)) * 4f;

                        spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1 + initRot.RotatedBy((Main.GameUpdateCount / 30f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.White);
                    }


                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1, new Rectangle(0, frames[l] * 28, 28, 28), Color.White);
                    
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                   
                    if (Main.rand.NextBool(10))
                    {
                        int target = 1;
                        int orig = target + Main.rand.Next(1, 3);

                        if (orig >= oresGiven) orig -= oresGiven;

                        int lightningproj = Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i * 16, j * 16) + positions[orig] + new Vector2(12, 12), Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), 20, 2.5f);

                        if (Main.netMode != NetmodeID.Server)
                        {
                            PrimSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj]));
                        }

                        TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                        zappy.target = new Vector2(i * 16, j * 16) + positions[target] + new Vector2(12, 12);
                    } //-96
                }
                else if (cutsceneTicks < 300 && oresGiven >= 2) //Lythen switches to Hydros
                {
                    int l = 1;

                    Vector2 orig1 = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition;

                    Vector2 lightPoint = orig1 + new Vector2(12, 12) / 16f;

                    //Outline layer

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    ApplyIntroShader(1f, ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/HydrosOutline").Value.Bounds.Size(), 1f, true);

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/HydrosOutline").Value, orig1 + new Vector2(12, 12), null, Color.Gold, 0f, new Vector2(159, 83), (cutsceneTicks - 240) / 60f, SpriteEffects.None, 0f);

                    //Lythen layer

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    ApplyIntroShader(1 - ((cutsceneTicks - 240) / 60f), new Vector2(24, 24));

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig1, new Rectangle(0, frames[l] * 28, 28, 28), Color.White * (1 - ((cutsceneTicks - 240) / 20f)));

                    //Hydros layer 1

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    ApplyIntroShader(1f, ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value.Bounds.Size(), -1f);

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, orig1 + new Vector2(12, 12), null, Color.White, 0f, new Vector2(157, 81), (cutsceneTicks - 240) / 60f, SpriteEffects.None, 0f);

                    //Hydros layer 2

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    ApplyIntroShader(1f, ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value.Bounds.Size(), - 1f);

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, orig1 + new Vector2(12, 12), null, Color.White, 0f, new Vector2(157, 81), (cutsceneTicks - 240) / 60f, SpriteEffects.None, 0f);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                }
                else if (cutsceneTicks == 300)
                {
                    Vector2 pos = new Vector2(i * 16, j * 16) + positions[1] + new Vector2(12, 12) + new Vector2(0, 81);
                    NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<Hydros>());
                }
            }

            cooldown--;

            return true;
        }

        public void ApplyIntroShader(float lerpVal, Vector2 scale, float timeMultiplier = 1, bool invert = false)
        {
            EEMod.hydrosIntro.Parameters["newColor"].SetValue(new Vector4(1f, 1f, 0f, 1f));

            EEMod.hydrosIntro.Parameters["lerpVal"].SetValue(lerpVal);
            EEMod.hydrosIntro.Parameters["time"].SetValue((cutsceneTicks / 60f) * timeMultiplier * Vector2.One);
            EEMod.hydrosIntro.Parameters["thresh"].SetValue(lerpVal);
            EEMod.hydrosIntro.Parameters["invert"].SetValue(invert);
            EEMod.hydrosIntro.Parameters["frames"].SetValue(1);

            Main.NewText(lerpVal);

            EEMod.hydrosIntro.Parameters["noiseBounds"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelated").Value.Bounds.Size());
            EEMod.hydrosIntro.Parameters["imgBounds"].SetValue(scale);

            EEMod.hydrosIntro.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelated").Value);

            EEMod.hydrosIntro.CurrentTechnique.Passes[0].Apply();
        }

        public override bool RightClick(int i, int j)
        {
            if (!spawningHydros)
            {
                if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<LythenOre>() && cooldown <= 0)
                {
                    Main.LocalPlayer.HeldItem.stack--;

                    oresGiven++;

                    frames[oresGiven - 1] = Main.rand.Next(10);

                    cooldown = 10;

                    if (oresGiven >= 3)
                    {
                        spawningHydros = true;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}