using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.EEWorld;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Tiles.Ores
{
    public class LythenOreTile : EETile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            //Main.tileValue[Type] = 410; // Metal Detector value
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lythen Ore");
            AddMapEntry(new Color(152, 171, 198), name);

            DustType = DustID.Platinum;
            ItemDrop = ModContent.ItemType<LythenOre>();
            SoundType = SoundID.Tink;
            SoundStyle = 1;
            MineResist = 1f;
            MinPick = 30;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.005f));
            Color chosen = Color.Lerp(Color.DarkCyan, Color.White, Main.rand.NextFloat(1f));
            EEMod.MainParticles.SpawnParticles(new Vector2(i, j) * 16, new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)) * 2, 2, chosen, new SlowDown(0.99f), new ZigzagMotion(10, 1.5f), new AfterImageTrail(0.75f), new SetMask(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, Color.White), new SetLighting(chosen.ToVector3(), 0.2f));
        }
    }
}