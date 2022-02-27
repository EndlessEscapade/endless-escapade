using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Materials;
using EEMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using EEMod.Items.Placeables.Ores;
using System;
using EEMod.Prim;
using EEMod.Projectiles.CoralReefs;
using EEMod.NPCs.Bosses.Hydros;

namespace EEMod.Tiles.Furniture.GoblinFort
{
    public class ShadowflameCampfire : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;

            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ShadowflameCampfireTE>().Hook_AfterPlacement, -1, 0, true);

            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Shadowflame Campfire");
            AddMapEntry(new Color(44, 193, 139), name);
            DustType = DustID.Clentaminator_Cyan;
            DisableSmartCursor = false;
            MinPick = 0;
            MineResist = 1f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color chosen = Color.Lerp(Color.Pink, Color.White, Main.rand.NextFloat(1f));
            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.015f));
            EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), 
                new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-2f, -0.75f)), 
                Mod.Assets.Request<Texture2D>("Particles/SmallCircle").Value, 30, 1, chosen, 
                new SlowDown(0.98f), 
                new RotateTexture(0.02f), 
                new SetMask(EEMod.Instance.Assets.Request<Texture2D>("Textures/RadialGradient").Value, Color.Magenta), 
                new AfterImageTrail(1f), new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)),
                new SetLighting(chosen.ToVector3(), 0.2f));

            Lighting.AddLight(new Vector2(i * 16, j * 16), Color.Violet.ToVector3() * 1.5f);
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            ModContent.GetInstance<ShadowflameCampfireTE>().Kill(i, j);
        }
    }

    public class ShadowflameCampfireTE : EETileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }

            Vector2 initPos = new Vector2(i * 16, (j * 16) + 16);

            PrimitiveSystem.primitives.CreateTrail(new ShadowflameCampfirePrims(Color.Violet * 0.2f, initPos + new Vector2(18, 16), initPos + new Vector2(18, -22), initPos + new Vector2(18, -58), 30, 20, true));
            PrimitiveSystem.primitives.CreateTrail(new ShadowflameCampfirePrims(Color.Violet, initPos + new Vector2(18, 16), initPos + new Vector2(18, -18), initPos + new Vector2(18, -50), 22, 20, true));
            PrimitiveSystem.primitives.CreateTrail(new ShadowflameCampfirePrims(Color.Violet, initPos + new Vector2(18, 16), initPos + new Vector2(18, -18), initPos + new Vector2(18, -50), 24, 20));

            PrimitiveSystem.primitives.CreateTrail(new ShadowflameCampfirePrims(Color.Violet * 0.2f, initPos + new Vector2(30, 16), initPos + new Vector2(30, -14), initPos + new Vector2(30, -38), 26, 20, true));
            PrimitiveSystem.primitives.CreateTrail(new ShadowflameCampfirePrims(Color.Violet, initPos + new Vector2(30, 16), initPos + new Vector2(30, -10), initPos + new Vector2(30, -30), 18, 20, true));
            PrimitiveSystem.primitives.CreateTrail(new ShadowflameCampfirePrims(Color.Violet, initPos + new Vector2(30, 16), initPos + new Vector2(30, -10), initPos + new Vector2(30, -30), 10, 20));

            return Place(i, j);
        }
    }
}