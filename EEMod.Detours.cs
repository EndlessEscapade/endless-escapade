using EEMod.Config;
using EEMod.ID;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
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
            On.Terraria.Main.DrawWoF += Main_DrawWoF;

            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;

            On.Terraria.WorldGen.SaveAndQuitCallBack += WorldGen_SaveAndQuitCallBack;
            On.Terraria.WorldGen.SmashAltar += WorldGen_SmashAltar;
        }
        private void UnloadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.DoUpdate -= Main_DoUpdate;
            On.Terraria.Main.Draw -= Main_Draw;
            On.Terraria.Main.DrawBG -= Main_DrawBG;
            On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
            On.Terraria.Main.DrawWoF -= Main_DrawWoF;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor -= UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf -= UIWorldListItem_DrawSelf;
            On.Terraria.WorldGen.SaveAndQuitCallBack -= WorldGen_SaveAndQuitCallBack;
            On.Terraria.WorldGen.SmashAltar -= WorldGen_SmashAltar;
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

        private void Main_DrawWoF(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {
            //UpdateLight();
            DrawNoiseSurfacing();
            DrawLensFlares();

            if (Main.worldName == KeyID.CoralReefs)
            {
                EEWorld.EEWorld.instance.DrawVines();
            }

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
                    if (Main.projectile[i].type == ModContent.ProjectileType<Gradient>())
                    {
                        (Main.projectile[i].modProjectile as Gradient).pixelPlacmentHours();
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<CyanoburstTomeKelp>())
                    {
                        (Main.projectile[i].modProjectile as CyanoburstTomeKelp).DrawBehind();
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
            prims.DrawTrails(Main.spriteBatch);

            orig(self);
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

        private void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Terraria.Main self, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!Main.gameMenu && Main.netMode != NetmodeID.MultiplayerClient && !isSaving)
            {
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