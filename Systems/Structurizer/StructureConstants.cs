namespace EEMod.Systems.Structurizer
{
    public static class StructureConstants
    {
        public const ushort RepeatedAirFlag = 0xFFFF;
        public const ushort AirTileFlag = 0xFFFE;
        public const ushort RepeatedTileFlag = 0xFFFD;
        public const ushort PlaceMultitileFlag = 0xFFFC;
        public const ushort PlaceWaterFlag = 0xFFFB;
        public const ushort PlaceLavaFlag = 0xFFFA;
        public const ushort PlaceHoneyFlag = 0xFFF9;
        public const ushort RepeatedWaterFlag = 0xFFF8;
        public const ushort RepeatedLavaFlag = 0xFFF7;
        public const ushort RepeatedHoneyFlag = 0xFFF6;
        public const ushort PlaceMultitileWithStyleFlag = 0xFFF5;
        public const ushort PlaceMultitileWithAlternateStyleFlag = 0xFFF4;
        public const ushort RepeatedWallFlag = 0xFFF3;
        public const ushort EmptyWallFlag = 0xFFF2;
        public const ushort RepeatedEmptyWallFlag = 0xFFF1;
        public const ushort PlaceTileWithSlopeFlag = 0xFFF0;
        public const ushort RepeatedTileWithSlopeFlag = 0xFFEF;
        public const ushort PlaceHalfBrickFlag = 0xFFEE;
        public const ushort RepeatedHalfBrickFlag = 0xFFED;
        public const ushort EndOfTilesDataFlag = 0xFFEC;

        public const byte StructureFileFormatVersion = 0;
    }
}