using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
using EEMod.Tiles;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;

namespace EEMod
{
    public partial class EEMod
    {
        private void LoadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float += Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.DoUpdate += Main_DoUpdate;
            On.Terraria.Main.Draw += Main_Draw;
            On.Terraria.Main.DrawBG += Main_DrawBG;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.DrawNPC += Main_DrawNPC;
            On.Terraria.Main.DrawWoF += Main_DrawWoF;
            On.Terraria.Main.DrawNPC += Main_DrawNPC1;
            On.Terraria.Main.CacheNPCDraws += Main_CacheNPCDraws;
            On.Terraria.Main.DrawGoreBehind += Main_DrawGoreBehind;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
            On.Terraria.Main.oldDrawWater += Main_oldDrawWater1;

            On.Terraria.WorldGen.SaveAndQuitCallBack += WorldGen_SaveAndQuitCallBack;
            On.Terraria.WorldGen.SmashAltar += WorldGen_SmashAltar;
        }

        private void Main_oldDrawWater1(On.Terraria.Main.orig_oldDrawWater orig, Main self, bool bg, int Style, float Alpha)
        {
            float num = 0f;
            float num2 = 99999f;
            float num3 = 99999f;
            int num4 = -1;
            int num5 = -1;
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            //new Color[4];
            int num6 = (int)(255f * (1f - Main.gfxQuality) + 40f * Main.gfxQuality);
            float num7 = Main.gfxQuality;
            float num8 = Main.gfxQuality;
            int num9 = (int)((Main.screenPosition.X - zero.X) / 16f - 1f);
            int num10 = (int)((Main.screenPosition.X + (float)Main.screenWidth + zero.X) / 16f) + 2;
            int num11 = (int)((Main.screenPosition.Y - zero.Y) / 16f - 1f);
            int num12 = (int)((Main.screenPosition.Y + (float)Main.screenHeight + zero.Y) / 16f) + 5;
            if (num9 < 5)
            {
                num9 = 5;
            }
            if (num10 > Main.maxTilesX - 5)
            {
                num10 = Main.maxTilesX - 5;
            }
            if (num11 < 5)
            {
                num11 = 5;
            }
            if (num12 > Main.maxTilesY - 5)
            {
                num12 = Main.maxTilesY - 5;
            }
            for (int i = num11; i < num12 + 4; i++)
            {
                for (int j = num9 - 2; j < num10 + 2; j++)
                {
                    if (Main.tile[j, i] == null)
                    {
                        Main.tile[j, i] = new Tile();
                    }
                    if (Main.tile[j, i].liquid > 0 || Main.tile[j, i].type == ModContent.TileType<EmptyTile>() &&  (!Main.tile[j, i].nactive() || !Main.tileSolid[(int)Main.tile[j, i].type] || Main.tileSolidTop[(int)Main.tile[j, i].type]) && (Lighting.Brightness(j, i) > 0f || bg))
                    {
                        Color color = Lighting.GetColor(j, i);
                        float num13 = (float)(256 - (int)Main.tile[j, i].liquid);
                        num13 /= 32f;
                        int num14 = 0;
                        if (Main.tile[j, i].lava())
                        {
                            if (Main.drewLava)
                            {
                                goto IL_E7B;
                            }
                            float num15 = Math.Abs((float)(j * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
                            float num16 = Math.Abs((float)(i * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
                            if (num15 < (float)(Main.screenWidth * 2) && num16 < (float)(Main.screenHeight * 2))
                            {
                                float num17 = (float)Math.Sqrt((double)(num15 * num15 + num16 * num16));
                                float num18 = 1f - num17 / ((float)Main.screenWidth * 0.75f);
                                if (num18 > 0f)
                                {
                                    num += num18;
                                }
                            }
                            if (num15 < num2)
                            {
                                num2 = num15;
                                num4 = j * 16 + 8;
                            }
                            if (num16 < num3)
                            {
                                num3 = num15;
                                num5 = i * 16 + 8;
                            }
                            num14 = 1;
                        }
                        else if (Main.tile[j, i].honey())
                        {
                            num14 = 11;
                        }
                        if (num14 == 0)
                        {
                            num14 = Style;
                        }
                        if ((num14 != 1 && num14 != 11) || !Main.drewLava)
                        {
                            float num19 = 0.5f;
                            if (bg)
                            {
                                num19 = 1f;
                            }
                            if (num14 != 1 && num14 != 11)
                            {
                                num19 *= Alpha;
                            }
                            Vector2 vector = new Vector2((float)(j * 16), (float)(i * 16 + (int)num13 * 2));
                            Rectangle rectangle = new Rectangle(0, 0, 16, 16 - (int)num13 * 2);
                            if (Main.tile[j, i + 1].liquid < 245 && (!Main.tile[j, i + 1].nactive() || !Main.tileSolid[(int)Main.tile[j, i + 1].type] || Main.tileSolidTop[(int)Main.tile[j, i + 1].type] || Main.tile[j, i].type == ModContent.TileType<EmptyTile>()))
                            {
                                float num20 = (float)(256 - (int)Main.tile[j, i + 1].liquid);
                                num20 /= 32f;
                                num19 = 0.5f * (8f - num13) / 4f;
                                if ((double)num19 > 0.55)
                                {
                                    num19 = 0.55f;
                                }
                                if ((double)num19 < 0.35)
                                {
                                    num19 = 0.35f;
                                }
                                float num21 = num13 / 2f;
                                if (Main.tile[j, i + 1].liquid < 200)
                                {
                                    if (bg)
                                    {
                                        goto IL_E7B;
                                    }
                                    if (Main.tile[j, i - 1].liquid > 0 && Main.tile[j, i - 1].liquid > 0)
                                    {
                                        rectangle = new Rectangle(0, 4, 16, 16);
                                        num19 = 0.5f;
                                    }
                                    else if (Main.tile[j, i - 1].liquid > 0)
                                    {
                                        vector = new Vector2((float)(j * 16), (float)(i * 16 + 4));
                                        rectangle = new Rectangle(0, 4, 16, 12);
                                        num19 = 0.5f;
                                    }
                                    else if (Main.tile[j, i + 1].liquid <= 0)
                                    {
                                        vector = new Vector2((float)(j * 16 + (int)num21), (float)(i * 16 + (int)num21 * 2 + (int)num20 * 2));
                                        rectangle = new Rectangle(0, 4, 16 - (int)num21 * 2, 16 - (int)num21 * 2);
                                    }
                                    else
                                    {
                                        vector = new Vector2((float)(j * 16), (float)(i * 16 + (int)num13 * 2 + (int)num20 * 2));
                                        rectangle = new Rectangle(0, 4, 16, 16 - (int)num13 * 2);
                                    }
                                }
                                else
                                {
                                    num19 = 0.5f;
                                    rectangle = new Rectangle(0, 4, 16, 16 - (int)num13 * 2 + (int)num20 * 2);
                                }
                            }
                            else if (Main.tile[j, i - 1].liquid > 32)
                            {
                                rectangle = new Rectangle(0, 4, rectangle.Width, rectangle.Height);
                            }
                            else if (num13 < 1f && Main.tile[j, i - 1].nactive() && Main.tileSolid[(int)Main.tile[j, i - 1].type] && !Main.tileSolidTop[(int)Main.tile[j, i - 1].type] && Main.tile[j, i].type != ModContent.TileType<EmptyTile>())
                            {
                                vector = new Vector2((float)(j * 16), (float)(i * 16));
                                rectangle = new Rectangle(0, 4, 16, 16);
                            }
                            else
                            {
                                bool flag = true;
                                int num22 = i + 1;
                                while (num22 < i + 6 && (!Main.tile[j, num22].nactive() || !Main.tileSolid[(int)Main.tile[j, num22].type] || Main.tileSolidTop[(int)Main.tile[j, num22].type]) && Main.tile[j, i].type != ModContent.TileType<EmptyTile>())
                                {
                                    if (Main.tile[j, num22].liquid < 200)
                                    {
                                        flag = false;
                                        break;
                                    }
                                    num22++;
                                }
                                if (!flag)
                                {
                                    num19 = 0.5f;
                                    rectangle = new Rectangle(0, 4, 16, 16);
                                }
                                else if (Main.tile[j, i - 1].liquid > 0)
                                {
                                    rectangle = new Rectangle(0, 2, rectangle.Width, rectangle.Height);
                                }
                            }
                            if ((color.R > 20 || color.B > 20 || color.G > 20) && rectangle.Y < 4)
                            {
                                int num23 = (int)color.R;
                                if ((int)color.G > num23)
                                {
                                    num23 = (int)color.G;
                                }
                                if ((int)color.B > num23)
                                {
                                    num23 = (int)color.B;
                                }
                                num23 /= 30;
                                if (Main.rand.Next(20000) < num23)
                                {
                                    Color newColor = new Color(255, 255, 255);
                                    if (Main.tile[j, i].honey())
                                    {
                                        newColor = new Color(255, 255, 50);
                                    }
                                    int num24 = Dust.NewDust(new Vector2((float)(j * 16), vector.Y - 2f), 16, 8, 43, 0f, 0f, 254, newColor, 0.75f);
                                    Main.dust[num24].velocity *= 0f;
                                }
                            }
                            if (Main.tile[j, i].honey())
                            {
                                num19 *= 1.6f;
                                if (num19 > 1f)
                                {
                                    num19 = 1f;
                                }
                            }
                            if (Main.tile[j, i].lava())
                            {
                                num19 *= 1.8f;
                                if (num19 > 1f)
                                {
                                    num19 = 1f;
                                }
                                if (self.IsActive && !Main.gamePaused && Dust.lavaBubbles < 200)
                                {
                                    if (Main.tile[j, i].liquid > 200 && Main.rand.Next(700) == 0)
                                    {
                                        Dust.NewDust(new Vector2((float)(j * 16), (float)(i * 16)), 16, 16, 35, 0f, 0f, 0, default(Color), 1f);
                                    }
                                    if (rectangle.Y == 0 && Main.rand.Next(350) == 0)
                                    {
                                        int num25 = Dust.NewDust(new Vector2((float)(j * 16), (float)(i * 16) + num13 * 2f - 8f), 16, 8, 35, 0f, 0f, 50, default(Color), 1.5f);
                                        Main.dust[num25].velocity *= 0.8f;
                                        Dust dust = Main.dust[num25];
                                        dust.velocity.X = dust.velocity.X * 2f;
                                        Dust dust2 = Main.dust[num25];
                                        dust2.velocity.Y = dust2.velocity.Y - (float)Main.rand.Next(1, 7) * 0.1f;
                                        if (Main.rand.Next(10) == 0)
                                        {
                                            Dust dust3 = Main.dust[num25];
                                            dust3.velocity.Y = dust3.velocity.Y * (float)Main.rand.Next(2, 5);
                                        }
                                        Main.dust[num25].noGravity = true;
                                    }
                                }
                            }
                            float num26 = (float)color.R * num19;
                            float num27 = (float)color.G * num19;
                            float num28 = (float)color.B * num19;
                            float num29 = (float)color.A * num19;
                            color = new Color((int)((byte)num26), (int)((byte)num27), (int)((byte)num28), (int)((byte)num29));
                            if (Lighting.NotRetro && !bg)
                            {
                                Color color2 = color;
                                if (num14 != 1 && ((double)color2.R > (double)num6 * 0.6 || (double)color2.G > (double)num6 * 0.65 || (double)color2.B > (double)num6 * 0.7))
                                {
                                    for (int k = 0; k < 4; k++)
                                    {
                                        int num30 = 0;
                                        int num31 = 0;
                                        int width = 8;
                                        int height = 8;
                                        Color color3 = color2;
                                        Color color4 = Lighting.GetColor(j, i);
                                        if (k == 0)
                                        {
                                            color4 = Lighting.GetColor(j - 1, i - 1);
                                            if (rectangle.Height < 8)
                                            {
                                                height = rectangle.Height;
                                            }
                                        }
                                        if (k == 1)
                                        {
                                            color4 = Lighting.GetColor(j + 1, i - 1);
                                            num30 = 8;
                                            if (rectangle.Height < 8)
                                            {
                                                height = rectangle.Height;
                                            }
                                        }
                                        if (k == 2)
                                        {
                                            color4 = Lighting.GetColor(j - 1, i + 1);
                                            num31 = 8;
                                            height = 8 - (16 - rectangle.Height);
                                        }
                                        if (k == 3)
                                        {
                                            color4 = Lighting.GetColor(j + 1, i + 1);
                                            num30 = 8;
                                            num31 = 8;
                                            height = 8 - (16 - rectangle.Height);
                                        }
                                        num26 = (float)color4.R * num19;
                                        num27 = (float)color4.G * num19;
                                        num28 = (float)color4.B * num19;
                                        num29 = (float)color4.A * num19;
                                        color4 = new Color((int)((byte)num26), (int)((byte)num27), (int)((byte)num28), (int)((byte)num29));
                                        color3.R = (byte)((color2.R * 3 + color4.R * 2) / 5);
                                        color3.G = (byte)((color2.G * 3 + color4.G * 2) / 5);
                                        color3.B = (byte)((color2.B * 3 + color4.B * 2) / 5);
                                        color3.A = (byte)((color2.A * 3 + color4.A * 2) / 5);
                                        Main.spriteBatch.Draw(Main.liquidTexture[num14], vector - Main.screenPosition + new Vector2((float)num30, (float)num31) + zero, new Rectangle?(new Rectangle(rectangle.X + num30, rectangle.Y + num31, width, height)), color3, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                    }
                                }
                                else
                                {
                                    Main.spriteBatch.Draw(Main.liquidTexture[num14], vector - Main.screenPosition + zero, new Rectangle?(rectangle), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                }
                            }
                            else
                            {
                                if (rectangle.Y < 4)
                                {
                                    rectangle.X += (int)(Main.wFrame * 18f);
                                }
                                Main.spriteBatch.Draw(Main.liquidTexture[num14], vector - Main.screenPosition + zero, new Rectangle?(rectangle), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                            }
                            if (Main.tile[j, i + 1].halfBrick())
                            {
                                color = Lighting.GetColor(j, i + 1);
                                num26 = (float)color.R * num19;
                                num27 = (float)color.G * num19;
                                num28 = (float)color.B * num19;
                                num29 = (float)color.A * num19;
                                color = new Color((int)((byte)num26), (int)((byte)num27), (int)((byte)num28), (int)((byte)num29));
                                vector = new Vector2((float)(j * 16), (float)(i * 16 + 16));
                                Main.spriteBatch.Draw(Main.liquidTexture[num14], vector - Main.screenPosition + zero, new Rectangle?(new Rectangle(0, 4, 16, 8)), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                            }
                        }
                    }
                IL_E7B:;
                }
            }
            if (!Main.drewLava)
            {
                Main.ambientLavaX = (float)num4;
                Main.ambientLavaY = (float)num5;
                Main.ambientLavaStrength = num;
            }
            Main.drewLava = true;
        }

        private void UnloadDetours()
        {
            On.Terraria.Main.CacheNPCDraws -= Main_CacheNPCDraws;
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.DoUpdate -= Main_DoUpdate;
            On.Terraria.Main.DrawNPC -= Main_DrawNPC;
            On.Terraria.Main.Draw -= Main_Draw;
            On.Terraria.Main.DrawBG -= Main_DrawBG;
            On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
            On.Terraria.Main.DrawWoF -= Main_DrawWoF;
            On.Terraria.Main.DrawNPC -= Main_DrawNPC1;
            On.Terraria.Main.DrawGoreBehind -= Main_DrawGoreBehind;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor -= UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf -= UIWorldListItem_DrawSelf;
            On.Terraria.WorldGen.SaveAndQuitCallBack -= WorldGen_SaveAndQuitCallBack;
            On.Terraria.WorldGen.SmashAltar -= WorldGen_SmashAltar;
        }
        private void Main_CacheNPCDraws(On.Terraria.Main.orig_CacheNPCDraws orig, Main self)
        {
            //DrawSpiderPort();
            orig(self);
        }
        private void Main_DrawGoreBehind(On.Terraria.Main.orig_DrawGoreBehind orig, Main self)
        {
            orig(self);
        }
        private void Main_DrawNPC1(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
        {
            orig(self, iNPCIndex, behindTiles);
        }
        private void WorldGen_SmashAltar(On.Terraria.WorldGen.orig_SmashAltar orig, int i, int j)
        {
            orig(i, j);

            EEPlayer.moralScore -= 50;
            Main.NewText(EEPlayer.moralScore);
        }

        private void WorldGen_SaveAndQuitCallBack(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadContext)
        {
            isSaving = true;

            orig(threadContext);

            isSaving = false;

            //saveInterface?.SetState(null);
        }

        private void UIWorldListItem_DrawSelf(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);

            WorldFileData data = (WorldFileData)typeof(Main).Assembly.GetType("Terraria.GameContent.UI.Elements.UIWorldListItem").GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            UIImage worldIcon = (UIImage)typeof(Main).Assembly.GetType("Terraria.GameContent.UI.Elements.UIWorldListItem").GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            CalculatedStyle innerDimensions = self.GetInnerDimensions();
            CalculatedStyle dimensions = worldIcon.GetDimensions();
            float num = 56f;

            if (SocialAPI.Cloud != null)
            {
                num += 24f;
            }

            if (data.WorldGeneratorVersion != 0L)
            {
                num += 24f;
            }

            float num2 = dimensions.X + num;

            Vector2 position = new Vector2(num2, innerDimensions.Y + 59);
            float width = 200;

            spriteBatch.Draw(TextureManager.Load("Images/UI/InnerPanelBackground"), position, new Rectangle(0, 0, 8, TextureManager.Load("Images/UI/InnerPanelBackground").Height), Color.White);
            spriteBatch.Draw(TextureManager.Load("Images/UI/InnerPanelBackground"), new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, TextureManager.Load("Images/UI/InnerPanelBackground").Height), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(TextureManager.Load("Images/UI/InnerPanelBackground"), new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, TextureManager.Load("Images/UI/InnerPanelBackground").Height), Color.White);
        }

        private void UIWorldListItem_ctor(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, Terraria.GameContent.UI.Elements.UIWorldListItem self, Terraria.IO.WorldFileData data, int snapPointIndex)
        {
            orig(self, data, snapPointIndex);

            float num = 56f;

            if (SocialAPI.Cloud != null)
            {
                num += 24f;
            }

            if (data.WorldGeneratorVersion != 0L)
            {
                num += 24f;
            }

            UIText buttonLabel = new UIText("")
            {
                VAlign = 1f
            };
            buttonLabel.Left.Set(num + 210f, 0f);
            buttonLabel.Top.Set(-3f, 0f);

            typeof(Main).Assembly.GetType("Terraria.GameContent.UI.Elements.UIWorldListItem").GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, buttonLabel);

            self.Append(buttonLabel);
        }

        //CHAD NAME
        private int Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
        {
            int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);

            if (Main.netMode != NetmodeID.Server)
            {
                trailManager.DoTrailCreation(Main.projectile[index]);
            }

            return index;
        }
        void HandleCrystalDraw(Vector2 position)
        {
            Texture2D tex = instance.GetTexture("Tiles/EmptyTileArrays/CoralCrystal");
            Rectangle mouseBox = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 3, 3);
            Rectangle crystalBox = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            Main.spriteBatch.Draw(tex, new Rectangle((int)position.ForDraw().X, (int)position.ForDraw().Y, tex.Width, tex.Height), new Rectangle(0, 0, tex.Width, tex.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawKelpTarzanVines()
        {
            foreach (int index in VerletHelpers.EndPointChains)
            {
                var vec = Verlet.Points[index].point;
                if ((vec - Main.LocalPlayer.Center).LengthSquared() < 100 * 100)
                {
                    float lerp = 1f - (vec - Main.LocalPlayer.Center).LengthSquared() / (100 * 100);

                    if (Inspect.Current)
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
                        if (Inspect.JustPressed)
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
                Lighting.AddLight(vec, new Vector3(235, 166, 0) / 255);
            }

            #region Spawning particles
            if (bufferVariable != Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine)
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().isHangingOnVine)
                {
                    Particles.Get("Main").SetSpawningModules(new SpawnRandomly(1f));
                    for (int i = 0; i < 20; i++)
                    {
                        Particles.Get("Main").SpawnParticles(Main.LocalPlayer.Center, null, 1, Color.White, new Spew(6.14f, 1f, Vector2.One / 2f, 0.98f));
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

            #region Player movement
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
            #endregion
        }


        Vector2[] jointPoints;
        Vector2[] legPoints;
        bool[] CanMove;
        int[] CoolDown;
        Vector2 SpiderBodyPosition;
        int numberOfLegs = 6;
        int seperationOfLegs = 15;
        int lengthOfUpperLeg = 30;
        int lengthOfLowerLeg = 30;
        int legVert = 25;
        int jointElevation = 5;
        float accell = 0.03f;
        float velocityOfSpider;
        float VertVel;
        bool OnGround;
        public void UpdateSpiderPort()
        {
            if (SpiderBodyPosition == Vector2.Zero)
            {
                SpiderBodyPosition = Main.LocalPlayer.Center;
            }
            jointPoints = jointPoints ?? new Vector2[numberOfLegs];
            legPoints = legPoints ?? new Vector2[numberOfLegs];
            CanMove = CanMove ?? new bool[numberOfLegs];
            CoolDown = CoolDown ?? new int[numberOfLegs];
            for (int i = 0; i < numberOfLegs; i++)
            {
                legPoints[i] = legPoints[i] == Vector2.Zero ? new Vector2(Main.LocalPlayer.Center.X + (i - numberOfLegs * 0.5f + 0.5f) * seperationOfLegs, Main.LocalPlayer.Center.Y + legVert) : legPoints[i];
                jointPoints[i] = jointPoints[i] == Vector2.Zero ? new Vector2(Main.LocalPlayer.Center.X + (i - numberOfLegs * 0.5f + 0.5f) * seperationOfLegs, Main.LocalPlayer.Center.Y + legVert) : jointPoints[i];
            }
            if (Main.LocalPlayer.controlRight)
            {
                velocityOfSpider += accell;
            }
            if (Main.LocalPlayer.controlLeft)
            {
                velocityOfSpider -= accell;
            }

            velocityOfSpider *= 0.99f;
            SpiderBodyPosition.X += velocityOfSpider;
            float absVel = Math.Abs(velocityOfSpider);
            for (int i = 0; i < numberOfLegs; i++)
            {
                jointPoints[i] = CorrectLeg(SpiderBodyPosition, jointPoints[i])[1];
                jointPoints[i] = CorrectLeg(legPoints[i], jointPoints[i])[1];
                jointPoints[i].Y -= jointElevation;
                float dx = legPoints[i].X - jointPoints[i].X;
                float dy = legPoints[i].Y - jointPoints[i].Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                float TrueY = SpiderBodyPosition.Y + legVert;
                float TrueX = SpiderBodyPosition.X + (i - numberOfLegs * 0.5f + 0.5f) * seperationOfLegs + velocityOfSpider * 20;
                int Pogger = 0;

                if (WorldGen.InWorld((int)(TrueX / 16), (int)(TrueY / 16), 20))
                {
                    while (Main.tileSolid[Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).type] && Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).active() && Pogger < 64 && Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).slope() == 0)
                    {
                        SpiderBodyPosition.Y -= 0.005f;
                        TrueY--;
                        Pogger++;
                    }
                    while ((!Main.tileSolid[Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).type] || !Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).active()) && Pogger < 32)
                    {
                        SpiderBodyPosition.Y += 0.005f;
                        TrueY++;
                        Pogger++;
                    }
                }
                Vector2 Goto = new Vector2(TrueX, TrueY);
                float dx2 = legPoints[i].X - Goto.X;
                float dy2 = legPoints[i].Y - Goto.Y;
                float distToGoto = (float)Math.Sqrt(dx2 * dx2 + dy2 * dy2);
                if (dist > lengthOfLowerLeg)
                {
                    if (CoolDown[i] > 0)
                        CoolDown[i]--;
                    if (CoolDown[i] <= 0)
                        CanMove[i] = true;
                    legPoints[i].Y += (Goto.Y - legPoints[i].Y) / 6f;
                }
                if (CanMove[i])
                {
                    float factor = 4f;
                    if (CoolDown[i] > 0)
                        CoolDown[i]--;
                    float xCompletion = Math.Abs(Goto.X - legPoints[i].X);
                    legPoints[i].X += (Goto.X - legPoints[i].X) / factor;
                    legPoints[i].Y += (Goto.Y - legPoints[i].Y) / factor - (float)Math.Sin(xCompletion / 20f) * 7;
                    if (distToGoto < (factor * absVel + 4f + absVel))
                    {
                        CoolDown[i] = Main.rand.Next((int)(20 - absVel * 3), (int)(40 + absVel * 3));
                        CanMove[i] = false;
                    }
                }
            }
            float UnderSpiderY = SpiderBodyPosition.Y + legVert;
            float UnderSpiderX = SpiderBodyPosition.X;
            bool OnGroundBuffer = OnGround;
            if (!Main.tileSolid[Framing.GetTileSafely((int)UnderSpiderX / 16, (int)UnderSpiderY / 16).type] || !Framing.GetTileSafely((int)UnderSpiderX / 16, (int)UnderSpiderY / 16).active())
            {
                VertVel += 0.2f;
                OnGround = false;
            }
            else
            {
                VertVel -= 0.1f;
                OnGround = true;
            }
            if (OnGroundBuffer != OnGround && OnGround)
            {
                VertVel = 0;
            }
            VertVel *= 0.95f;
            SpiderBodyPosition.Y += VertVel;

            Vector2[] CorrectLeg(Vector2 feetVec, Vector2 jointVec)
            {
                float dx = feetVec.X - jointVec.X;
                float dy = feetVec.Y - jointVec.Y;
                float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
                float deltaLength = currentLength - lengthOfUpperLeg;
                float perc = (deltaLength / (float)currentLength) * 0.5f;
                float offsetX = perc * dx;
                float offsetY = perc * dy;
                Vector2 F = new Vector2(feetVec.X + offsetX, feetVec.Y + offsetY);
                Vector2 J = new Vector2(jointVec.X + offsetX, jointVec.Y + offsetY);

                return new Vector2[] { F, J };
            }
        }
        public void DrawSpiderPort()
        {
            for (int i = 0; i < numberOfLegs; i++)
            {
                Helpers.DrawLine(new Vector2(SpiderBodyPosition.X, SpiderBodyPosition.Y).ForDraw(), new Vector2(jointPoints[i].X, jointPoints[i].Y).ForDraw(), Color.White, 1f);
                Helpers.DrawLine(new Vector2(jointPoints[i].X, jointPoints[i].Y).ForDraw(), new Vector2(legPoints[i].X, legPoints[i].Y).ForDraw(), Color.White, 1f);
            }
        }


        void HandleBulbDraw(Vector2 position)
        {
            Lighting.AddLight(position, new Vector3(0, 0.1f, 0.4f));
            Vector2 tilePos = position / 16;
            int spread = 8;
            int down = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X, (int)tilePos.Y, 1, 50);
            int up = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X, (int)tilePos.Y, -1, 50);
            int down2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, 1, 50);
            int up2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, -1, 50);
            int down3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, 1, 50);
            int up3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, -1, 50);
            Vector2 p1 = new Vector2(tilePos.X * 16, down * 16);
            Vector2 p2 = new Vector2(tilePos.X * 16, up * 16);
            Vector2 p3 = new Vector2((tilePos.X - spread) * 16, down2 * 16);
            Vector2 p4 = new Vector2((tilePos.X - spread) * 16, up2 * 16);
            Vector2 p5 = new Vector2((tilePos.X + spread) * 16, down3 * 16);
            Vector2 p6 = new Vector2((tilePos.X + spread) * 16, up3 * 16);
            Texture2D BlueLight = instance.GetTexture("Projectiles/LightBlue");
            Texture2D vineTexture = instance.GetTexture("Projectiles/BigVine");
            float Addon = 10;
            float cockandbol = 0.8f;
            float bolandcock = 7f;
            if (p1.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p1, position + new Vector2(0, 65), Vector2.Lerp(p1, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p1 + new Vector2(0, Addon), position + new Vector2(0, 65 + Addon), Vector2.Lerp(p1, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p2.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p2, position + new Vector2(0, -65), Vector2.Lerp(p2, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.5f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p2 + new Vector2(0, Addon), position + new Vector2(0, -65 + Addon), Vector2.Lerp(p2, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.5f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p3.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p3, position + new Vector2(-60, 55), Vector2.Lerp(p3, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.2f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p3 + new Vector2(0, Addon), position + new Vector2(-60, 55 + Addon), Vector2.Lerp(p3, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.2f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p4.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p4, position + new Vector2(-60, -55), Vector2.Lerp(p4, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.8f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p4 + new Vector2(0, Addon), position + new Vector2(-60, -55 + Addon), Vector2.Lerp(p4, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.8f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p5.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p5, position + new Vector2(60, 55), Vector2.Lerp(p5, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.9f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p5 + new Vector2(0, Addon), position + new Vector2(60, 55 + Addon), Vector2.Lerp(p5, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.9f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p6.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p6, position + new Vector2(60, -55), Vector2.Lerp(p6, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2.2f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p6 + new Vector2(0, Addon), position + new Vector2(60, -55 + Addon), Vector2.Lerp(p6, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2.2f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Noise2DShift.Parameters["noiseTexture"].SetValue(instance.GetTexture("Noise/noise"));
            Noise2DShift.Parameters["tentacle"].SetValue(instance.GetTexture("Noise/WormNoisePixelated"));
            Noise2DShift.Parameters["yCoord"].SetValue((float)Math.Sin(sineInt) * 0.2f);
            Noise2DShift.Parameters["xCoord"].SetValue((float)Math.Cos(sineInt) * 0.2f);

            Noise2DShift.CurrentTechnique.Passes[0].Apply();


            Noise2DShift.Parameters["lightColour"].SetValue(Lighting.GetColor((int)tilePos.X, (int)tilePos.Y).ToVector3());
            Texture2D tex = instance.GetTexture("ShaderAssets/BulbousBall");

            Main.spriteBatch.Draw(tex, new Rectangle((int)position.ForDraw().X, (int)position.ForDraw().Y + (int)(Math.Sin(sineInt * 4) * 10), tex.Width + (int)(Math.Sin(sineInt) * 10), tex.Height + (int)Math.Cos(sineInt) * 10), new Rectangle(0, 0, tex.Width + (int)Math.Sin(sineInt) * 10, tex.Height + (int)Math.Cos(sineInt) * 10), Color.White * 0, (float)Math.Sin(sineInt), tex.Bounds.Size() / 2, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);


        }
        private void Main_DrawWoF(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {

            //UpdateLight();
            DrawKelpTarzanVines();
            verlet.GlobalRenderPoints();
            DrawNoiseSurfacing();
            DrawLensFlares();
            DrawCoralReefsBg();
            for (int i = 0; i < EESubWorlds.BulbousTreePosition.Count; i++)
            {
                if ((EESubWorlds.BulbousTreePosition[i] * 16 - Main.LocalPlayer.Center).LengthSquared() < 2000 * 2000)
                    HandleBulbDraw(EESubWorlds.BulbousTreePosition[i] * 16);
            }
            for (int i = 0; i < EESubWorlds.CoralCrystalPosition.Count; i++)
            {
                //  if ((EESubWorlds.CoralCrystalPosition[i] * 16 - Main.LocalPlayer.Center).LengthSquared() < 2000 * 2000)
                //HandleCrystalDraw(EESubWorlds.CoralCrystalPosition[i] * 16);
            }
            if (Main.worldName == KeyID.CoralReefs)
            {
                EEWorld.EEWorld.instance.DrawVines();
            }
            EmptyTileEntityCache.Update();
            EmptyTileEntityCache.Draw();
            //Main.spriteBatch.Draw(Main.magicPixel, ChangingPoints.ForDraw(), Color.Red);

            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ZoneCoralReefs)
            {
                _alphaBG += (1 - _alphaBG) / 64f;
            }
            else
            {
                _alphaBG -= _alphaBG / 64f;
            }

            for (int i = 0; i < 400; i++)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].modProjectile is Gradient a)
                    {
                        a.pixelPlacmentHours();
                    }
                    if (Main.projectile[i].modProjectile is CyanoburstTomeKelp aa)
                    {
                        aa.DrawBehind();
                    }
                }
            }

            if (NPC.AnyNPCs(ModContent.NPCType<TentacleEdgeHandler>()))
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<TentacleEdgeHandler>())
                    {
                        (Main.npc[i].modNPC as TentacleEdgeHandler).DrawTentacleBeziers();
                    }
                }
            }

            orig(self);
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            trailManager.DrawTrails(Main.spriteBatch);

            orig(self);
        }
        private void Main_DrawNPC(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCTiles, bool behindTiles)
        {
            prims.DrawTrails(Main.spriteBatch);
            orig(self, iNPCTiles, behindTiles);
        }
        private void Main_DrawBG(On.Terraria.Main.orig_DrawBG orig, Main self)
        {
            if (EEModConfigClient.Instance.BetterLighting && !Main.gameMenu)
            {
                BetterLightingHandler();
                DrawGlobalShaderTextures();
            }
            else
            {
                UnloadShaderAssets();
            }

            orig(self);
        }

        private void Main_Draw(On.Terraria.Main.orig_Draw orig, Main self, Microsoft.Xna.Framework.GameTime gameTime)
        {
            orig(self, gameTime);

            if (EEModConfigClient.Instance.EEDebug)
            {
                Main.spriteBatch.Begin();

                Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug MenuMode: " + Main.menuMode.ToString(), new Vector2(50, 60), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

                if (!Main.gameMenu)
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Velocity X: " + Main.LocalPlayer.velocity.X.ToString(), new Vector2(50, 80), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Velocity Y: " + Main.LocalPlayer.velocity.Y.ToString(), new Vector2(50, 100), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Position X: " + Main.LocalPlayer.Center.Y.ToString(), new Vector2(50, 120), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Position Y: " + Main.LocalPlayer.Center.X.ToString(), new Vector2(50, 140), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Tile Pos X: " + ((int)Main.LocalPlayer.Center.Y / 16).ToString(), new Vector2(50, 160), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Tile Pos Y: " + ((int)Main.LocalPlayer.Center.X / 16).ToString(), new Vector2(50, 180), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Time: " + Main.time.ToString(), new Vector2(50, 200), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
            }

            if (isSaving && Main.gameMenu)
            {
                alpha += 0.01f;
                if (lerp != 1)
                    lerp += (1 - lerp) / 16f;
                if (lerp > 0.99f)
                {
                    lerp = 1;
                }
                if (alpha > 1)
                {
                    alpha = 1;
                }

                velocity = Vector2.Zero;
                Main.numClouds = 0;
                Main.logo2Texture = EEMod.instance.GetTexture("Empty");
                Main.logoTexture = EEMod.instance.GetTexture("Empty");
                Main.sun2Texture = EEMod.instance.GetTexture("Empty");
                Main.sun3Texture = EEMod.instance.GetTexture("Empty");
                Main.sunTexture = EEMod.instance.GetTexture("Empty");

                if (SkyManager.Instance["EEMod:SavingCutscene"] != null)
                {
                    SkyManager.Instance.Activate("EEMod:SavingCutscene", default, new object[0]);
                }

                if (loadingFlag)
                {
                    loadingChoose = Main.rand.Next(68);
                    loadingChooseImage = Main.rand.Next(5);
                    loadingFlag = false;
                }

                switch (loadingChoose)
                {
                    case 0:
                        screenMessageText = "Europium Phosphorus Iodine Cabon Chromium Oxygen Tungsten Nitrogen Potassium Indium Gadolinium";
                        break;

                    case 1:
                        screenMessageText = "All good sprites made by Nomis";
                        break;

                    case 2:
                        screenMessageText = "Tip of the Day: Loading screens are useless!";
                        break;

                    case 3:
                        screenMessageText = "Fear the MS Paint cat";
                        break;

                    case 4:
                        screenMessageText = "Terraria sprites need outlines... except when I make them";
                        break;

                    case 5:
                        screenMessageText = "Remove the banding";
                        break;

                    case 6:
                        screenMessageText = Main.LocalPlayer.name + " ... huh? What a cruddy name";
                        break;

                    case 7:
                        screenMessageText = "Don't ping everyone you big dumb stupid";
                        break;

                    case 8:
                        screenMessageText = "I'm nothing without attention";
                        break;

                    case 9:
                        screenMessageText = "Why are you even reading this?";
                        break;

                    case 10:
                        screenMessageText = "We actually think we're funny";
                        break;

                    case 11:
                        screenMessageText = "Interitos... what's that?";
                        break;

                    case 12:
                        screenMessageText = "It's my style";
                        break;

                    case 13:
                        screenMessageText = "Now featuring 50% more monkey per chimp!";
                        break;

                    case 14:
                        screenMessageText = "im angy";
                        break;

                    case 15:
                        screenMessageText = "All good music made by A44";
                        break;

                    case 16:
                        screenMessageText = "Mod is not edgy, I swear!";
                        break;

                    case 17:
                        screenMessageText = "All good art made by cynik";
                        break;

                    case 18:
                        screenMessageText = "I'm gonna have to mute you for that";
                        break;

                    case 19:
                        screenMessageText = "Gamers, rise up!";
                        break;

                    case 20:
                        screenMessageText = "THAT'S NOT THE CONCEPT";
                        break;

                    case 21:
                        screenMessageText = "caramel popcorn and celeste";
                        break;

                    case 22:
                        screenMessageText = "D D D A G# G F D F G";
                        break;

                    case 23:
                        screenMessageText = "We live in a society";
                        break;

                    case 24:
                        screenMessageText = "Don't mine at night!";
                        break;

                    case 25:
                        screenMessageText = "deleting system32...";
                        break;

                    case 26:
                        screenMessageText = "Sans in real!";
                        break;

                    case 27:
                        screenMessageText = "I sure hope I didnt break the codeghsduighshsy";
                        break;

                    case 28:
                        screenMessageText = "If you like Endless Escapade, you'll love Endless Escapade Premium (patent pending)!";
                        break;

                    case 29:
                        screenMessageText = "When\n\nBottomText";
                        break;

                    case 30:
                        screenMessageText = "mario in real life";
                        break;

                    case 31:
                        screenMessageText = "All good concept art made by phanta";
                        break;

                    case 32:
                        screenMessageText = "EEMod Foretold? More like doesn't exist";
                        break;

                    case 33:
                        screenMessageText = "You think this is a game? Look behind you 0_0";
                        break;

                    case 34:
                        screenMessageText = "Respect the drip Karen";
                        break;

                    case 35:
                        screenMessageText = "phosh";
                        break;

                    case 36:
                        screenMessageText = "All good sprites made by daimgamer";
                        break;

                    case 37:
                        screenMessageText = "All good music made by Universe";
                        break;

                    case 38:
                        screenMessageText = "All good sprites made by Vadim";
                        break;

                    case 39:
                        screenMessageText = "All good sprites made by Zarn";
                        break;

                    case 40:
                        screenMessageText = "All good builds made by Cherry";
                        break;

                    case 41:
                        screenMessageText = "Haha funny mod go brrr";
                        break;

                    case 42:
                        screenMessageText = "Do a Barrel Roll";
                        break;

                    case 43:
                        screenMessageText = "The man behind the laughter";
                        break;

                    case 44:
                        screenMessageText = "All good weapons made by Graydee";
                        break;

                    case 45:
                        screenMessageText = "An apple a day keeps the errors away!";
                        break;

                    case 46:
                        screenMessageText = "Poggers? Poggers.";
                        break;

                    case 47:
                        screenMessageText = $"By the way, {Main.LocalPlayer.name} is a dumb name";
                        break;

                    case 48:
                        screenMessageText = "It all ends eventually!";
                        break;

                    case 49:
                        screenMessageText = "Illegal in 5 countries!";
                        break;

                    case 50:
                        screenMessageText = "Inside jokes you wont understand!";
                        break;

                    case 51:
                        screenMessageText = "Big content mod bad!";
                        break;

                    case 52:
                        screenMessageText = "Loading the random chimp event...";
                        break;

                    case 53:
                        screenMessageText = "Sending you to the Aether...";
                        break;

                    case 54:
                        screenMessageText = "When";
                        break;

                    case 55:
                        screenMessageText = "[Insert non funny joke here]";
                        break;

                    case 56:
                        screenMessageText = "The dev server is indeed an asylum";
                        break;

                    case 57:
                        screenMessageText = "owo";
                        break;

                    case 58:
                        screenMessageText = "That's how the mafia works";
                        break;

                    case 59:
                        screenMessageText = "Hacking the mainframe...";
                        break;

                    case 60:
                        screenMessageText = "Not Proud";
                        break;

                    case 61:
                        screenMessageText = "You know I think the ocean needs more con- Haha the literal ocean goes brr";
                        break;

                    case 62:
                        screenMessageText = "EA Jorts, it's in the seams.";
                        break;

                    case 63:
                        screenMessageText = "Forged in Fury";
                        break;

                    case 64:
                        screenMessageText = "Have you guys heard of calamity?";
                        break;

                    case 65:
                        screenMessageText = "Who's the ideas guy?";
                        break;

                    case 66:
                        screenMessageText = "Tomato? Tomato.";
                        break;

                    case 67:
                        screenMessageText = "All existing code programmed by Stevie";
                        break;
                }

                Main.spriteBatch.Begin();
                DrawSky();

                if (Main.fontDeathText != null && screenMessageText != null)
                {
                    Vector2 textSize = Main.fontDeathText.MeasureString(screenMessageText);

                    if (progressMessage != null)
                    {
                        Vector2 textSize2 = Main.fontMouseText.MeasureString(progressMessage);
                        float textPosition2Left = Main.screenWidth / 2 - textSize2.X / 2;

                        Main.spriteBatch.DrawString(Main.fontMouseText, progressMessage, new Vector2(textPosition2Left, Main.screenHeight / 2 + 200), Color.AliceBlue * alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    }

                    float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;

                    Main.spriteBatch.DrawString(Main.fontDeathText, screenMessageText, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), Color.White * alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
            }
            else
            {
                if (!Main.dedServ)
                {
                    loadingChoose = Main.rand.Next(68);
                    loadingChooseImage = Main.rand.Next(5);
                    Main.numClouds = 10;

                    if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive())
                    {
                        SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                    }

                    Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
                    Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
                    Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
                    Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
                    Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
                }
            }
        }
        public static float lerp;
        private void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Terraria.Main self, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!Main.gameMenu && Main.netMode != NetmodeID.MultiplayerClient && !isSaving)
            {
                lerp = 0;
                alpha = 0;
                loadingChoose = Main.rand.Next(68);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;

                if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive())
                {
                    SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                }

                Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
                Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
                Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
                Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
                Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
            }

            Main.sunTexture = EEMod.instance.GetTexture("Empty");

            orig(self, gameTime);
        }

        private void Lighting_AddLight_int_int_float_float_float(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float R, float G, float B)
        {
            //LightPoints.Add(new Vector2(d + 0.5f,e + 0.5f));
            //ColourPoints.Add(new Color(a, b, c));

            orig(i, j, R, G, B);
        }
    }
}
