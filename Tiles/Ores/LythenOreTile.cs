using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.EEWorld;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Tiles.Ores
{
    public class LythenOreTile : ModTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 410; // Metal Detector value
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lythen Ore");
            AddMapEntry(new Color(152, 171, 198), name);

            dustType = 84;
            drop = ModContent.ItemType<LythenOre>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 30;
        }

        public override bool CanExplode(int i, int j)
        {
            return EEWorld.EEWorld.downedHydros;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.005f));
            Color chosen = Color.Lerp(Color.DarkCyan, Color.White, Main.rand.NextFloat(1f));
            EEMod.Particles.Get("Main").SpawnParticles(new Vector2(i, j) * 16, new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)) * 2, 2, chosen, new SlowDown(0.99f), new ZigzagMotion(10, 1.5f), new AfterImageTrail(0.75f), new SetMask(EEMod.instance.GetTexture("Masks/RadialGradient")), new SetLighting(chosen.ToVector3(), 0.2f));
        }
    }
}