using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.Tiles.Furniture.OrbHolder;

namespace EEMod.NPCs.CoralReefs
{
    public class OrbCollection : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb");
        }

        public int rippleCount = 2;
        public int rippleSize = 13;
        public int rippleSpeed = 200;
        public float distortStrength = 5;

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.alpha = 20;
            npc.lifeMax = 1000000;
            npc.width = 128;
            npc.height = 130;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                //  npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 3)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        private bool isPicking;
        private bool otherPhase;
        private bool otherPhase2;
        private float t;
        private Vector2[] Holder = new Vector2[2];

        public override void AI()
        {
            npc.ai[0] += 0.05f;
            if (!otherPhase)
                npc.position.Y += (float)Math.Sin(npc.ai[0]) / 2f;
            if (npc.life == 0)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
                {
                    Filters.Scene["EEMod:Shockwave"].Deactivate();
                }
            }
            if (Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
            {
                npc.Center = Main.player[(int)npc.ai[1]].Center - new Vector2(0, 80);
                if (Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
                {
                    Main.player[(int)npc.ai[1]].bodyFrame.Y = 56 * 5;
                }
            }
            if (isPicking && !Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos == Vector2.Zero)
                {
                    otherPhase = true;
                    Holder[0] = npc.Center;
                    Holder[1] = Main.MouseWorld;
                }
                else
                {
                    otherPhase2 = true;
                    Holder[0] = npc.Center;
                    Holder[1] = Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos + new Vector2(70, 60);
                }
            }
            if (otherPhase)
            {
                t += 0.01f;
                if (t <= 1)
                {
                    Vector2 mid = (Holder[0] + Holder[1]) / 2;
                    npc.Center = Helpers.TraverseBezier(Holder[1], Holder[0], mid - new Vector2(0, 300), mid - new Vector2(0, 300), t);
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, false, true, 0);
                }
                else if (t <= 1.3f)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, true, false, 10);
                }
                else
                {
                    t = 0;
                    otherPhase = false;
                }
            }
            else if (otherPhase2)
            {
                t += 0.01f;
                if (t <= 1)
                {
                    Vector2 mid = (Holder[0] + Holder[1]) / 2;
                    npc.Center = Helpers.TraverseBezier(Holder[1], Holder[0], mid - new Vector2(0, 300), mid - new Vector2(0, 300), t);
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, false, true, 0);
                }
                else if (t <= 1.3f)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, true, false, 10);
                }
                else
                {
                    Tile tile = Main.tile[(int)(Holder[1].X / 16), (int)(Holder[1].Y / 16)];
                    int index = ModContent.GetInstance<OrbHolderTE>().Find((int)(Holder[1].X / 16 - tile.frameX / 16), (int)(Holder[1].Y / 16 - tile.frameY / 16));
                    if (index != -1)
                    {
                        OrbHolderTE TE = (OrbHolderTE)TileEntity.ByID[index];
                        TE.hasOrb = true;
                    }
                    t = 0;
                    otherPhase2 = false;
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 position = npc.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 50 * i)) * 30;
                        //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                        Dust dust = Dust.NewDustPerfect(position, DustID.PurpleCrystalShard);
                        dust.noGravity = true;
                        dust.velocity = Vector2.Normalize(dust.position - npc.Center) * 4;
                        dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                    npc.life = 0;
                    npc.timeLeft = 0;
                }
            }
            else
            {
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }
            isPicking = Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp;
        }

        public int size = 128;
        public int sizeGrowth;
        public float num88 = 1;
    }
}