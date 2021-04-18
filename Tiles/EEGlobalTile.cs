using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using EEMod.ID;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Tiles
{
    public class EEGlobalTile : GlobalTile
    {
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if(Framing.GetTileSafely(i, j).type == TileID.LivingMahoganyLeaves)
            {
                EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.001f));
                EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), 0.5f), mod.GetTexture("Particles/MahoganyLeaf"), 60, Main.rand.NextFloat(0.8f, 1.1f), Lighting.GetColor(i, j), new RotateTexture(0.02f), new SlowDown(0.99f));
            }
            if (Framing.GetTileSafely(i, j).type == TileID.LeafBlock)
            {
                EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.001f));
                EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), 0.5f), mod.GetTexture("Particles/LivingLeaf"), 60, Main.rand.NextFloat(0.8f, 1.1f), Lighting.GetColor(i, j), new RotateTexture(0.02f), new SlowDown(0.99f));
            }
        }
    }
}