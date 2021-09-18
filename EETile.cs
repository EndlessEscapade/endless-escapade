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

namespace EEMod
{
    public abstract class EETile : ModTile
    {
        public virtual void SetDefaults()
        {
            SoundStyle = soundType;
            SoundStyle = soundStyle;

            DustType = dustType;
            ItemDrop = drop;

            MinPick = minPick;
            MineResist = mineResist;

            SetStaticDefaults();
        }

        public int soundType;
        public int soundStyle;
        public int dustType;
        public int drop;
        public int minPick;
        public float mineResist;
        public bool disableSmartCursor;

        public override bool HasSmartInteract()
        {
            return disableSmartCursor;
        }

        public virtual void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            //SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
        }

        public virtual void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            //DrawEffects(i, j, spriteBatch, ref TileDrawInfo);
        }
    }
}
