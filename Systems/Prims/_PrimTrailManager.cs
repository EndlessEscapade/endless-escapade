using System.Collections.Generic;

namespace EEMod.Prim
{
    public class PrimTrailManager
    {
        public List<PrimTrail> _trails = new List<PrimTrail>();
        public void DrawTrailsAboveTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if (!trail.behindTiles && !trail.ManualDraw)
                    trail.Draw();
            }

        }
        public void DrawTrailsBehindTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if(trail.behindTiles && !trail.ManualDraw)
                trail.Draw();
            }
        }
        public void UpdateTrailsBehindTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if (trail.behindTiles)
                trail.Update();
            }
        }
        public void UpdateTrailsAboveTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if (!trail.behindTiles)
                trail.Update();
            }
        }
        public void CreateTrail(PrimTrail PT) => _trails.Add(PT);

    }
}