using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.Projectiles;
using EEMod.Tiles;
using EEMod.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;
using static Terraria.ModLoader.ModContent;
using EEMod.Seamap.Core;

using EEMod.NPCs.Aquamarine;
using Terraria.DataStructures;
using EEMod.NPCs.Friendly;
using EEMod.Subworlds.CoralReefs;
using EEMod.Subworlds;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        private readonly List<float> _bubbleRoots = new List<float>();
        public List<BubbleClass> bubbles = new List<BubbleClass>();

        public bool jellyfishMigration;

        /*public void UpdateCR()
        {
            /*if (player.position.Y >= 800 * 16 && !player.accDivingHelm)
            {
                player.AddBuff(BuffType<WaterPressure>(), 60);
            }

            if (Main.dayTime)
            {
                IncreaseStarFall = true;
            }
            else if (IncreaseStarFall)
            {
                IncreaseStarFall = false;
                Star.starfallBoost += 1f;
            }

            if (Player.GetModPlayer<SeamapPlayer>().noU)
            {
                Player.GetModPlayer<SeamapPlayer>().titleText -= 0.005f;
            }
            else
            {
                Player.GetModPlayer<SeamapPlayer>().titleText += 0.005f;
            }

            if (Player.GetModPlayer<SeamapPlayer>().titleText >= 1)
            {
                Player.GetModPlayer<SeamapPlayer>().noU = true;
            }

            if (Player.GetModPlayer<SeamapPlayer>().titleText <= 0)
            {
                Player.GetModPlayer<SeamapPlayer>().titleText = 0;
            }

            Player.GetModPlayer<SeamapPlayer>().seamapUpdateCount++;

            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, CoralReefs.SpirePosition * 16) < 700 * 700)
            {
                HasVisitedSpire = true;
            }

            if (HasVisitedSpire && SpireCutscene < 200)
            {
                FixateCameraOn(CoralReefs.SpirePosition * 16 + new Vector2(0, -13 * 16), 64f, false, true, 0);
                SpireCutscene++;
            }

            if (SpireCutscene == 200)
            {
                HasVisitedSpire = true;
                SpireCutscene++;
                TurnCameraFixationsOff();
            }

            if (!Player.GetModPlayer<SeamapPlayer>().arrowFlag)
            {
                NPC.NewNPC(new Terraria.DataStructures.EntitySource_Parent(Player), (int)(CoralReefs.SpirePosition.X * 16), (int)(CoralReefs.SpirePosition.Y * 16), NPCType<AquamarineSpire>());
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, Main.myPlayer);
                }

                Player.GetModPlayer<SeamapPlayer>().arrowFlag = true;
            }

            if (CoralReefs.CoralBoatPos == Vector2.Zero)
            {
                CoralReefs.CoralBoatPos = new Vector2(200, 48);
            }

            try
            {
                //Projectile oceanarrow = Main.projectile[Arrow2];

                if (Helpers.PointInRectangle(Player.Center / 16, CoralReefs.CoralBoatPos.X, CoralReefs.CoralBoatPos.Y + 12, 4, 4))
                {
                    if (Player.controlUp)
                    {
                        Initialize();

                        SeamapObjects.localship.position = new Vector2(Main.screenWidth - 300, Main.screenHeight - 600);

                        SubworldLibrary.SubworldSystem.Enter<Sea>();
                    }
                }
                else
                {
                    // ArrowsUIState.OceanArrowVisible = false;
                }
            }
            catch { }

            if (Main.moonPhase == 4 && !Main.dayTime && !jellyfishMigration)
            {
                Main.NewText("Jellyfish are migrating on the surface!", new Color(50, 255, 130));
                jellyfishMigration = true;
            }
            if (jellyfishMigration && Main.dayTime)
            {
                jellyfishMigration = false;
            }
        }*/
    }
}