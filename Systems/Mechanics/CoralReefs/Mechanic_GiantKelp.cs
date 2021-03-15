using EEMod.Extensions;
using EEMod.ID;
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
    public class GiantKelp : Mechanic
    {
        protected override Layer DrawLayering => Layer.BehindTiles;
        public void DrawGiantKelp()
        {
            if (EESubWorlds.GiantKelpRoots.Count > 0)
            {
                for (int i = 1; i < EESubWorlds.GiantKelpRoots.Count - 2; i++)
                {
                    Vector2 ChainConneccPos = EESubWorlds.GiantKelpRoots[i] * 16;
                    Vector2 LastChainConneccPos = EESubWorlds.GiantKelpRoots[i - 1] * 16;

                    Tile CurrentTile = Framing.GetTileSafely((int)EESubWorlds.GiantKelpRoots[i].X, (int)EESubWorlds.GiantKelpRoots[i].Y);
                    Tile LastTile = Framing.GetTileSafely((int)EESubWorlds.GiantKelpRoots[i - 1].X, (int)EESubWorlds.GiantKelpRoots[i - 1].Y);

                    bool isValid = CurrentTile.active() && LastTile.active() && Main.tileSolid[CurrentTile.type] && Main.tileSolid[LastTile.type];

                    Vector2 Mid = (ChainConneccPos + LastChainConneccPos) / 2;
                    Vector2 lerp1 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.1f);
                    Vector2 lerp2 = Vector2.Lerp(ChainConneccPos, LastChainConneccPos, 0.9f);

                    float rot = (ChainConneccPos - LastChainConneccPos).ToRotation();

                    if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Mid) < 2000 * 2000 && isValid &&
                        !Framing.GetTileSafely((int)Mid.X / 16, (int)Mid.Y / 16).active()
                        && !Framing.GetTileSafely((int)lerp1.X / 16, (int)lerp1.Y / 16).active()
                        && !Framing.GetTileSafely((int)lerp2.X / 16, (int)lerp2.Y / 16).active()
                        && Collision.CanHit(lerp1, 1, 1, lerp2, 1, 1))
                    {
                        Texture2D GiantKelp = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/GiantKelpColumn");

                        Helpers.DrawChain(GiantKelp, 58, ((int)(Main.GameUpdateCount / 8)) % 9, ChainConneccPos, LastChainConneccPos, 0, 1, Lighting.GetColor((int)EESubWorlds.GiantKelpRoots[i].X, (int)EESubWorlds.GiantKelpRoots[i].Y));
                    }
                }
            }
        }

        public override void OnDraw(SpriteBatch spriteBatch)
        {
            if(Main.worldName == KeyID.CoralReefs) DrawGiantKelp();
        }
    }
}