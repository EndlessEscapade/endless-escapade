using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles.Enemy;
using EEMod.Extensions;
using Terraria.ID;
using EEMod.Prim;
using System.Linq;

namespace EEMod.Tiles.Foliage.Coral
{
    public class AquamarineLamp1 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.9f;
            b = 0.9f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {

        }
    }

    public class AquamarineLamp1Glow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Lamp");
        }

        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.timeLeft = 999999999;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.extraUpdates = 12;
        }

        private NPC spire = null;
        private Vector2 origin = Vector2.Zero;
        private bool active = false;
        private bool firstFrame = true;
        private bool dead = false;
        private float speen;
        private Vector2 desiredTarget;
        private float spid;

        public override void AI()
        {
            projectile.timeLeft = 999999999;
            if (spire == null)
            {
                for (int k = 0; k < Main.npc.Length - 1; k++)
                {
                    if (Main.npc[k].type == ModContent.NPCType<AquamarineSpire>())
                    {
                        spire = Main.npc[k];
                    }
                }
            }

            if (spire != null)
            {
                if (spire.ai[0] == 40 && !dead)
                {
                    active = true;
                }

                if (active)
                {
                    if (spire.ai[0] <= 40 && spire.ai[0] > 20 && !dead) //If in first phase and projectile is not inactive
                    {
                        Vector2 desiredVector = (spire.Center + new Vector2(-2, 2)) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * projectile.ai[1]) + Main.GameUpdateCount * 2)) * 48;

                        if (Vector2.Distance(projectile.Center, desiredVector) > 10)
                            projectile.Center += Vector2.Normalize(desiredVector - projectile.Center) * 4;
                        else
                            projectile.Center = desiredVector;

                        var proj = Main.projectile.Where(x => Vector2.DistanceSquared(x.Center, projectile.Center) <= 900 && x.type == ModContent.ProjectileType<SpireLaser>() && x.ai[0] > 0 && x.active);
                        foreach (var laser in proj)
                        {
                            switch (laser.ai[1])
                            {
                                case 0:
                                    break;
                                case 1: //Blue
                                    strikeColor = Color.Blue;
                                    break;
                                case 2: //Cyan
                                    strikeColor = Color.Cyan;
                                    break;
                                case 3: //Pink
                                    strikeColor = Color.Magenta;
                                    break;
                                case 4: //Purple
                                    strikeColor = Color.Purple;
                                    break;
                            }

                            strikeTime = 60;
                            
                            dead = true;
                            laser.Kill();
                        }
                    }

                    if (spire.ai[0] <= 20 && spire.ai[0] > 0) //If in second phase and projectile is not inactive
                    {
                        Player target = Main.player[Helpers.GetNearestAlivePlayer(spire)];

                        if (desiredTarget == Vector2.Zero) desiredTarget = target.Center;
                        if (Main.GameUpdateCount % 240 < 180)
                        {
                            spid = ((Main.GameUpdateCount % 240) / 180f);

                            if (Vector2.Distance(desiredTarget, target.Center) > 10)
                                desiredTarget += Vector2.Normalize(target.Center - desiredTarget) * 4;
                            else
                                desiredTarget = target.Center;
                        }
                        if (Main.GameUpdateCount % 240 >= 180)
                        {
                            if(spid > 0) spid -= 0.04f;
                            if (spid < 0) spid = 0;

                            desiredTarget += (target.Center - desiredTarget) * (1 - Helpers.Clamp(((Main.GameUpdateCount % 240) - 180) / 30f, 0, 1));
                        }

                        speen += (spid / 60f);
                        Vector2 desiredVector = desiredTarget + Vector2.UnitX.RotatedBy(speen + MathHelper.ToRadians((360f / projectile.ai[0]) * projectile.ai[1])) * 128;

                        projectile.Center = desiredVector;

                        if (Vector2.DistanceSquared(target.Center, desiredTarget) >= 192 * 192)
                        {
                            target.Hurt(PlayerDeathReason.ByNPC(spire.whoAmI), 50, default);
                        }
                    }

                    if(spire.ai[0] <= 0)
                    {
                        active = false;
                        dead = false;
                    }
                }

                if (firstFrame)
                {
                    origin = projectile.Center;
                    firstFrame = false;
                }

                if (spire.ai[0] <= 20 && dead) dead = false;

                if (dead || !active)
                {
                    if (Vector2.Distance(projectile.Center, origin) > 10)
                        projectile.Center += Vector2.Normalize(origin - projectile.Center) * 4;
                    else
                        projectile.Center = origin;
                }
            }
        }
        

        float HeartBeat;
        private int frame;
        private int frameTimer;
        private int frameSpeed = 0;

        private int cooldown = 240;
        private Color strikeColor;
        private int strikeTime;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (spire != null)
            {
                HeartBeat = spire.ai[3];

                Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Tiles/Foliage/Coral/AquamarineLamp1Glow");
                Texture2D mask = ModContent.GetInstance<EEMod>().GetTexture("Masks/SmoothFadeOut");

                float sineAdd = (float)Math.Sin(Main.GameUpdateCount / 20f) + 2.5f;
                Main.spriteBatch.Draw(mask, projectile.position, null, new Color(sineAdd, sineAdd, sineAdd, 0) * 0.2f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                frameSpeed = 3 + (int)(Math.Sin(Main.GameUpdateCount / 60f) * 2);
                if (frameSpeed > 0)
                {
                    frameTimer++;
                    if (frameTimer >= frameSpeed)
                    {
                        frame++;
                        frameTimer = 0;
                    }
                    if (frame >= 8) frame = 0;
                }

                if (spire.ai[0] <= 20 && spire.ai[0] > 0 && active) //If in second phase and projectile is not inactive
                {
                    Player target = Main.player[Helpers.GetNearestAlivePlayer(spire)];

                    Vector2 desiredVector = desiredTarget + Vector2.UnitX.RotatedBy(speen + MathHelper.ToRadians((360f / projectile.ai[0]) * (projectile.ai[1] + 1))) * 128;

                    float n = 1 / (desiredVector - projectile.Center).Length();

                    for (float k = 0; k < 1; k += n)
                    {
                        Main.spriteBatch.Draw(mod.GetTexture("Particles/Square"), projectile.Center + (desiredVector - projectile.Center) * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Lerp(Color.Cyan, Color.Magenta, (float)Math.Sin(Main.GameUpdateCount / 30f)), (desiredVector - projectile.Center).ToRotation(), Vector2.One, 2f, SpriteEffects.None, 0);
                    }

                    frame = 0;
                }

                Vector2 diamondPos = projectile.Center + new Vector2(4 * (float)Math.Sin(Main.GameUpdateCount / 10f));

                if (!active && !dead)
                {
                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnPeriodically(8, true));
                    Vector2 part = projectile.Center;
                    EEMod.Particles.Get("Main").SpawnParticles(part, default, 2, Color.White, new CircularMotionSinSpinC(15, 15, 0.1f, part), new AfterImageTrail(1), new SetMask(Helpers.RadialMask));

                    Lighting.AddLight(projectile.Center, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Math.Abs((float)Math.Sin(Main.GameUpdateCount / 100f))).ToVector3());
                }

                if(strikeTime > 0) strikeTime--;
                Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), new Rectangle(0, frame * 24, 22, 24), Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16)), 0f, new Vector2(11, 12), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), new Rectangle(0, frame * 24, 22, 24), Color.Lerp(Color.White * HeartBeat, strikeColor, strikeTime / 60f), 0f, new Vector2(11, 12), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
