using EEMod.Extensions;
using EEMod.ID;
using EEMod.Systems;
using EEMod.Systems.Subworlds.EESubworlds;
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
    public class GiantKelp : Mechanic
    {
        protected override Layer DrawLayering => Layer.BehindTiles;
        public void DrawGiantKelp()
        {
            if (CoralReefs.GiantKelpRoots.Count > 0)
            {
                for (int i = 1; i < CoralReefs.GiantKelpRoots.Count - 2; i++)
                {
                    Vector2 ChainConneccPos = CoralReefs.GiantKelpRoots[i] * 16;
                    Vector2 LastChainConneccPos = CoralReefs.GiantKelpRoots[i - 1] * 16;
                    Vector2 Mid = (ChainConneccPos + LastChainConneccPos) / 2;
                    if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Mid) < 2000 * 2000)
                    {
                        Tile CurrentTile = Framing.GetTileSafely((int)CoralReefs.GiantKelpRoots[i].X, (int)CoralReefs.GiantKelpRoots[i].Y);
                        Tile LastTile = Framing.GetTileSafely((int)CoralReefs.GiantKelpRoots[i - 1].X, (int)CoralReefs.GiantKelpRoots[i - 1].Y);
                        Vector2 lerp1 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.2f);
                        Vector2 lerp2 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.8f);
                        bool isValid = CurrentTile.active() && LastTile.active() && Main.tileSolid[CurrentTile.type] && Main.tileSolid[LastTile.type] &&
                            !Framing.GetTileSafely((int)Mid.X / 16, (int)Mid.Y / 16).active()
                            && !Framing.GetTileSafely((int)lerp1.X / 16, (int)lerp1.Y / 16).active()
                            && !Framing.GetTileSafely((int)lerp2.X / 16, (int)lerp2.Y / 16).active()
                            && Collision.CanHit(lerp1, 1, 1, lerp2, 1, 1);
                        if (isValid)
                        {
                            Texture2D GiantKelp = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/GiantKelpColumn");
                            Point MP = Mid.ToTileCoordinates();
                            Helpers.DrawChain(GiantKelp, 58, ((int)(Main.GameUpdateCount / 8)) % 9, ChainConneccPos, LastChainConneccPos, 0, 1, Lighting.GetColor((int)ChainConneccPos.X, (int)ChainConneccPos.Y));
                        }
                    }
                }
            }
        }

        public override void OnDraw(SpriteBatch spriteBatch)
        {
            if (Main.worldName == KeyID.CoralReefs) DrawGiantKelp();
        }
    }
}