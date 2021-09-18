using EEMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles.Ores
{
    public class AquamarineTile : EETile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            //Main.tileValue[Type] = 410; // Metal Detector value
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 1100; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Aquamarine");
            AddMapEntry(new Color(152, 171, 198), name);

            dustType = DustID.Platinum;
            drop = ModContent.ItemType<Aquamarine>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 100;
        }

        public override bool CanExplode(int i, int j)
        {
            return NPC.downedMechBossAny;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.0025f));
            //EEMod.MainParticles.SpawnParticles(new Vector2(Main.rand.Next(i * 16, (i + 1) * 16), Main.rand.Next(j * 16, (j + 1) * 16)), new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), ModContent.GetTexture("EEMod/Particles/Cross"), 90, 1f, Color.White, new SlowDown(0.98f), new RotateTexture(0.01f), new SetMask(Helpers.RadialMask, 0.4f));
        }
    }
}