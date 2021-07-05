using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.KelpForest
{

    public class GroundGlowCoral3 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coral Lamp");
            AddMapEntry(new Color(95, 143, 65), name);
            dustType = DustID.Plantera_Green;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile.frameY == 18 || tile.frameY == 36)
            {
                r = 0.25f;
                g = 0.2f;
                b = 0f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color chosen = Color.Lerp(Color.Gold, Color.LightYellow, Main.rand.NextFloat(1f));
            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.003f));
            EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)), mod.GetTexture("Particles/SmallCircle"), 30, 1, chosen, new SlowDown(0.98f), new RotateTexture(0.02f), new SetMask(ModContent.GetInstance<EEMod>().GetTexture("Textures/RadialGradient"), 0.6f), new AfterImageTrail(0.96f), new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)), new SetLighting(chosen.ToVector3(), 0.3f));

            Tile tile = Framing.GetTileSafely(i, j);
            Helpers.DrawTileGlowmask(mod.GetTexture("Tiles/Foliage/KelpForest/GroundGlowCoralGlow3"), i, j, Color.White * (float)(0.5f + (Math.Sin((i - (tile.frameX / 18f)) + (j - (tile.frameY / 18f)) + Main.GameUpdateCount / 60f) / 2f)));
        }
    }
}