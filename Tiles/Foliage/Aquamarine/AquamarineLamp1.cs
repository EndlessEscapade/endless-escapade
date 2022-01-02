using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using EEMod.NPCs.Aquamarine;
using EEMod.Projectiles.Enemy;
using EEMod.Extensions;
using Terraria.ID;
using EEMod.Prim;
using System.Linq;

namespace EEMod.Tiles.Foliage.Aquamarine
{
    public class AquamarineLamp1 : EETile
    {
        public override void SetStaticDefaults()
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
            // TileObjectData.newTile.LavaDeath = false;
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

    public class AquamarineLamp1Glow : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Lamp");
        }

        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.timeLeft = 999999999;
            Projectile.ignoreWater = true;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            // Projectile.tileCollide = false;
            Projectile.extraUpdates = 12;
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
            Projectile.timeLeft = 999999999;
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
                    AquamarineSpire spirespire = spire.ModNPC as AquamarineSpire;

                    if ((spire.ai[0] <= 40 && spire.ai[0] > 20 && !dead)) //If in first phase and projectile is not inactive
                    {
                        Vector2 desiredVector = (spire.Center + new Vector2(-2, 2)) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * Projectile.ai[1]) + Main.GameUpdateCount * 2)) * 48;

                        if (Vector2.Distance(Projectile.Center, desiredVector) > 10)
                            Projectile.Center += Vector2.Normalize(desiredVector - Projectile.Center) * 4;
                        else
                            Projectile.Center = desiredVector;

                        var proj = Main.projectile.Where(x => Vector2.DistanceSquared(x.Center, Projectile.Center) <= 24 * 24 && x.type == ModContent.ProjectileType<SpireLaser>() && x.ai[0] > 0 && x.active);
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

                        if(((Projectile.ai[0] + Projectile.ai[1]) * 120) % Main.GameUpdateCount == 0)
                        {
                            Player target = Main.player[Helpers.GetNearestAlivePlayer(spire)];

                            Projectile projectile2 = Projectile.NewProjectileDirect(new ProjectileSource_BySourceId(ModContent.ProjectileType<SpireLaserAlt>()), Projectile.Center, Vector2.Normalize(target.Center - Projectile.Center) * 2, ModContent.ProjectileType<SpireLaserAlt>(), (int)(Main.npc[spire.whoAmI].damage / 5f), 0f, default, 0, 1);
                            PrimitiveSystem.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Cyan, Color.Magenta, Main.rand.NextFloat(0, 1)), 30));
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
                        Vector2 desiredVector = desiredTarget + Vector2.UnitX.RotatedBy(speen + MathHelper.ToRadians((360f / Projectile.ai[0]) * Projectile.ai[1])) * 128;

                        Projectile.Center = desiredVector;

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
                    origin = Projectile.Center;
                    firstFrame = false;
                }

                if (spire.ai[0] <= 20 && dead) dead = false;

                if (dead || !active)
                {
                    if (Vector2.Distance(Projectile.Center, origin) > 10)
                        Projectile.Center += Vector2.Normalize(origin - Projectile.Center) * 4;
                    else
                        Projectile.Center = origin;
                }
            }
        }
        

        float HeartBeat;
        private int frame;
        private int frameTimer;
        private int frameSpeed = 0;

        private Color strikeColor;
        private int strikeTime;
        public override bool PreDraw(ref Color lightColor)
        {
            if (spire.active == true && Vector2.DistanceSquared(Main.LocalPlayer.Center, Projectile.Center) <= (192 * 16) * (192 * 16))
            {
                HeartBeat = spire.ai[3];
                Projectile.scale = 1f + (HeartBeat / 5f);

                Texture2D tex = EEMod.Instance.Assets.Request<Texture2D>("Tiles/Foliage/Aquamarine/AquamarineLamp1Glow").Value;
                Texture2D mask = EEMod.Instance.Assets.Request<Texture2D>("Textures/SmoothFadeOut").Value;

                float sineAdd = (float)Math.Sin(Main.GameUpdateCount / 20f) + 2.5f;
                Main.spriteBatch.Draw(mask, Projectile.position, null, new Color(sineAdd, sineAdd, sineAdd, 0) * 0.2f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

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

                AquamarineSpire spirespire = spire.ModNPC as AquamarineSpire; 
                //(spire.ai[0] <= 20 && spire.ai[0] > 0 && active && !spirespire.phase2Transition)
                if ((spire.ai[0] <= 40 && spire.ai[0] > 20 && !dead))
                {
                    int tempint = (int)Projectile.ai[1] + 1;

                    if ((tempint) > spirespire.shields.Count - 1) tempint = 0;

                    if (!(spirespire.shields[tempint].ModProjectile as AquamarineLamp1Glow).dead)
                    {
                        Vector2 desiredVector = (spire.Center + new Vector2(-2, 2)) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * (Projectile.ai[1] + 1)) + Main.GameUpdateCount * 2)) * 48;

                        Vector2 desiredPos = (spire.Center + new Vector2(-2, 2)) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * (Projectile.ai[1])) + Main.GameUpdateCount * 2)) * 48;

                        if (Vector2.Distance(desiredPos, Projectile.Center) <= 10)
                        {
                            float n = 1 / (desiredVector - Projectile.Center).Length();

                            for (float k = 0; k < 1; k += n)
                            {
                                Color drawColor = Color.Lerp(Color.Cyan, Color.Magenta, (float)Math.Sin(Main.GameUpdateCount / 30f));

                                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Particles/Square").Value, Projectile.Center + (desiredVector - Projectile.Center) * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), drawColor, (desiredVector - Projectile.Center).ToRotation(), Vector2.One, 2f, SpriteEffects.None, 0);
                            }
                        }
                    }

                    frame = 0;
                }
                
                if (spire.ai[0] <= 20 && spire.ai[0] > 0 && active) //If in second phase and projectile is not inactive
                {
                    Vector2 desiredVector = desiredTarget + Vector2.UnitX.RotatedBy(speen + MathHelper.ToRadians((360f / Projectile.ai[0]) * (Projectile.ai[1] + 1))) * 128;

                    Vector2 desiredPos = desiredTarget + Vector2.UnitX.RotatedBy(speen + MathHelper.ToRadians((360f / Projectile.ai[0]) * Projectile.ai[1])) * 128;

                    if (Vector2.Distance(desiredPos, Projectile.Center) <= 10)
                    {
                        float n = 1 / (desiredVector - Projectile.Center).Length();

                        for (float k = 0; k < 1; k += n)
                        {
                            Color drawColor = Color.Lerp(Color.Cyan, Color.Magenta, (float)Math.Sin(Main.GameUpdateCount / 30f));

                            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Particles/Square").Value, Projectile.Center + (desiredVector - Projectile.Center) * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), drawColor, (desiredVector - Projectile.Center).ToRotation(), Vector2.One, 2f, SpriteEffects.None, 0);
                        }
                    }

                    frame = 0;
                }

                Vector2 diamondPos = Projectile.Center + new Vector2(4 * (float)Math.Sin(Main.GameUpdateCount / 10f));

                if (!active && !dead && Vector2.Distance(Projectile.Center, origin) == 0)
                {
                    EEMod.MainParticles.SetSpawningModules(new SpawnPeriodically(8, true));
                    Vector2 part = Projectile.Center;
                    EEMod.MainParticles.SpawnParticles(part, default, 2, Color.White, new CircularMotionSinSpinC(15, 15, 0.1f, part), new AfterImageTrail(1), new SetMask(Helpers.RadialMask));

                    Lighting.AddLight(Projectile.Center, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Math.Abs((float)Math.Sin(Main.GameUpdateCount / 100f))).ToVector3());
                }

                if(strikeTime > 0) strikeTime--;

                Helpers.DrawAdditive(mask, Projectile.Center.ForDraw(), Color.White * (0.5f + (HeartBeat / 2f)), Projectile.scale, Projectile.rotation);

                Rectangle rect = new Rectangle(0, frame * 24, 22, 24);
                Main.spriteBatch.Draw(tex, Projectile.Center.ForDraw(), rect, lightColor, 0f, new Vector2(11, 12), Projectile.scale, SpriteEffects.None, 1f);
                Main.spriteBatch.Draw(tex, Projectile.Center.ForDraw(), rect, Color.Lerp(Color.White * HeartBeat, strikeColor, strikeTime / 60f), 0f, new Vector2(11, 12), Projectile.scale, SpriteEffects.None, 1f);
            }
            return false;
        }
    }
}
