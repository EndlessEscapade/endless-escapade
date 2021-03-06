using Microsoft.Xna.Framework;

namespace EEMod.EEWorld
{
    public abstract class CoralReefMinibiome
    {
        internal Point Size { get; set; }
        internal Point Position { get; set; }
        internal Point Center => new Point(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
        protected Rectangle Bounds => new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

        public virtual void StructureStep()
        {
        }

        public virtual void FoliageStep()
        {
        }

        internal bool EnsureNoise;
    }
}