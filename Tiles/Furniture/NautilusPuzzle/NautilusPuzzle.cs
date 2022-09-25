using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;
using EEMod.Projectiles.CoralReefs;

namespace EEMod.Tiles.Furniture.NautilusPuzzle
{
    public class NautilusPuzzle : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.Width = 9;
            TileObjectData.newTile.Height = 9;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();

            name.SetDefault("Shrine of the Water Rune");
            AddMapEntry(new Color(0, 120, 255), name);
            DisableSmartCursor = true;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Framing.GetTileSafely(i, j).TileFrameX == 0 && Framing.GetTileSafely(i, j).TileFrameY == 0)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (tiles[y, x] != 0)
                        {
                            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                            if (Main.drawToScreen)
                            {
                                zero = Vector2.Zero;
                            }

                            Texture2D tex = ModContent.Request<Texture2D>(path + "NautilusPuzzle" + tiles[y, x]).Value;
                            Vector2 pos = new Vector2((i * 16) + (x * 48), (j * 16) + (y * 48));

                            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, 48, 48);

                            cooldown++;

                            if (rect.Contains(Main.MouseWorld.ToPoint()))
                            {
                                selectedVal = tiles[y, x];
                                selectedX = x;
                                selectedY = y;

                                if (Main.LocalPlayer.controlUseItem && cooldown >= 5)
                                {
                                    if (y + 1 < 3 && tiles[y + 1, x] == 0)
                                    {
                                        int temp = tiles[y + 1, x];
                                        tiles[y + 1, x] = selectedVal;
                                        tiles[selectedY, selectedX] = temp;

                                        cooldown = 0;
                                        selectedVal = 0;
                                    }
                                    else if (y - 1 >= 0 && tiles[y - 1, x] == 0)
                                    {
                                        int temp = tiles[y - 1, x];
                                        tiles[y - 1, x] = selectedVal;
                                        tiles[selectedY, selectedX] = temp;

                                        cooldown = 0;
                                        selectedVal = 0;
                                    }
                                    else if (x + 1 < 3 && tiles[y, x + 1] == 0)
                                    {
                                        int temp = tiles[y, x + 1];
                                        tiles[y, x + 1] = selectedVal;
                                        tiles[selectedY, selectedX] = temp;

                                        cooldown = 0;
                                        selectedVal = 0;
                                    }
                                    else if (x - 1 >= 0 && tiles[y, x - 1] == 0)
                                    {
                                        int temp = tiles[y, x - 1];
                                        tiles[y, x - 1] = selectedVal;
                                        tiles[selectedY, selectedX] = temp;

                                        cooldown = 0;
                                        selectedVal = 0;
                                    }
                                }
                            }

                            Color color = Color.White;
                            if (!sussy)
                            {
                                if (tiles[y, x] == selectedVal) color = Color.SkyBlue;
                                else color = Color.Gray;
                            }
                            else
                            {
                                color = Color.Lerp(Color.Gray, Color.White, colorLerp);
                                colorLerp += 0.02f;
                            }

                            spriteBatch.Draw(tex, pos - Main.screenPosition + zero, new Rectangle(0, 0, 48, 48), color, 0f, default, 1f, SpriteEffects.None, 0f);
                        }
                    }
                }

                if (tiles[0, 0] == 1 &&
                    tiles[0, 1] == 2 &&
                    tiles[0, 2] == 0 &&
                    tiles[1, 0] == 4 &&
                    tiles[1, 1] == 5 &&
                    tiles[1, 2] == 6 &&
                    tiles[2, 0] == 7 &&
                    tiles[2, 1] == 8 &&
                    tiles[2, 2] == 9 && !sussy)
                {
                    sussy = true;
                    Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Vector2((i * 16) + 72, (j * 16) + 72), new Vector2(0, -2f), ModContent.ProjectileType<KelpFlowerItem>(), 0, 0f);
                }
            }
        }

        private int cooldown = 0;
        private bool sussy;
        private float colorLerp;

        private string path = "EEMod/Tiles/Furniture/NautilusPuzzle/";
        private int[,] tiles = new int[,] {
            { 2, 8, 6 },
            { 1, 0, 5 },
            { 4, 7, 9 }
        };

        private int selectedVal = 0;
        private int selectedX = 0;
        private int selectedY = 0;
    }
}