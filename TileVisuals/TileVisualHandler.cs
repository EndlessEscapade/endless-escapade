using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public class TileVisualHandler
    {
        List<TileObjVisual> TileVisuals = new List<TileObjVisual>();
        public void Update()
        {
            foreach (TileObjVisual TV in TileVisuals)
            {
                TV.Update();
            }
        }
        public void Draw()
        {
            foreach (TileObjVisual TV in TileVisuals)
            {
                TV.Draw(Main.spriteBatch);
            }
        }

        public void AddElement(TileObjVisual TV)
        {
            foreach (TileObjVisual TOV in TileVisuals)
            {
                if (TOV.position == TV.position)
                    return;
            }
            TileVisuals.Add(TV);
        }
    }
}