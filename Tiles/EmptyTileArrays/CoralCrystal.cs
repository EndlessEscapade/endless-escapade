

using IL.Terraria;

namespace EEMod.Tiles.EmptyTileArrays
{
    public class EmptyTileArrays
    {
        public enum Slope
        {
            Solid,
            DownLeft,
            DownRight,
            UpLeft,
            UpRight,
            HalfBrick
        }
        public static byte[,,] CoralCrystal = new byte[,,]
        {
            {{0,0},{0,0},{0,0},{1,0},{1,0}},
            {{1,0},{1,0},{0,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0},{1,0},{1,0}},
            {{0,0},{1,0},{1,0},{1,0},{0,0}},
            {{0,0},{1,0},{1,0},{1,0},{0,0}},
            {{0,0},{1,0},{1,0},{1,0},{0,0}},
            {{0,0},{1,0},{1,0},{1,0},{0,0}},
        };
        public static byte[,,] LuminantCoralCrystalGround = new byte[,,]
        {
            {{0,0},{1,0},{1,0},{1,0},{0,0}},
            {{1,0},{1,0},{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0},{1,0},{1,(byte)Slope.UpRight}},
            {{1,0},{1,0},{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0},{1,0},{1,0}},
        };
        public static byte[,,] LuminantCoralCrystalSide1 = new byte[,,]
        {
            {{1,0},{1,0},{1,(byte)Slope.UpRight}}
        };
        public static byte[,,] LuminantCoralCrystalSide2 = new byte[,,]
        {
            {{1,0},{1,0}}
        };
        public static byte[,,] LuminantCoralCrystalGround2 = new byte[,,]
        {
            {{1,0},{1,0}},
            {{1,0},{1,0}}
        };
        public static byte[,,] LuminantCoralCrystalGround3 = new byte[,,]
        {
            {{1,0},{0,0}},
            {{1,0},{1,(byte)Slope.UpLeft}},
            {{1,0},{1,0}}
        };
        public static byte[,,] LuminantCoralCrystalHang1 = new byte[,,]
        {
            {{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0}},
            {{0,0},{1,0},{1,0}},
        };
        public static byte[,,] LuminantCoralCrystalHang2 = new byte[,,]
       {
            {{1,0},{1,0}},
            {{1,0},{1,0}},
            {{1,0},{0,0}},
            {{1,0},{0,0}}
       };
        public static byte[,,] LuminantCoralCrystalHang3 = new byte[,,]
       {
            {{1,0},{1,0}},
            {{1,0},{1,0}}
       };
        public static byte[,,] LuminantCoralCrystalGround4 = new byte[,,]
       {
            {{0,0},{1,0},{1,0},{1,0}},
            {{1,(byte)Slope.UpRight},{1,0},{1,0},{1,0}},
            {{1,0},{1,0},{1,0},{1,0}}
       };
        public static byte[,,] LuminantCoralCrystalSide3 = new byte[,,]
       {
            {{0,0},{1,0},{1,0},{1,0},{1,(byte)Slope.UpRight}},
            {{0,0},{1,0},{1,0},{1,0},{0,0}},
            {{0,0},{1,0},{1,0},{1,0},{1,(byte)Slope.UpLeft}}
       };
    }
}
