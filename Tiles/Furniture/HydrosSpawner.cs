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
            if (Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 0 && oresGiven < 3) 
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
                    Vector2 orig = new Vector2(i * 16, j * 16) + positions[l] + zero - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f);

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * 4f;

                        spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig + initRot.RotatedBy((Main.GameUpdateCount / 30f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.Gold * 0.5f);
                    }

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig, new Rectangle(0, frames[l] * 28, 28, 28), Color.White);
                }
            }

            cooldown--;

            return true;
        }

        public override bool RightClick(int i, int j)
        {
            if (oresGiven < 3)
            {
                if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<LythenOre>() && cooldown <= 0)
                {
                    Main.LocalPlayer.HeldItem.stack--;

                    oresGiven++;

                    frames[oresGiven - 1] = Main.rand.Next(10);

                    cooldown = 10;

                    if (oresGiven >= 3)
                    {
                        Main.StartRain();

                        int manager = NPC.NewNPC((i * 16) - Framing.GetTileSafely(i, j).frameX + 64, (j * 16) - Framing.GetTileSafely(i, j).frameY, ModContent.NPCType<HydrosCutsceneManager>());
                        (Main.npc[manager].ModNPC as HydrosCutsceneManager).frames = frames;
                        (Main.npc[manager].ModNPC as HydrosCutsceneManager).positions = positions;
                        (Main.npc[manager].ModNPC as HydrosCutsceneManager).frameCounter = frameCounter;
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