using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using EEMod.Projectiles;
using EEMod.NPCs.Bosses.Kraken;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        private void LoadIL()
        {
            On.Terraria.WorldGen.SmashAltar += WorldGen_SmashAltar;
            IL.Terraria.Main.DrawBackground += Main_DrawBackground;
            IL.Terraria.Main.OldDrawBackground += Main_OldDrawBackground;
            On.Terraria.Main.DoUpdate += OnUpdate;
            On.Terraria.WorldGen.SaveAndQuitCallBack += OnSave;
            On.Terraria.Main.DrawWoF += DrawBehindTiles;
            On.Terraria.Main.DrawBackground += OnDrawMenu;
        }
        private void UnloadIL()
        {
            On.Terraria.Main.DoUpdate -= OnUpdate;
            On.Terraria.WorldGen.SaveAndQuitCallBack -= OnSave;
            On.Terraria.WorldGen.SmashAltar -= WorldGen_SmashAltar;
            IL.Terraria.Main.DrawBackground -= Main_DrawBackground;
            On.Terraria.Main.DrawWoF -= DrawBehindTiles;
            On.Terraria.Main.DrawBackground -= OnDrawMenu;
        }

        private void Main_OldDrawBackground(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            Type spritebatchType = typeof(SpriteBatch);
            MethodInfo drawcall = spritebatchType.GetMethod(nameof(SpriteBatch.Draw), new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color) });
            MethodInfo drawcall2 = spritebatchType.GetMethod(nameof(SpriteBatch.Draw), new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(float), typeof(SpriteEffects), typeof(float) });
            MethodInfo get_noretro = typeof(Lighting).GetProperty(nameof(Lighting.NotRetro)).GetGetMethod();

            if (!c.TryGotoNext(i => i.MatchLdloc(19)))
                throw new Exception("Couldn't find local variable 19 loading");

            if (!c.TryGotoNext(i => i.MatchCallvirt(drawcall)))
                throw new Exception("Couldn't find call (post variable 19)");

            //int p = c.Index;
            //c.Index++; // move past
            //var label = c.DefineLabel(); // define label
            //c.Goto(p); // return
            //c.Emit(OpCodes.Br, label); // skip
            //c.MarkLabel(label); // define label target
            //c.Index--;
            c.Remove();
            c.Emit(OpCodes.Ldloc, 15); // array
            c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, int[]>>((sb, texture, pos, rectangle, color, array) =>
            {
                if (array[4] != 135 && array[4] != 131)
                    sb.Draw(texture, pos, rectangle, color);
            });

            if (!c.TryGotoNext(i => i.MatchCall(get_noretro)))
                throw new Exception("Couldn't find Lighting.NoRetro get call");

            for (int k = 0; k < 4; k++)
            {
                if (!c.TryGotoNext(i => i.MatchCallvirt(drawcall2)))
                    throw new Exception($"Couldn't find call {k}");
                c.Remove();
                c.Emit(OpCodes.Ldloc, 15);
                c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float, int[]>>((sb, texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth, array) =>
                {
                    if (array[5] != 126 && array[5] != 125)
                        sb.Draw(texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth);
                });
            }
        }
        public void DrawBehindTiles(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {
            /* for (int i = 0; i < 400; i++)
             {
                 if (Main.projectile[i].type == ModContent.ProjectileType<BetterLighting>())
                 {
                     (Main.projectile[i].modProjectile as BetterLighting).drawIt();
                 }
             }*/
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
        public void OnSave(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadcontext)
        {
            isSaving = true;
            orig(threadcontext);
            isSaving = false;
            //saveInterface?.SetState(null);
        }

        public void OnUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, GameTime gameTime)
        {
            if (!Main.gameMenu && Main.netMode != NetmodeID.MultiplayerClient && !isSaving)
            {
                loadingChoose = Main.rand.Next(62);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;
                if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                Main.logo2Texture = TextureCache.Terraria_Logo2Texture;
                Main.logoTexture = TextureCache.Terraria_LogoTexture;
                Main.sun2Texture = TextureCache.Terraria_Sun2Texture;
                Main.sun3Texture = TextureCache.Terraria_Sun3Texture;
                Main.sunTexture = TextureCache.Terraria_SunTexture;
            }
            orig(self, gameTime);
        }

        private void OnDrawMenu(On.Terraria.Main.orig_DrawBackground orig, Main self)
        {
            orig(self);
            velocity = Vector2.Zero;
            if (isSaving && Main.gameMenu)
            {
                Main.numClouds = 0;
                Main.logo2Texture = TextureCache.Empty;
                Main.logoTexture = TextureCache.Empty;
                Main.sun2Texture = TextureCache.Empty;
                Main.sun3Texture = TextureCache.Empty;
                Main.sunTexture = TextureCache.Empty;
                if (SkyManager.Instance["EEMod:SavingCutscene"] != null) SkyManager.Instance.Activate("EEMod:SavingCutscene", default, new object[0]);
                if (loadingFlag)
                {
                    loadingChoose = Main.rand.Next(65); // numbers from n to n-1
                    loadingChooseImage = Main.rand.Next(5);
                    loadingFlag = false;
                }
                switch (loadingChoose)
                {
                    case 0:
                        Main.statusText = "Watch Out! Dune Shamblers Pop from the ground from time to time!";
                        break;
                    case 1:
                        Main.statusText = "All good sprites made by Nomis";
                        break;
                    case 2:
                        Main.statusText = "Tip of the Day! Loading screens are useless";
                        break;
                    case 3:
                        Main.statusText = "Fear the MS Paint cat";
                        break;
                    case 4:
                        Main.statusText = "Terraria sprites need outlines... except when I make them";
                        break;
                    case 5:
                        Main.statusText = "Remove the Banding";
                        break;
                    case 6:
                        Main.statusText = Main.LocalPlayer.name + " ....huh...what a cruddy name";
                        break;
                    case 7:
                        Main.statusText = "Dont ping everyone you big dumb stupid";
                        break;
                    case 8:
                        Main.statusText = "I'm nothing without attention";
                        break;
                    case 9:
                        Main.statusText = "Why are you even reading this?";
                        break;
                    case 10:
                        Main.statusText = "We actually think we are funny";
                        break;
                    case 11:
                        Main.statusText = "Interitos...whats that?";
                        break;
                    case 12:
                        Main.statusText = "its my style";
                        break;
                    case 13:
                        Main.statusText = "Now featuring 50% more monkey per chimp!";
                        break;
                    case 14:
                        Main.statusText = "im angy";
                        break;
                    case 15:
                        Main.statusText = "All good music made by A44";
                        break;
                    case 16:
                        Main.statusText = "Mod is not edgy I swear!";
                        break;
                    case 17:
                        Main.statusText = "All good art made by cynik";
                        break;
                    case 18:
                        Main.statusText = "Im gonna have to mute you for that";
                        break;
                    case 19:
                        Main.statusText = "Gamers rise up!";
                        break;
                    case 20:
                        Main.statusText = "THATS NOT THE CONCEPT";
                        break;
                    case 21:
                        Main.statusText = "caramel popcorn and celeste";
                        break;
                    case 22:
                        Main.statusText = "D D D A G# G F D F G";
                        break;
                    case 23:
                        Main.statusText = "We live in a society";
                        break;
                    case 24:
                        Main.statusText = "Dont mine at night!";
                        break;
                    case 25:
                        Main.statusText = "deleting system32...";
                        break;
                    case 26:
                        Main.statusText = "Sans in real!";
                        break;
                    case 27:
                        Main.statusText = "I sure hope I didnt break the codeghsduighshsy";
                        break;
                    case 28:
                        Main.statusText = "If you liked endless escapade you will love endless escapade premium!";
                        break;
                    case 29:
                        Main.statusText = "When\nBottomText";
                        break;
                    case 30:
                        Main.statusText = "mario in real life";
                        break;
                    case 31:
                        Main.statusText = "All good concept art made by phanta";
                        break;
                    case 32:
                        Main.statusText = "EEMod Foretold? More like doesn't exist";
                        break;
                    case 33:
                        Main.statusText = "You think this is a game? Look behind you 0_0";
                        break;
                    case 34:
                        Main.statusText = "Respect the drip Karen";
                        break;
                    case 35:
                        Main.statusText = "trust me there is a lot phesh down in here, the longer the player is in the reefs the more amphibious he will become";
                        break;
                    case 36:
                        Main.statusText = "All good sprites made by daimgamer!";
                        break;
                    case 37:
                        Main.statusText = "All good music made by Universe";
                        break;
                    case 38:
                        Main.statusText = "All good sprites made by Vadim";
                        break;
                    case 39:
                        Main.statusText = "All good sprites made by Zarn";
                        break;
                    case 40:
                        Main.statusText = "All good sprites made by Franswal";
                        break;
                    case 41:
                        Main.statusText = "Totally not copying Starlight River";
                        break;
                    case 42:
                        Main.statusText = "Do a Barrel Roll";
                        break;
                    case 43:
                        Main.statusText = "The man behind the laughter";
                        break;
                    case 44:
                        Main.statusText = "Paint ruins the experience of building - Franswal 2020";
                        break;
                    case 45:
                        Main.statusText = "An apple a day keeps the errors away!";
                        break;
                    case 46:
                        Main.statusText = "Poggers? Poggers.";
                        break;
                    case 47:
                        Main.statusText = $"Totally not sentient AI. By the way, {Environment.UserName} is a dumb computer name";
                        break;
                    case 48:
                        Main.statusText = "It all ends eventually!";
                        break;
                    case 49:
                        Main.statusText = "Illegal in 5 countries!";
                        break;
                    case 50:
                        Main.statusText = "Inside jokes you wont understand!";
                        break;
                    case 51:
                        Main.statusText = "Big content mod bad!";
                        break;
                    case 52:
                        Main.statusText = "Loading the random chimp event...";
                        break;
                    case 53:
                        Main.statusText = "Sending you to the Aether...";
                        break;
                    case 54:
                        Main.statusText = "When";
                        break;
                    case 55:
                        Main.statusText = "[Insert non funny joke here]";
                        break;
                    case 56:
                        Main.statusText = "The dev server is indeed an asylum";
                        break;
                    case 57:
                        Main.statusText = "owo";
                        break;
                    case 58:
                        Main.statusText = "That's how the mafia works";
                        break;
                    case 59:
                        Main.statusText = "Hacking the mainframe...";
                        break;
                    case 60:
                        Main.statusText = "Not Proud";
                        break;
                    case 61:
                        Main.statusText = "You know I think the ocean needs more con- Haha the literal ocean goes brr";
                        break;
                    case 62:
                        Main.statusText = "EA Jorts, it's in the seams.";
                        break;
                    case 63:
                        Main.statusText = "Forged in Fury";
                        break;
                }
            }
            else
            {
                if (!Main.dedServ)
                {
                    loadingChoose = 47;//Main.rand.Next(64);
                    loadingChooseImage = Main.rand.Next(5);
                    Main.numClouds = 10;
                    if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                    Main.logo2Texture = TextureCache.Terraria_Logo2Texture;
                    Main.logoTexture = TextureCache.Terraria_LogoTexture;
                    Main.sun2Texture = TextureCache.Terraria_Sun2Texture;
                    Main.sun3Texture = TextureCache.Terraria_Sun3Texture;
                    Main.sunTexture = TextureCache.Terraria_SunTexture;
                }

            }
        }

        private void Main_DrawBackground(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            Type spritebatchtype = typeof(SpriteBatch);

            if (!c.TryGotoNext(i => i.MatchLdloc(18)))
                throw new Exception("Ldloc for local variable 18 not found");

            MethodInfo call1 = spritebatchtype.GetMethod("Draw", new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color) });

            if (!c.TryGotoNext(i => i.MatchCallvirt(call1)))
                throw new Exception("No call found for SpriteBatch.Draw(Texture2D, Vector2, Rectangle?, Color)");

            // 1st call
            c.Remove();
            c.Emit(OpCodes.Ldloc, 13); // array
            c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, int[]>>((spritebatch, texture, pos, sourcerectangle, color, array) =>
            {
                if (array[4] != 135)
                    spritebatch.Draw(texture, pos, sourcerectangle, color);
            });
            // 2nd call
            // getting to the else
            MethodInfo lightningnoretroget = typeof(Lighting).GetProperty(nameof(Lighting.NotRetro)).GetGetMethod();
            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt(lightningnoretroget)))
                throw new Exception("Call for the get method of the property Lighting.NoRetro not found");
            // finding the call       
            MethodInfo draw = spritebatchtype.GetMethod("Draw", new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(float), typeof(SpriteEffects), typeof(float) });
            for (int k = 0; k < 4; k++) // 4 calls
            {
                if (!c.TryGotoNext(i => i.MatchCallvirt(draw)))
                    throw new Exception($"Call number {k} not found");

                c.Remove();
                c.Emit(OpCodes.Ldloc, 13); // array
                c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float, int[]>>((spritebatch, texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth, array) =>
                {
                    if (array[5] != 126)
                        spritebatch.Draw(texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth);
                });
            }

            // 3rd call
            if (!c.TryGotoNext(i => i.MatchLdloc(20))) throw new Exception("Ldloc for local variable 20 (flag4) not found"); // flag4
            if (!c.TryGotoNext(i => i.MatchCallvirt(call1))) throw new Exception("'Last' SpriteBatch.Draw call not found"); // same overload
            c.Remove();
            c.Emit(OpCodes.Ldloc, 13); // array
            c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, int[]>>((spritebatch, texture, position, sourcerectangle, color, array) =>
            {
                if (array[6] != 186)
                    spritebatch.Draw(texture, position, sourcerectangle, color);
            });
        }

        public static void WorldGen_SmashAltar(On.Terraria.WorldGen.orig_SmashAltar orig, int i, int j)
        {
            orig(i, j);
            EEPlayer.moralScore -= 50;
            Main.NewText(EEPlayer.moralScore);
        }
        /*private static void ILSaveWorldTiles(ILContext il)
{
    ILCursor c = new ILCursor(il);
    PropertyInfo statusText = typeof(Main).GetProperty(nameof(Main.statusText));
    MethodInfo set = statusText.GetSetMethod();

    if (!c.TryGotoNext(i => i.MatchCall(set)))
        throw new Exception();

    c.EmitDelegate<Func<string, string>>((originalText) =>
    {
        return originalText;
    });
}*/
        /*
        //private static void ModifyColor(ref Color color, byte val)
        //{
        // 
        //}
        // private delegate void colorrefdelegate(ref Color color, byte val);
        private delegate void modifyingdelegate(Main instance, ref int focusmenu, ref int selectedmenu, ref int num2, ref int num4, ref int[] array4, ref byte[] array6, ref string[] array9, ref bool[] array, ref int num5, ref bool flag);
#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
        private static void GenkaiMenu(Main instance, ref int focusMenu, ref int selectedMenu, ref int num2, ref int num4, ref int[] array4, ref byte[] array6, ref string[] array9, ref bool[] array, ref int num5, ref bool flag5)
        {
            num2 = 200;
            num4 = 60;
            int offset = -10;
            array4[2] = 30 + offset - 1; //30 - 20; // 30
            array4[3] = 30 + offset - 3 - 1; //30 - 10; // 30
            array6[3] = 2; //2; // rarity // 2
            array4[4] = 70; // 70
            array4[5] = -40 + offset / 2 - 1; // -40 - 10;
            array6[5] = 5;
            if (focusMenu == 2)
            {
                array9[0] = Language.GetTextValue("UI.NormalDescriptionFlavor");
                array9[1] = Language.GetTextValue("UI.NormalDescription");
            }
            else if (focusMenu == 3)
            {
                array9[0] = Language.GetTextValue("UI.ExpertDescriptionFlavor");
                array9[1] = Language.GetTextValue("UI.ExpertDescription");
            }
            else if (focusMenu == 5) // Genkai's
            {
                array9[0] = "Not for easily angried";
                array9[1] = "(What'll it be? Who knows, find out ;])";
            }
            else
            {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                array9[0] = Lang.menu[32].Value;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            }
            array[0] = true;
            array[1] = true;

            array9[2] = Language.GetTextValue("UI.Normal");
            array9[3] = Language.GetTextValue("UI.Expert");
            array9[4] = Language.GetTextValue("UI.Back");
            array9[5] = "Genkai"; // Genkai
            num5 = 6;
            if (selectedMenu == 2)
            {
                Main.expertMode = false;
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = 7;
                if (Main.SettingsUnlock_WorldEvil)
                {
                    Main.menuMode = -71;
                }
            }
            else if (selectedMenu == 3)
            {
                Main.expertMode = true;
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = 7;
                if (Main.SettingsUnlock_WorldEvil)
                {
                    Main.menuMode = -71;
                }
            }
            else if (selectedMenu == 5) // Genkai's
            {
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = Main.SettingsUnlock_WorldEvil ? -71 : 7;
                Main.expertMode = true;
                EEWorld.EEWorld.GenkaiMode = true;
            }
            else if (selectedMenu == 4 || flag5)
            {
                flag5 = false;
                Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                Main.menuMode = 16;
            }
            Main.clrInput();
        }
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
        */
    }
}