namespace EEMod.Systems.Structurizer
{
    public struct PlacementAction
    {
        public PlacementActionType Type;
        public ushort EntryData;
        public ushort RepetitionData;
        public byte LiquidData;
        public byte StyleData; // also used by slopes for slope type
        public byte AlternateStyleData;

        public PlacementAction(PlacementActionType actionType, ushort entryData, ushort repetitionData, byte liquidData,
            byte styleData, byte altData)
        {
            Type = actionType;
            LiquidData = liquidData;
            EntryData = entryData;
            RepetitionData = repetitionData;
            StyleData = styleData;
            AlternateStyleData = altData;
        }

        public static PlacementAction AirTile => new PlacementAction(PlacementActionType.PlaceAir, 0, 0, 0, 0, 0);

        public static PlacementAction EmptyWall =>
            new PlacementAction(PlacementActionType.PlaceEmptyWall, 0, 0, 0, 0, 0);

        public static PlacementAction PlaceAirRepeated(ushort count) =>
            new PlacementAction(PlacementActionType.PlaceAirRepeated, 0, count, 0, 0, 0);

        public static PlacementAction PlaceTile(ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceTile, entry, 0, 0, 0, 0);

        public static PlacementAction PlaceTileRepeated(ushort entry, ushort count) =>
            new PlacementAction(PlacementActionType.PlaceTileRepeated, entry, count, 0, 0, 0);

        public static PlacementAction PlaceMultitile(ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceMultitile, entry, 0, 0, 0, 0);

        public static PlacementAction PlaceWater(byte amount) =>
            new PlacementAction(PlacementActionType.PlaceWater, 0, 0, amount, 0, 0);

        public static PlacementAction PlaceLava(byte amount) =>
            new PlacementAction(PlacementActionType.PlaceLava, 0, 0, amount, 0, 0);

        public static PlacementAction PlaceHoney(byte amount) =>
            new PlacementAction(PlacementActionType.PlaceHoney, 0, 0, amount, 0, 0);

        public static PlacementAction PlaceWaterRepeated(ushort count, byte amount) =>
            new PlacementAction(PlacementActionType.PlaceWaterRepeated, 0, count, amount, 0, 0);

        public static PlacementAction PlaceLavaRepeated(ushort count, byte amount) =>
            new PlacementAction(PlacementActionType.PlaceLavaRepeated, 0, count, amount, 0, 0);

        public static PlacementAction PlaceHoneyRepeated(ushort count, byte amount) =>
            new PlacementAction(PlacementActionType.PlaceHoneyRepeated, 0, count, amount, 0, 0);

        public static PlacementAction PlaceMultitileWithStyle(byte style, ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceMultitileWithStyle, entry, 0, 0, style, 0);

        public static PlacementAction PlaceMultitileWithAlternateStyle(byte style, byte alternate, ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceMultitileWithAlternateStyle, entry, 0, 0, style, alternate);

        public static PlacementAction PlaceWall(ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceWall, entry, 0, 0, 0, 0);

        public static PlacementAction PlaceWallRepeated(ushort entry, ushort count) =>
            new PlacementAction(PlacementActionType.PlaceWallRepeated, entry, count, 0, 0, 0);

        public static PlacementAction PlaceEmptyWallRepeated(ushort count) =>
            new PlacementAction(PlacementActionType.PlaceEmptyWallRepeated, 0, count, 0, 0, 0);

        public static PlacementAction PlaceSlope(byte slope, ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceSlope, entry, 0, 0, slope, 0);

        public static PlacementAction PlaceSlopeRepeated(ushort count, byte slope, ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceSlopeRepeated, entry, count, 0, slope, 0);

        public static PlacementAction PlaceHalfBrick(ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceHalfBrick, entry, 0, 0, 0, 0);

        public static PlacementAction PlaceHalfBrickRepeated(ushort count, ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceHalfBrickRepeated, entry, count, 0, 0, 0);
    }
}