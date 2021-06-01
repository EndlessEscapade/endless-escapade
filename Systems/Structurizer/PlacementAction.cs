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

        public static PlacementAction PlaceSlopeRepeated(ushort count, byte slope, ushort entry) =>
            new PlacementAction(PlacementActionType.PlaceSlopeRepeated, entry, count, 0, slope, 0);
    }
}