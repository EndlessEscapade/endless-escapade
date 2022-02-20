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
using System.Collections.Generic;

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
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 0;
			Item.knockBack = 6.5f;
            Item.autoReuse = false;
			Item.width = 16;
			Item.height = 22;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.sellPrice(silver: 10);

            Item.shoot = ModContent.ProjectileType<DarksaberHilt>();
            Item.noUseGraphic = true;

			Item.DamageType = DamageClass.Melee;
            // Item.autoReuse = false;
            Item.UseSound = SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/darksaber");
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DarksaberHilt>()] >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile proj = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_Item(player, Item), player.Center, Vector2.Zero, ModContent.ProjectileType<DarksaberHilt>(), 20, 2f);
            (proj.ModProjectile as DarksaberHilt).rot = (Main.MouseWorld - player.Center).ToRotation();

            return false;
        }
    }

    public class DarksaberHilt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormspear");
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
            Projectile.timeLeft = 100000;
            Projectile.damage = 1;
            Projectile.hide = false;
        }

        public float rot;

        public override void AI()
        {
            if (Main.player[Projectile.owner].HeldItem.type != ModContent.ItemType<Darksaber>())
            {
                Projectile.Kill();
            }

            //Player owner = Main.player[Projectile.owner];
            Player owner = Main.LocalPlayer;

            rot += (Projectile.ai[1] * (float)Math.Sin((Projectile.ai[0] * 3.14f) / 20f)) / 5f;

            Projectile.rotation = rot + 1.57f;

            Projectile.Center = owner.Center + (Vector2.UnitX.RotatedBy(rot) * 32f);

            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[1] = Main.rand.NextBool() ? -1 : 1;

                rot = (Main.MouseWorld - owner.Center).ToRotation() - 0.7f * Projectile.ai[1];

                PrimitiveSystem.primitives.CreateTrail(trail = new DarksaberPrimTrail(Projectile, Color.Black, 80, 100, 10, true, 5, 10));
                PrimitiveSystem.primitives.CreateTrail(trail2 = new DarksaberPrimTrail(Projectile, Color.Black, 80, 100, 10, false, 5, 5));
            }
            else
            {
                trail.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f);
                trail.rot = rot;
                trail.ticks = (int)Projectile.ai[0];

                trail2.orig = Projectile.Center + new Vector2(7, 2).RotatedBy(rot + 1.57f);
                trail2.rot = rot;
                trail2.ticks = (int)Projectile.ai[0];
            }

            if (Projectile.ai[1] == -1)
            {
                trail.flipped = true;
                trail2.flipped = true;
            }

            if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < -0.77f) owner.bodyFrame.Y = 3 * 56;
            else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 0.77f) owner.bodyFrame.Y = 4 * 56;
            else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 2.3f) owner.bodyFrame.Y = 3 * 56;
            else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 3f) owner.bodyFrame.Y = 2 * 56;
            else if (Math.Abs((Projectile.Center - owner.Center).ToRotation() - 3.14f) - 1.57f < 3.87f) owner.bodyFrame.Y = 1 * 56;
            else owner.bodyFrame.Y = 2 * 56;

            if (Projectile.Center.X - Main.LocalPlayer.Center.X > 0)
            {
                Main.LocalPlayer.direction = 1;
            }
            else
            {
                Main.LocalPlayer.direction = -1;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i] == null || Main.npc[i].active == false) continue;

                for (int j = 0; j < 100; j += 5)
                {
                    Vector2 test = Projectile.Center + new Vector2(7, 2).RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation() + 1.57f)
                        + (Vector2.UnitX.RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation()) * j);

                    if(Main.npc[i].Hitbox.Contains(test.ToPoint()))
                    {
                        Vector2 newVel = ((Projectile.Center + new Vector2(7, 2).RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation() + 1.57f)
                            + (Vector2.UnitX.RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation()) * j)) -
                        ((Projectile.oldPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f)) + new Vector2(7, 2).RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation() + 1.57f)
                            + (Vector2.UnitX.RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation()) * j)));

                        if (newVel.LengthSquared() >= 6f * 6f) newVel = Vector2.Normalize(newVel) * 6f;

                        Main.npc[i].velocity += newVel;

                        Main.npc[i].StrikeNPC(Projectile.damage, 0f, 0);

                        break;
                    }
                }
            }

            Projectile.ai[0]++;
        }

        public override void Kill(int timeLeft)
        {
            trail.Dispose();
            trail2.Dispose();

            base.Kill(timeLeft);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Weapons/Melee/Swords/DarksaberHilt").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, default, 1f, default, default);
            return true;
        }
    }

    public class DarksaberPrimTrail : Primitive
    {
        public DarksaberPrimTrail(Entity projectile, Color _color, int _bladeVal1, int _bladeVal2, int _interval, bool _additive, int cap = 300, int _width = 5) : base(projectile)
        {
            BindableEntity = projectile;
            width = _width;
            color = _color;
            _cap = cap;

            interval = _interval;

            bladeVal1 = _bladeVal1;
            bladeVal2 = _bladeVal2;

            interval = _interval;

            additive = _additive;
        }

        public int myLength;
        private Color color;
        public bool additive;
        public bool flipped;
        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = false;
            ManualDraw = false;
            manualDraw = true;
            pixelated = true;
        }

        public float width;

        public int bladeVal1;
        public int bladeVal2;

        public int interval;

        public Vector2 orig;

        public float rot;

        public int ticks;

        protected static Vector2 MyCurveNormal(List<Vector2> myPoints, int index)
        {
            if (myPoints.Count == 1) return myPoints[0];

            if (index == 0)
            {
                return Clockwise90(Vector2.Normalize(myPoints[1] - myPoints[0]));
            }
            if (index == myPoints.Count - 1)
            {
                return Clockwise90(Vector2.Normalize(myPoints[index] - myPoints[index - 1]));
            }
            return Clockwise90(Vector2.Normalize(myPoints[index + 1] - myPoints[index - 1]));
        }

        public List<List<Vector2>> points = new List<List<Vector2>>();

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || points.Count() <= 1) return;

            for (int j = 0; j < points.Count(); j++)
            {
                if (j == points.Count() - 1)
                {
                    for (int i = 0; i < points[j].Count() - 1; i++) //going up the spine of the BLADE
                    {
                        Main.NewText(points[j].Count());

                        Vector2 normal = MyCurveNormal(points[j], i);
                        Vector2 normalAhead = MyCurveNormal(points[j], i + 1);

                        Vector2 firstUp = points[j][i] - normal * width;
                        Vector2 firstDown = points[j][i] + normal * width;

                        Vector2 firstSpine = points[j][i];
                        Vector2 secondSpine = points[j][i + 1];

                        Vector2 secondUp = points[j][i + 1] - normalAhead * width;
                        Vector2 secondDown = points[j][i + 1] + normalAhead * width;

                        if (i == points[j].Count() - 2)
                        {
                            if (flipped)
                            {
                                AddVertex(firstDown, Color.White, new Vector2((i / points[j].Count()), 1));
                                AddVertex(firstSpine, Color.Black, new Vector2((i / points[j].Count()), 0));
                                AddVertex(secondDown, Color.White, new Vector2((i + 1) / points[j].Count(), 1));

                                AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / points[j].Count(), 0));
                                AddVertex(secondDown, Color.White, new Vector2((i + 1) / points[j].Count(), 1));
                                AddVertex(firstSpine, Color.Black, new Vector2((i / points[j].Count()), 0));


                                AddVertex(firstSpine, Color.Black, new Vector2((i / points[j].Count()), 1));
                                AddVertex(firstUp, Color.White, new Vector2((i / points[j].Count()), 0));
                                AddVertex(secondSpine, Color.White, new Vector2((i + 1) / points[j].Count(), 1));
                            }
                            else
                            {
                                AddVertex(secondUp, Color.White, new Vector2((i + 1) / points[j].Count(), 1));
                                AddVertex(firstSpine, Color.Black, new Vector2((i / points[j].Count()), 0));
                                AddVertex(firstUp, Color.White, new Vector2((i / points[j].Count()), 1));

                                AddVertex(firstSpine, Color.Black, new Vector2((i / points[j].Count()), 0));
                                AddVertex(secondUp, Color.White, new Vector2((i + 1) / points[j].Count(), 1));
                                AddVertex(secondSpine, Color.Black, new Vector2((i + 1) / points[j].Count(), 0));


                                AddVertex(secondSpine, Color.White, new Vector2((i + 1) / points[j].Count(), 1));
                                AddVertex(firstDown, Color.White, new Vector2((i / points[j].Count()), 0));
                                AddVertex(firstSpine, Color.Black, new Vector2((i / points[j].Count()), 1));
                            }
                        }
                        else if (i == points[j].Count() - 1)
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
                else
                {
                    for (int i = 0; i < points[j].Count() - 1; i++)
                    {
                        Vector2 firstUp = points[j][i];
                        Vector2 firstDown = points[j + 1][i];

                        Vector2 secondUp = points[j][i + 1];
                        Vector2 secondDown = points[j + 1][i + 1];

                        AddVertex(firstDown, Color.Black, new Vector2((i / points[j].Count()), 1));
                        AddVertex(firstUp, Color.Black, new Vector2((i / points[j].Count()), 0));
                        AddVertex(secondDown, Color.Black, new Vector2((i + 1) / points[j].Count(), 1));

                        AddVertex(secondUp, Color.Black, new Vector2((i + 1) / points[j].Count(), 0));
                        AddVertex(secondDown, Color.Black, new Vector2((i + 1) / points[j].Count(), 1));
                        AddVertex(firstUp, Color.Black, new Vector2((i / points[j].Count()), 0));
                    }
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

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(192f, 192f, 192f, 64f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(255f, 255f, 255f, 0f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.4f);

                EEMod.DarksaberShader.Parameters["transformMatrix"].SetValue(view * projection);
            }
            else
            {
                Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                EEMod.DarksaberShader.Parameters["bladeColor"].SetValue(new Vector4(0f, 0f, 0f, 255f) / 255f);
                EEMod.DarksaberShader.Parameters["edgeColor"].SetValue(new Vector4(255f, 255f, 255f, 255f) / 255f);

                EEMod.DarksaberShader.Parameters["edgeThresh"].SetValue(0.4f);

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
        }

        public override void OnUpdate()
        {
            if (additive) width = 10f + ((float)Math.Sin(Main.GameUpdateCount / 10f) * 1.5f);

            if (myLength < bladeVal2) myLength += (interval * 2);

            _counter++;

            if(points.Count() >= 1) _noOfPoints = (points.Count() - 1 * 6) + ((points[points.Count - 1].Count() - 1) * 24);

            while (_cap < points.Count())
            {
                points.RemoveAt(0);
            }

            if ((!BindableEntity.active && BindableEntity != null) || BindableEntity == null || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                points.Add(new List<Vector2>());

                for (int i = 0; i < myLength / interval; i++)
                {
                    if (i == 0)
                    {
                        points[points.Count() - 1].Add(orig);

                        continue;
                    }

                    Vector2 vec = Vector2.Lerp(orig, orig + (Vector2.UnitX.RotatedBy(rot) * myLength), (float)i / (float)(myLength / interval));

                    points[points.Count() - 1].Add(vec);
                }
            }
        }

        public override void OnDestroy()
        {
            Dispose();
        }

        public override void PostDraw()
        {
            Main.spriteBatch.End(); Main.spriteBatch.Begin();
        }
    }
}