namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BaseRepeatedPlacementAction<TPlacementAction> : IRepeatedPlacementAction<TPlacementAction>
        where TPlacementAction : IPlacementAction
    {
        public abstract ushort Flag { get; }

        public virtual ushort EntryData { get; }

        public virtual byte StyleData { get; }

        public virtual byte AlternateStyleData { get; }

        protected byte InternalRepetitionCount { get; set; }

        public virtual byte RepetitionCount
        {
            get => InternalRepetitionCount;

            set => InternalRepetitionCount = value;
        }

        public abstract TPlacementAction PlacementAction { get; }

        protected BaseRepeatedPlacementAction(byte repetitionCount)
        {
            InternalRepetitionCount = repetitionCount;
        }

        public virtual void Place()
        {
        }
    }
}