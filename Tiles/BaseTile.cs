using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace EEMod.Tiles
{
    public class BaseTile : EETile
    {
        protected virtual int _drop => ItemID.DirtBlock;
        protected virtual string _displayName => "Tile";
        protected virtual Color _mapColor => Color.White;
        protected virtual int _dustType => 1;
        protected virtual int _soundStyle => 1;
        protected virtual float _mineResist => 1f;
        protected virtual int _minPick => 0;
        protected virtual bool _mergeDirt => false;
        protected virtual bool _tileSolid => true;
        protected virtual bool _tileBlendAll => true;

        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = _mergeDirt;
            Main.tileSolid[Type] = _tileSolid;
            Main.tileBlendAll[Type] = _tileBlendAll;

            AddMapEntry(_mapColor);

            DustType = _dustType;
            ItemDrop = _drop;
            SoundStyle = _soundStyle;
            MineResist = _mineResist;
            MinPick = _minPick;
        }
    }
}