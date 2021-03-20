using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.Coral
{
    public class ThermalVent1x2 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Thermal Vent");
            AddMapEntry(new Color(0, 100, 200), name);
            dustType = DustID.Dirt;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.3f;
            b = 0.2f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Helpers.DrawTileGlowmask(mod.GetTexture("Tiles/Foliage/Coral/ThermalVent1x2Glow"), i, j);

            if (Framing.GetTileSafely(i, j).frameY == 0)
            {
                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.005f));
                EEMod.Particles.Get("Main").SpawnParticles(new Vector2(i * 16 + Main.rand.Next(4, 12), j * 16), new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.25f, -0.75f)), mod.GetTexture("Projectiles/WaterDragonsBubble"), 90, Main.rand.NextFloat(0.5f, 0.75f), Color.White, new SlowDown(0.99f), new RotateTexture(0.003f));

                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.1f));
                EEMod.Particles.Get("Main").SpawnParticles(new Vector2(i * 16 + Main.rand.Next(4, 12), j * 16), new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.25f, -0.75f)), mod.GetTexture("Particles/Cross"), 60, Main.rand.NextFloat(1f, 2f), Color.Lerp(Color.DimGray, Color.DarkSlateGray, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.97f), new RotateTexture(0.02f), new AfterImageTrail(0.75f), new SetMask(mod.GetTexture("Masks/RadialGradient")));
            }
        }
    }
}