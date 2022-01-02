using EEMod.Extensions;
using EEMod.Systems;
using EEMod.Tiles.Furniture;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class OrbNoiseSurfacing : ModSystem
    {
        private float speed;
        private float seed;
        public void DrawNoiseSurfacing()
        {
            Vector2 mouseTilePos = Main.MouseWorld / 16;
            if (WorldGen.InWorld((int)mouseTilePos.X, (int)mouseTilePos.Y, 10))
            {
                Tile tile = Framing.GetTileSafely((int)mouseTilePos.X, (int)mouseTilePos.Y);

                Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos = Vector2.Zero;
                if (tile != null)
                {
                    if (tile.IsActive && tile.type == ModContent.TileType<OrbHolder>())
                    {
                        speed += 0.002f;

                        if (speed % 0.5f < 0.002f)
                        {
                            seed = Main.rand.NextFloat(0, 1);
                        }

                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                        EEMod.NoiseSurfacing.Parameters["yCoord"].SetValue(seed);
                        EEMod.NoiseSurfacing.Parameters["t"].SetValue((0.25f - Math.Abs(0.25f - (speed % 0.5f))) * 4);
                        EEMod.NoiseSurfacing.Parameters["xDis"].SetValue(speed % 0.5f);
                        EEMod.NoiseSurfacing.Parameters["noiseTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("noise").Value);
                        EEMod.NoiseSurfacing.CurrentTechnique.Passes[0].Apply();

                        Vector2 position = new Vector2((int)mouseTilePos.X * 16, (int)mouseTilePos.Y * 16) - new Vector2(tile.frameX / 18 * 16, tile.frameY / 18 * 16);

                        Main.spriteBatch.Draw(EEMod.Instance.Assets.Request<Texture2D>("NoiseSurfacingTest").Value, position.ForDraw() + new Vector2(15, -20), Color.Purple);
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos = position;
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                }
            }
        }

        public override void PostDrawTiles()
        {
            DrawNoiseSurfacing();
        }
    }
}