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
using EEMod.Extensions;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace EEMod.Tiles.Furniture.GoblinFort
{
    public class GoblinBanner : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);

            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 1;

            TileObjectData.newTile.Origin = new Point16(0, 0);

            TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;

            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<GoblinBannerTE>().Hook_AfterPlacement, -1, 0, true);

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorLeft = AnchorData.Empty;
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);

            TileObjectData.addAlternate(0);

            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();

            name.SetDefault("Goblin Banner");

            AddMapEntry(new Color(44, 193, 139), name);

            DustType = DustID.Clentaminator_Cyan;
            DisableSmartCursor = false;
            MinPick = 0;
            MineResist = 1f;
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            ModContent.GetInstance<GoblinBannerTE>().Kill(i, j);
        }
    }

    public class GoblinBannerTE : EETileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile;
        }

        public bool placedPrims = false;

        private GoblinBannerPrims trail1;
        private GoblinBannerPrims trail2;

        public override void Update()
        {
            if (!placedPrims)
            {
                Vector2 initPos = new Vector2(Position.X * 16, (Position.Y * 16));

                PrimitiveSystem.primitives.CreateTrail(trail1 = new GoblinBannerPrims(initPos + new Vector2(24, 2), true));
                PrimitiveSystem.primitives.CreateTrail(trail2 = new GoblinBannerPrims(initPos + new Vector2(24, 2), false));

                placedPrims = true;
            }

            base.Update();
        }

        public override void OnKill()
        {
            trail1.Dispose();
            trail2.Dispose();

            base.OnKill();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }

            return Place(i, j);
        }
    }

    public class GoblinBannerPrims : Primitive
    {
        Vector2 myPos;

        public GoblinBannerPrims(Vector2 projectile, bool outline) : base(null)
        {
            myPos = projectile;

            _cap = 200;
            _width = 23;

            this.outline = outline;

            if (this.outline) color = new Color(54, 38, 52);
            else color = Color.White;
        }

        public bool outline;

        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = false;
            manualDraw = true;
            pixelated = true;

            myShader = new BasicEffect(Main.graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
            };

            myShader.Projection = Matrix.CreateOrthographic(_device.Viewport.Width / 2, _device.Viewport.Height / 2, 0, 1000);
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1 || _points.Count() <= 1) return;

            for (int i = 0; i < _points.Count() - 1; i++)
            {
                Vector2 normal = CurveNormal(_points, i);
                Vector2 normalAhead = CurveNormal(_points, i + 1);


                Vector2 firstUp = _points[i] - normal * 23;
                Vector2 firstDown = _points[i] + normal * 23;

                Vector2 secondUp = _points[i + 1] - normalAhead * 23;
                Vector2 secondDown = _points[i + 1] + normalAhead * 23;

                Color color2 = new Color((color * colorVal).ToVector3());

                AddVertex(firstDown, color2, new Vector2(1, i / (float)(_points.Count() - 1)));
                AddVertex(firstUp, color2, new Vector2(0, i / (float)(_points.Count() - 1)));
                AddVertex(secondDown, color2, new Vector2(1, (i + 1) / (float)(_points.Count() - 1)));

                AddVertex(secondUp, color2, new Vector2(0, (i + 1) / (float)(_points.Count() - 1)));
                AddVertex(secondDown, color2, new Vector2(1, (i + 1) / (float)(_points.Count() - 1)));
                AddVertex(firstUp, color2, new Vector2(0, i / (float)(_points.Count() - 1)));
            }
        }

        public Color color;
        float colorVal;

        BasicEffect myShader;

        public override void SetShaders()
        {
            if (vertices.Length == 0) return;

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, default, SamplerState.PointClamp, default, default, myShader, Main.GameViewMatrix.ZoomMatrix);

            myShader.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(_device.Viewport.Width / 2, _device.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            myShader.TextureEnabled = true;

            if (!outline)
                myShader.Texture = ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/GoblinFort/GoblinBannerBanner").Value;
            else
                myShader.Texture = ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/GoblinFort/GoblinBannerOutline").Value;

            DynamicVertexBuffer buffer = VertexBufferPool.Shared.RentDynamicVertexBuffer(VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);

            foreach (EffectPass pass in myShader.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _noOfPoints / 3);

            VertexBufferPool.Shared.Return(buffer);
        }

        public int ticks;

        public override void OnUpdate()
        {
            Color lightColor = Lighting.GetColor((myPos / 16f).ToPoint());
            colorVal = ((lightColor.R + lightColor.G + lightColor.B) / 3f) / 255f;

            ticks++;

            _counter++;
            _noOfPoints = _points.Count() * 6;

            _points.Clear();

            for (int i = 0; i < 11; i++)
            {
                Vector2 offset = Vector2.Zero;

                if (outline) offset = new Vector2(0, 2);

                Vector2 endpoint = Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 110).RotatedBy(Math.Sin(ticks / 45f).PositiveSin() * 0.15f), i / 11f);

                if (i <= 1) _points.Add(offset + myPos + Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 110), i / 11f));
                else _points.Add(offset + myPos + endpoint + (Vector2.UnitX * 3f * (float)Math.Sin((ticks / 30f) + (i / 1.5f))).RotatedBy((endpoint).ToRotation() + 1.57f));
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