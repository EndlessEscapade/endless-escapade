using EEMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using EEMod.Prim;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class Darksaber : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksaber");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 0;
            Item.knockBack = 6.5f;
            Item.autoReuse = true;
            Item.reuseDelay = 0;
            Item.width = 16;
            Item.height = 22;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.shoot = ModContent.ProjectileType<DarksaberHilt>();
            Item.noUseGraphic = true;

            Item.DamageType = DamageClass.Melee;

            Item.UseSound = SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/darksaber");
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 40;
                Item.useAnimation = 40;

                Item.shoot = ModContent.ProjectileType<DarksaberHiltShort>();

                Item.autoReuse = true;
                Item.reuseDelay = 0;
            }
            else
            {
                Item.useTime = 60;
                Item.useAnimation = 60;

                Item.shoot = ModContent.ProjectileType<DarksaberHilt>();

                Item.autoReuse = true;
                Item.reuseDelay = 0;
            }

            if (player.ownedProjectileCounts[ModContent.ProjectileType<DarksaberHiltShort>()] >= 1 || player.ownedProjectileCounts[ModContent.ProjectileType<DarksaberHilt>()] >= 1)
            {
                return false;
            }

            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                Item.autoReuse = true;
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_ItemUse(player, Item), player.Center, Vector2.Zero, ModContent.ProjectileType<DarksaberHiltShort>(), 10, 0f, player.whoAmI, 0);
            }
            else
            {
                Item.autoReuse = true;
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_ItemUse(player, Item), player.Center, Vector2.Zero, ModContent.ProjectileType<DarksaberHilt>(), 10, 0f, player.whoAmI, 0);
            }

            //PlayDarksaberSound();

            return false;
        }
        /*
        public void PlayDarksaberSound()
        {
            switch(Main.rand.Next(3))
            {
                case 0:
                    SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/darksaber"));
                    break;
                case 1:
                    SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/darksaber2"));
                    break;
                case 2:
                    SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/darksaber3"));
                    break;
            }
        }*/

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }

    public class DarksaberHilt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksaber");
        }

        public DarksaberPrimTrail trail;
        public DarksaberPrimTrail trail2;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.damage = 1;
            Projectile.hide = false;
            Projectile.damage = 5;
            Projectile.knockBack = 0f;
        }

        public float rot;

        public override void AI()
        {
            if (Main.player[Projectile.owner].HeldItem.type != ModContent.ItemType<Darksaber>())
            {
                Projectile.Kill();
            }

            Player owner = Main.player[Projectile.owner];

            if (Projectile.ai[0] < 40)
            {
                //rot += (Projectile.ai[1] * (float)Math.Sin((Projectile.ai[0] * 3.14f) / 40f)) / 12f;

                if (Projectile.ai[0] < 20)
                {
                    rot += Projectile.ai[1] * (((float)Math.Pow(Projectile.ai[0], 4) / 200000f)) / 10f;
                }
                else if (Projectile.ai[0] < 25)
                {
                    rot += Projectile.ai[1] * (((float)Math.Pow(Projectile.ai[0] - 20, 2) / 50f) + 2) / 10f;
                }
                else
                {
                    rot += Projectile.ai[1] * (((float)Math.Pow(Projectile.ai[0] - 40, 2) / 150f)) / 10f;
                }

                Projectile.rotation = rot + 1.57f;

                Projectile.Center = owner.Center + (Vector2.UnitX.RotatedBy(rot) * 24f);


                if (Projectile.ai[0] == 0)
                {
                    Projectile.ai[1] = Main.rand.NextBool() ? -1 : 1;

                    rot = (Main.MouseWorld - owner.Center).ToRotation() - (Projectile.ai[1] * 1.4f);

                    PrimitiveSystem.primitives.CreateTrail(trail = new DarksaberPrimTrail(Projectile, Color.Black, 100, 10, true, 2000, 10));
                    PrimitiveSystem.primitives.CreateTrail(trail2 = new DarksaberPrimTrail(Projectile, Color.Black, 100, 10, false, 2000, 5));

                    trail.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail.rot = rot;
                    trail.ticks = (int)Projectile.ai[0];

                    trail2.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail2.rot = rot;
                    trail2.ticks = (int)Projectile.ai[0];

                    Projectile.ai[0]++;

                    if (Projectile.ai[1] == -1)
                    {
                        trail.flipped = true;
                        trail2.flipped = true;
                    }

                    return;
                }
                else
                {
                    trail.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail.rot = rot;
                    trail.ticks = (int)Projectile.ai[0];

                    trail2.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail2.rot = rot;
                    trail2.ticks = (int)Projectile.ai[0];
                }

                if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < -0.77f) owner.bodyFrame.Y = 3 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 0.77f) owner.bodyFrame.Y = 4 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 2.3f) owner.bodyFrame.Y = 3 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 3f) owner.bodyFrame.Y = 2 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 3.87f) owner.bodyFrame.Y = 1 * 56;
                else owner.bodyFrame.Y = 2 * 56;

                if (Projectile.Center.X - owner.Center.X > 0)
                {
                    owner.direction = 1;
                }
                else
                {
                    owner.direction = -1;
                }

                /*for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i] == null || Main.npc[i].active == false || Main.npc[i].friendly/* || Main.npc[i].GetGlobalNPC<DarksaberNPC>().darksaberIframes <= 0) continue;

                    for (int j = 0; j < 100; j += 5)
                    {
                        Vector2 test = Projectile.Center + new Vector2(7, 2).RotatedBy(rot)
                            + (Vector2.UnitX.RotatedBy(rot) * j);

                        if (Main.npc[i].Hitbox.Contains(test.ToPoint()))
                        {
                            //Vector2 newVel = ((Projectile.Center + new Vector2(7, 2).RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation() + 1.57f)
                            //    + (Vector2.UnitX.RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation()) * j)) -
                            //((Projectile.oldPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f)) + new Vector2(7, 2).RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation() + 1.57f)
                            //    + (Vector2.UnitX.RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation()) * j)));

                            //if (newVel.LengthSquared() >= 2f * 2f) newVel = Vector2.Normalize(newVel) * 2f;

                            //Main.npc[i].velocity += newVel;

                            Main.npc[i].StrikeNPC(Projectile.damage, 0f, 0);

                            //Main.npc[i].GetGlobalNPC<DarksaberNPC>().darksaberIframes = 10;

                            break;
                        }
                    }
                }*/
            }
            else
            {
                if (trail == null) return;

                trail.Dispose();
                trail2.Dispose();

                trail = null;
                trail2 = null;
            }

            Projectile.ai[0]++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                Projectile.Center + new Vector2(7, 2).RotatedBy(rot),
                 Projectile.Center + new Vector2(7, 2).RotatedBy(rot) + (Vector2.UnitX.RotatedBy(rot) * 100),
                10, ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] >= 40) return false;

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Weapons/Melee/Swords/DarksaberHilt").Value, Projectile.Center - Main.screenPosition, null, Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)), Projectile.rotation, default, 1f, default, default);
            return false;
        }
    }

    public class DarksaberHiltShort : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksaber");
        }

        public DarksaberPrimTrail2 trail;
        public DarksaberPrimTrail2 trail2;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 40;
            Projectile.damage = 1;
            Projectile.hide = false;
            Projectile.damage = 5;
            Projectile.knockBack = 0f;
        }

        public float rot;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.HeldItem.type != ModContent.ItemType<Darksaber>())
            {
                Projectile.Kill();
            }

            if (Projectile.ai[0] < 30)
            {
                //rot += (Projectile.ai[1] * (float)Math.Sin((Projectile.ai[0] * 3.14f) / 40f)) / 12f;

                if (Projectile.ai[0] < 15)
                {
                    rot += Projectile.ai[1] * (((float)Math.Pow(Projectile.ai[0], 4) / 200000f)) / 7.5f;
                }
                else if (Projectile.ai[0] < 18)
                {
                    rot += Projectile.ai[1] * (((float)Math.Pow(Projectile.ai[0] - 15, 2) / 50f) + 2) / 7.5f;
                }
                else
                {
                    rot += Projectile.ai[1] * (((float)Math.Pow(Projectile.ai[0] - 30, 2) / 150f)) / 7.5f;
                }

                Projectile.rotation = rot + 1.57f;

                Projectile.Center = owner.Center + (Vector2.UnitX.RotatedBy(rot) * 24f);


                if (Projectile.ai[0] == 0)
                {
                    Projectile.ai[1] = Main.rand.NextBool() ? -1 : 1;

                    rot = (Main.MouseWorld - owner.Center).ToRotation() - (Projectile.ai[1] * 0.9f);

                    PrimitiveSystem.primitives.CreateTrail(trail = new DarksaberPrimTrail2(Projectile, Color.Black, 60, 10, true, 2000, 10));
                    PrimitiveSystem.primitives.CreateTrail(trail2 = new DarksaberPrimTrail2(Projectile, Color.Black, 60, 10, false, 2000, 5));

                    trail.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail.rot = rot;
                    trail.ticks = (int)Projectile.ai[0];

                    trail2.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail2.rot = rot;
                    trail2.ticks = (int)Projectile.ai[0];

                    Projectile.ai[0]++;

                    if (Projectile.ai[1] == -1)
                    {
                        trail.flipped = true;
                        trail2.flipped = true;
                    }

                    return;
                }
                else
                {
                    trail.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail.rot = rot;
                    trail.ticks = (int)Projectile.ai[0];

                    trail2.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f) + (Projectile.position - Projectile.oldPosition);
                    trail2.rot = rot;
                    trail2.ticks = (int)Projectile.ai[0];
                }

                if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < -0.77f) owner.bodyFrame.Y = 3 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 0.77f) owner.bodyFrame.Y = 4 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 2.3f) owner.bodyFrame.Y = 3 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 3f) owner.bodyFrame.Y = 2 * 56;
                else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 3.87f) owner.bodyFrame.Y = 1 * 56;
                else owner.bodyFrame.Y = 2 * 56;

                if (Projectile.Center.X - owner.Center.X > 0)
                {
                    owner.direction = 1;
                }
                else
                {
                    owner.direction = -1;
                }
            }
            else
            {
                if (trail == null) return;

                trail.Dispose();
                trail2.Dispose();

                trail = null;
                trail2 = null;
            }

            Projectile.ai[0]++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (trail == null) return false;

            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                Projectile.Center + new Vector2(7, 2).RotatedBy(rot),
                 Projectile.Center + new Vector2(7, 2).RotatedBy(rot) + (Vector2.UnitX.RotatedBy(rot) * trail.bladeVal2),
                10, ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] >= 30) return false;

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Weapons/Melee/Swords/DarksaberHilt").Value, Projectile.Center - Main.screenPosition, null, Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)), Projectile.rotation, default, 1f, default, default);
            return false;
        }
    }

    /*public class DarksaberNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int darksaberIframes;

        public override bool PreAI(NPC npc)
        {
            darksaberIframes--;

            return base.PreAI(npc);
        }
    }*/

    public class DarksaberPrimTrail : Primitive
    {
        public DarksaberPrimTrail(Entity projectile, Color _color, int _bladeVal2, int _interval, bool _additive, int cap = 300, int _width = 5) : base(projectile)
        {
            BindableEntity = projectile;
            width = _width;
            color = _color;
            _cap = cap;

            interval = _interval;

            bladeVal2 = _bladeVal2;

            interval = _interval;

            additive = _additive;

            orig = Main.player[(BindableEntity as Projectile).owner].Center;

            _points.Clear();

            myLength = 0;
        }

        public int myLength;
        private Color color;
        public bool additive;
        public bool flipped;
        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = false;
            manualDraw = true;
            pixelated = true;
        }

        public float width;

        public int bladeVal2;

        public int interval;

        public Vector2 orig;

        public float rot;

        public int ticks;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;

            for (int i = 0; i < (myLength / interval) - 1; i++)
            {
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                //normal = new Vector2(0, -1);
                //normalAhead = new Vector2(0, -1);

                Vector2 firstUp = _points[i] - normal * width;
                Vector2 firstDown = _points[i] + normal * width;

                Vector2 firstSpine = _points[i];
                Vector2 secondSpine = _points[i + 1];

                Vector2 secondUp = _points[i + 1] - normalAhead * width;
                Vector2 secondDown = _points[i + 1] + normalAhead * width;

                if (i == (myLength / interval) - 3)
                {
                    if (flipped)
                    {
                        AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));

                        AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / _cap, 0));
                        AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));


                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 1));
                        AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                        AddVertex(secondSpine, Color.White, new Vector2((i + 1) / _cap, 1));
                    }
                    else
                    {
                        AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));

                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / _cap, 0));


                        AddVertex(secondSpine, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstDown, Color.White, new Vector2((i / _cap), 0));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 1));
                    }
                }
                else if (i == (myLength / interval) - 2)
                {
                    if (flipped)
                    {
                        if (!additive)
                        {
                            AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                            AddVertex(firstSpine, Color.White, new Vector2((i / _cap), 0));
                            AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        }
                        else
                        {
                            AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                            AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                            AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        }
                    }
                    else
                    {
                        if (!additive)
                        {
                            AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                            AddVertex(firstSpine, Color.White, new Vector2((i / _cap), 0));
                            AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));
                        }
                        else
                        {
                            AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                            AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                            AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));
                        }
                    }
                }
                else
                {
                    AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                    AddVertex(firstSpine, color, new Vector2((i / _cap), 0));
                    AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));

                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 0));
                    AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                    AddVertex(firstSpine, color, new Vector2((i / _cap), 0));


                    AddVertex(firstSpine, color, new Vector2((i / _cap), 1));
                    AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 1));

                    AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 0));
                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 1));
                    AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                }
            }
        }

        public override void SetShaders()
        {
            if (vertices.Length == 0) return;

            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            if (additive)
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(204f, 107f, 183f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(204f, 107f, 183f, 0f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.4f);

                EEMod.DarksaberShader.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/LightningNoisePixelatedBloom").Value);

                EEMod.DarksaberShader.Parameters["ticks"].SetValue(Main.GameUpdateCount / 20f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }
            else
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(120f, 44f, 166f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(204f, 107f, 183f, 64f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.3f);

                EEMod.DarksaberShader.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/LightningNoisePixelatedBloom").Value);

                EEMod.DarksaberShader.Parameters["ticks"].SetValue(Main.GameUpdateCount / 20f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.DarksaberShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            _device.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints);

            VertexBufferPool.Shared.Return(buffer);
        }

        public override void OnUpdate()
        {
            if (additive) width = 10f + ((float)Math.Sin(Main.GameUpdateCount / 10f) * 1.5f);

            if (myLength < bladeVal2 && ticks < 30 && ticks > 3) myLength += (interval * 2);

            ticks++;
            if (ticks > 35) myLength -= (interval * 2);

            _counter++;
            _noOfPoints = _points.Count() * 12;
            if (_cap < _noOfPoints / 12)
            {
                _points.RemoveAt(0);
            }

            if ((!BindableEntity.active && BindableEntity != null) || BindableEntity == null || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                _points.Clear();

                for (int i = 0; i < myLength / interval; i++)
                {
                    Vector2 vec = Vector2.Lerp(orig, orig + (Vector2.UnitX.RotatedBy(rot) * myLength), (float)i / (float)(myLength / interval));

                    _points.Add(vec);
                }
            }
        }

        public override void OnDestroy()
        {
            _points.Clear();

            Dispose();
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }

    public class DarksaberPrimTrail2 : Primitive
    {
        public DarksaberPrimTrail2(Entity projectile, Color _color, int _bladeVal2, int _interval, bool _additive, int cap = 300, int _width = 5) : base(projectile)
        {
            BindableEntity = projectile;
            width = _width;
            color = _color;
            _cap = cap;

            interval = _interval;

            bladeVal2 = _bladeVal2;

            interval = _interval;

            additive = _additive;

            orig = Main.player[(BindableEntity as Projectile).owner].Center;

            _points.Clear();

            myLength = 0;
        }

        public int myLength;
        private Color color;
        public bool additive;
        public bool flipped;
        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = false;
            manualDraw = true;
            pixelated = true;
        }

        public float width;

        public int bladeVal2;

        public int interval;

        public Vector2 orig;

        public float rot;

        public int ticks;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;

            for (int i = 0; i < (myLength / interval) - 1; i++)
            {
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);

                //normal = new Vector2(0, -1);
                //normalAhead = new Vector2(0, -1);

                Vector2 firstUp = _points[i] - normal * width;
                Vector2 firstDown = _points[i] + normal * width;

                Vector2 firstSpine = _points[i];
                Vector2 secondSpine = _points[i + 1];

                Vector2 secondUp = _points[i + 1] - normalAhead * width;
                Vector2 secondDown = _points[i + 1] + normalAhead * width;

                if (i == (myLength / interval) - 3)
                {
                    if (flipped)
                    {
                        AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));

                        AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / _cap, 0));
                        AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));


                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 1));
                        AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                        AddVertex(secondSpine, Color.White, new Vector2((i + 1) / _cap, 1));
                    }
                    else
                    {
                        AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));

                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                        AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / _cap, 0));


                        AddVertex(secondSpine, Color.White, new Vector2((i + 1) / _cap, 1));
                        AddVertex(firstDown, Color.White, new Vector2((i / _cap), 0));
                        AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 1));
                    }
                }
                else if (i == (myLength / interval) - 2)
                {
                    if (flipped)
                    {
                        if (!additive)
                        {
                            AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                            AddVertex(firstSpine, Color.White, new Vector2((i / _cap), 0));
                            AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        }
                        else
                        {
                            AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                            AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                            AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                        }
                    }
                    else
                    {
                        if (!additive)
                        {
                            AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                            AddVertex(firstSpine, Color.White, new Vector2((i / _cap), 0));
                            AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));
                        }
                        else
                        {
                            AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 1));
                            AddVertex(firstSpine, Color.Black, new Vector2((i / _cap), 0));
                            AddVertex(firstUp, Color.White, new Vector2((i / _cap), 1));
                        }
                    }
                }
                else
                {
                    AddVertex(firstDown, Color.White, new Vector2((i / _cap), 1));
                    AddVertex(firstSpine, color, new Vector2((i / _cap), 0));
                    AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));

                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 0));
                    AddVertex(secondDown, Color.White, new Vector2((i + 1) / _cap, 1));
                    AddVertex(firstSpine, color, new Vector2((i / _cap), 0));


                    AddVertex(firstSpine, color, new Vector2((i / _cap), 1));
                    AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 1));

                    AddVertex(secondUp, Color.White, new Vector2((i + 1) / _cap, 0));
                    AddVertex(secondSpine, color, new Vector2((i + 1) / _cap, 1));
                    AddVertex(firstUp, Color.White, new Vector2((i / _cap), 0));
                }
            }
        }

        public override void SetShaders()
        {
            if (vertices.Length == 0) return;

            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            if (additive)
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(204f, 107f, 183f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(204f, 107f, 183f, 0f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.4f);

                EEMod.DarksaberShader.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/LightningNoisePixelatedBloom").Value);

                EEMod.DarksaberShader.Parameters["ticks"].SetValue(Main.GameUpdateCount / 20f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }
            else
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(120f, 44f, 166f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(204f, 107f, 183f, 64f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.3f);

                EEMod.DarksaberShader.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/LightningNoisePixelatedBloom").Value);

                EEMod.DarksaberShader.Parameters["ticks"].SetValue(Main.GameUpdateCount / 20f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.DarksaberShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            _device.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints);

            VertexBufferPool.Shared.Return(buffer);
        }

        public override void OnUpdate()
        {
            if (additive) width = 10f + ((float)Math.Sin(Main.GameUpdateCount / 10f) * 1.5f);

            if (myLength < bladeVal2 && ticks < 20 && ticks > 3) myLength += (interval * 2);

            ticks++;
            if (ticks > 26) myLength -= (interval * 2);

            _counter++;
            _noOfPoints = _points.Count() * 12;
            if (_cap < _noOfPoints / 12)
            {
                _points.RemoveAt(0);
            }

            if ((!BindableEntity.active && BindableEntity != null) || BindableEntity == null || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                _points.Clear();

                for (int i = 0; i < myLength / interval; i++)
                {
                    Vector2 vec = Vector2.Lerp(orig, orig + (Vector2.UnitX.RotatedBy(rot) * myLength), (float)i / (float)(myLength / interval));

                    _points.Add(vec);
                }
            }
        }

        public override void OnDestroy()
        {
            _points.Clear();

            Dispose();
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
}