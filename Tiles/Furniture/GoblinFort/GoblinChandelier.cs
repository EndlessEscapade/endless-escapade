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
using System.Collections.Generic;

namespace EEMod.Tiles.Furniture.GoblinFort
{
    public class GoblinChandelier : EETile
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

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;

            TileObjectData.newTile.Origin = new Point16(0, 0);

            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;

            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<GoblinChandelierTE>().Hook_AfterPlacement, -1, 0, true);

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
            ModContent.GetInstance<GoblinChandelierTE>().Kill(i, j);
        }
    }

    public class GoblinChandelierTE : EETileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile;
        }

        public bool placedPrims = false;

        private GoblinChandelierLight chandelier;

        public override void Update()
        {
            if (!placedPrims)
            {
                Vector2 initPos = new Vector2(Position.X * 16, (Position.Y * 16));

                chandelier = Projectile.NewProjectileDirect(new EntitySource_WorldGen(), initPos + new Vector2(8, 8 + 64), Vector2.Zero, ModContent.ProjectileType<GoblinChandelierLight>(), 0, 0).ModProjectile as GoblinChandelierLight;
                chandelier.anchorPos = new Vector2(Position.X, Position.Y);
                
                placedPrims = true;
            }

            base.Update();
        }

        public override void OnKill()
        {
            chandelier.Projectile.Kill();

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

    public class GoblinChandelierLight : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chandelier");
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void SetDefaults()
        {
            Projectile.width = 104;
            Projectile.height = 66;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

            Projectile.timeLeft = 100000000;

            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.hide = true;
        }

        public ShadowflameCampfirePrims[] trails = new ShadowflameCampfirePrims[9];

        public Vector2 anchorPos;
        public Vector2 anchorPos16;

        public float axisRotation;

        public float rotationVelocity;

        public int chainLength = 80;

        public bool disabled;

        public override void AI()
        {
            anchorPos16 = (anchorPos * 16) + new Vector2(8, 8);

            if (rotationVelocity != 0)
            {
                rotationVelocity += (0 - axisRotation) * 0.01f;

                axisRotation += rotationVelocity;

                axisRotation *= 0.965f;

                /*if(chainLength > 80)
                {
                    chainLength--;
                }*/
            }

            if (Projectile.ai[0] == 0)
            {
                PrimitiveSystem.primitives.CreateTrail(trails[0] = new ShadowflameCampfirePrims(Color.Violet * 0.2f, anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + -8).RotatedBy(axisRotation), anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + -16).RotatedBy(axisRotation), 10, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trails[1] = new ShadowflameCampfirePrims(Color.Violet, anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + -6).RotatedBy(axisRotation), anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + -12).RotatedBy(axisRotation), 6, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trails[2] = new ShadowflameCampfirePrims(Color.Violet, anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + -8).RotatedBy(axisRotation), anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + -16).RotatedBy(axisRotation), 8, 20, false, 1));

                PrimitiveSystem.primitives.CreateTrail(trails[3] = new ShadowflameCampfirePrims(Color.Violet * 0.2f, anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(0, (chainLength - 7) + -12).RotatedBy(axisRotation), anchorPos16 + new Vector2(0, (chainLength - 7) + -22).RotatedBy(axisRotation), 10, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trails[4] = new ShadowflameCampfirePrims(Color.Violet, anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(0, (chainLength - 7) + -8).RotatedBy(axisRotation), anchorPos16 + new Vector2(0, (chainLength - 7) + -16).RotatedBy(axisRotation), 6, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trails[5] = new ShadowflameCampfirePrims(Color.Violet, anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(0, (chainLength - 7) + -12).RotatedBy(axisRotation), anchorPos16 + new Vector2(0, (chainLength - 7) + -22).RotatedBy(axisRotation), 8, 20, false, 1));

                PrimitiveSystem.primitives.CreateTrail(trails[6] = new ShadowflameCampfirePrims(Color.Violet * 0.2f, anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(flameDist, (chainLength - 7) + -8).RotatedBy(axisRotation), anchorPos16 + new Vector2(flameDist, (chainLength - 7) + -16).RotatedBy(axisRotation), 10, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trails[7] = new ShadowflameCampfirePrims(Color.Violet, anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(flameDist, (chainLength - 7) + -6).RotatedBy(axisRotation), anchorPos16 + new Vector2(flameDist, (chainLength - 7) + -12).RotatedBy(axisRotation), 6, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trails[8] = new ShadowflameCampfirePrims(Color.Violet, anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation), anchorPos16 + new Vector2(flameDist, (chainLength - 7) + -8).RotatedBy(axisRotation), anchorPos16 + new Vector2(flameDist, (chainLength - 7) + -16).RotatedBy(axisRotation), 8, 20, false, 1));
            }

            Projectile.ai[0]++;

            Projectile.Center = anchorPos16 + new Vector2(0, 8) + (Vector2.UnitY.RotatedBy(axisRotation) * chainLength);
            Projectile.rotation = (anchorPos16 - Projectile.Center).ToRotation() + 1.57f;

            foreach(ShadowflameCampfirePrims trail in trails)
            {
                trail.behindTiles = false;
                trail.rot = axisRotation;
            }

            if (!hideFlames)
            {
                if (flameHeight < 20) flameHeight++;

                trails[0].startPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[0].controlPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[0].endPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -flameHeight).RotatedBy(axisRotation / 1.5f);

                trails[1].startPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[1].controlPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[1].endPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.75f)).RotatedBy(axisRotation / 1.5f);

                trails[2].startPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[2].controlPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[2].endPoint = anchorPos16 + new Vector2(-flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -flameHeight).RotatedBy(axisRotation / 1.5f);

                trails[3].startPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[3].controlPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.75f)).RotatedBy(axisRotation / 1.5f);
                trails[3].endPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 1.25f)).RotatedBy(axisRotation / 1.5f);

                trails[4].startPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[4].controlPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[4].endPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight)).RotatedBy(axisRotation / 1.5f);

                trails[5].startPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[5].controlPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.75f)).RotatedBy(axisRotation / 1.5f);
                trails[5].endPoint = anchorPos16 + new Vector2(0, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 1.25f)).RotatedBy(axisRotation / 1.5f);

                trails[6].startPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[6].controlPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[6].endPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -flameHeight).RotatedBy(axisRotation / 1.5f);

                trails[7].startPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[7].controlPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[7].endPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.75f)).RotatedBy(axisRotation / 1.5f);

                trails[8].startPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation);
                trails[8].controlPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -(flameHeight * 0.5f)).RotatedBy(axisRotation / 1.5f);
                trails[8].endPoint = anchorPos16 + new Vector2(flameDist, (chainLength - 7) + 22).RotatedBy(axisRotation) + new Vector2(0, -flameHeight).RotatedBy(axisRotation / 1.5f);
            }
            else
            {
                foreach(ShadowflameCampfirePrims trail in trails)
                {
                    trail.startPoint = new Vector2(-100, -100);
                    trail.controlPoint = new Vector2(-100, -100);
                    trail.endPoint = new Vector2(-100, -100);
                }

                flameHeight = 0;
            }

            Lighting.AddLight(Projectile.Center, Color.Violet.ToVector3());
        }

        public int flameHeight;
        public int flameDist = 48;

        public bool hideFlames;

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 localAnchorPos16 = (anchorPos * 16) + new Vector2(8, 16);

            int distance = (int)Vector2.Distance(localAnchorPos16, Projectile.Center);

            for(float i = 5; i < distance; i += 10) 
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/GoblinFort/GoblinChandelierChain").Value,
                    Vector2.Lerp(localAnchorPos16, Projectile.Center, i / (float)(distance)) - Main.screenPosition, null, Lighting.GetColor(anchorPos.ToPoint()), axisRotation, new Vector2(4, 5), 1f, SpriteEffects.None, 0f);
            }

            lightColor = Lighting.GetColor(anchorPos.ToPoint());

            return true;
        }
    }
}