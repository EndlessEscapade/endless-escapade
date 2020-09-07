namespace EEMod.Net
{
    public enum EEMessageType : byte
    {
        None = 0,
        MouseCheck,
        SyncVector,
        SyncPosition,
        SyncCool,
        SyncBoatPos,
        SyncBrightness
    }
}