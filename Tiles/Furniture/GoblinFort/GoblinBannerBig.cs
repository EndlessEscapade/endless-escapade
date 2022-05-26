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
    public class GoblinBannerBig : EETile
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

            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 1;

            TileObjectData.newTile.Origin = new Point16(0, 0);

            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 5, 0);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;

            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<GoblinBannerBigTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceEveryone = new PlacementHook(ModContent.GetInstance<GoblinBannerBigTE>().Hook_AfterPlacement, -1, 0, true);

            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();

            name.SetDefault("Goblin Banner Big");

            AddMapEntry(new Color(44, 193, 139), name);

            DustType = DustID.Clentaminator_Cyan;
            DisableSmartCursor = false;
            MinPick = 0;
            MineResist = 1f;
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            ModContent.GetInstance<GoblinBannerBigTE>().Kill(i, j);
        }
    }

    public class GoblinBannerBigTE : EETileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile;
        }

        public bool placedPrims = false;

        private GoblinBannerBigPrims trail1;
        private GoblinBannerBigPrims trail2;

        public override void Update()
        {
            if (!placedPrims)
            {
                Vector2 initPos = new Vector2(Position.X * 16, (Position.Y * 16));

                PrimitiveSystem.primitives.CreateTrail(trail1 = new GoblinBannerBigPrims(initPos + new Vector2(40, 0), true));
                PrimitiveSystem.primitives.CreateTrail(trail2 = new GoblinBannerBigPrims(initPos + new Vector2(40, 0), false));

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

    public class GoblinBannerBigPrims : Primitive
    {
        Vector2 myPos;

        public GoblinBannerBigPrims(Vector2 projectile, bool outline) : base(null)
        {
            myPos = projectile;

            _cap = 200;
            _width = 40;

            this.outline = outline;

            if (this.outline) color = new Color(16, 20, 27);
            else color = Color.White;
        }

        public bool outline;

        public override void SetDefaults()
        {
            Alpha = 1f;

            behindTiles = true;
            ManualDraw = false;
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


                Vector2 firstUp = _points[i] - normal * 40;
                Vector2 firstDown = _points[i] + normal * 40;

                Vector2 secondUp = _points[i + 1] - normalAhead * 40;
                Vector2 secondDown = _points[i + 1] + normalAhead * 40;

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

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default);

            myShader.TextureEnabled = true;

            if (!outline)
                myShader.Texture = ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/GoblinFort/GoblinBannerBigBanner").Value;
            else
                myShader.Texture = ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/GoblinFort/GoblinBannerBigOutline").Value;

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

            for (int i = 0; i < 58; i++)
            {
                Vector2 offset = Vector2.Zero;

                if (outline) offset = new Vector2(0, 2);

                Vector2 endpoint = Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 464).RotatedBy(Math.Sin((ticks / 60f) + myPos.X) * 0f), i / 58f);

                if (i <= 1) _points.Add(offset + myPos + Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 464), i / 58f));
                else _points.Add(offset + myPos + endpoint + (Vector2.UnitX * 2f * (float)Math.Sin((-ticks / 45f) + (i / 4f) + myPos.X)).RotatedBy((endpoint).ToRotation() + 1.57f));
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