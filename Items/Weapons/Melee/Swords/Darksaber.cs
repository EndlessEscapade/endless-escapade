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
			Item.useStyle = ItemUseStyleID.Swing;
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

            Item.UseSound = SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/Darksaber");
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
    }
	
	public class DarksaberHilt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormspear");
        }

        public DarksaberPrimTrail trail;

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
            Projectile.hide = false;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HeldItem.type != ModContent.ItemType<Darksaber>())
            {
                Projectile.Kill();
            }

            //Player owner = Main.player[Projectile.owner];
            Player owner = Main.LocalPlayer;

			Projectile.Center = owner.Center + (Vector2.UnitX.RotatedBy(Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation()) * 32f);


            Main.NewText(owner.Center);

            if(Projectile.ai[0] == 0)
            {
                PrimitiveSystem.primitives.CreateTrail(trail = new DarksaberPrimTrail(Projectile, Color.Black, 40, 50, 5, 300, 10));
            }
            else
            {
                trail.orig = Projectile.Center + new Vector2(-11, -1);
                trail.rot = Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation();
                trail.ticks = (int)Projectile.ai[0];
            }

            Projectile.ai[0]++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Weapons/Melee/Swords/DarksaberHilt").Value, Projectile.Center - Main.screenPosition, Color.White);
            return true;
        }
    }

    public class DarksaberPrimTrail : Primitive
    {
        public DarksaberPrimTrail(Entity projectile, Color _color, int _bladeVal1, int _bladeVal2, int _interval, int cap = 300, int _width = 10) : base(projectile)
        {
            BindableEntity = projectile;
            width = _width;
            color = _color;
            _cap = cap;

            interval = _interval;

            bladeVal1 = _bladeVal1;
            bladeVal2 = _bladeVal2;

            interval = _interval;
        }

        private Color color;
        public override void SetDefaults()
        {
            Alpha = 0.8f;

            behindTiles = false;
            ManualDraw = false;
            pixelated = false;
            manualDraw = true;
        }

        public int width;

        public int bladeVal1;
        public int bladeVal2;

        public int interval;

        public Vector2 orig;

        public float rot;

        public int ticks;

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            _points.Clear();

            Vector2 segment = Vector2.UnitX.RotatedBy(rot) * interval;

            Main.NewText(orig + (segment * (1 + 1)) + (Vector2.UnitX.RotatedBy(rot - 1.57f) * _width / 2f));

            for(int i = 0; i < bladeVal2; i += interval)
            {
                AddVertex(orig + (segment * i), color, new Vector2((i / _cap), 0));
                AddVertex(orig + (segment * (i + 1)) + (Vector2.UnitX.RotatedBy(rot - 1.57f) * _width / 2f), color, new Vector2((i / _cap), 0));
                AddVertex(orig + (segment * i) + (Vector2.UnitX.RotatedBy(rot - 1.57f) * _width / 2f), color, new Vector2((i / _cap), 0));

                AddVertex(orig + (segment * (i + 1)) + (Vector2.UnitX.RotatedBy(rot - 1.57f) * _width / 2f), color, new Vector2((i / _cap), 0));
                AddVertex(orig + (segment * (i + 1)), color, new Vector2((i / _cap), 0));
                AddVertex(orig + (segment * i), color, new Vector2((i / _cap), 0));
            }
        }

        public override void SetShaders()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(_device.Viewport.Width, _device.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, EEMod.TornSailShader, Main.GameViewMatrix.ZoomMatrix);

            EEMod.LightningShader.Parameters["maskTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/GlowingWeb").Value);
            EEMod.LightningShader.Parameters["newColor"].SetValue(new Vector4(color.R, color.G, color.B, color.A) / 255f);

            EEMod.LightningShader.Parameters["transformMatrix"].SetValue(view * projection);

            if (vertices.Length == 0) return;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in EEMod.LightningShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            if (_noOfPoints >= 1)
            {
                _device.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints / 3);
            }

            VertexBufferPool.Shared.Return(buffer);
        }

        public override void OnUpdate()
        {
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if ((!BindableEntity.active && BindableEntity != null) || _destroyed)
            {
                OnDestroy();
            }
            else
            {

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