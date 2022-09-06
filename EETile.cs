using Terraria.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ObjectInteractions;

namespace EEMod
{
    public abstract class EETile : ModTile
    {
        public bool DisableSmartCursor;

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            SetDrawPositions(i, j, ref width, ref offsetY, ref height);
        }
        public virtual void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            //SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
        }

        [Obsolete("Old method, use DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) instead")]
        public virtual void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            //DrawEffects(i, j, spriteBatch, ref TileDrawInfo);
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {

        }
    }
}
