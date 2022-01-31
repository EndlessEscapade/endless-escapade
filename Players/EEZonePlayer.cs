using EEMod.ID;
using EEMod.Systems.Subworlds.EESubworlds;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Players
{
    internal class EEZonePlayer : ModPlayer
    {
        //Biome checks
        public bool ZoneCoralReefs;

        public bool ZoneSurfaceReefs;
        public bool ZoneUpperReefs;
        public bool ZoneLowerReefs;
        public bool ZoneReefDepths;

        public bool ZoneTropicalIsland;

        public MinibiomeID reefMinibiomeID = MinibiomeID.None;

        public override void PostUpdate()
        {
            base.PostUpdate();

            CoralReefsPostUpdate();
        }

        private void CoralReefsPostUpdate()
        {
            ZoneCoralReefs = Main.ActiveWorldFileData.Name == KeyID.CoralReefs;
            if (ZoneCoralReefs)
            {
                reefMinibiomeID = MinibiomeID.None;

                for (int k = 0; k < CoralReefs.Minibiomes.Count; k++)
                {
                    Vector2 playerPos = Player.Center / 16;
                    Vector2 minibiomePos = CoralReefs.Minibiomes[k].Center.ToVector2();

                    if (EEWorld.EEWorld.OvalCheck((int)minibiomePos.X, (int)minibiomePos.Y, (int)playerPos.X, (int)playerPos.Y, CoralReefs.Minibiomes[k].Size.X / 2, CoralReefs.Minibiomes[k].Size.Y / 2))
                    {
                        reefMinibiomeID = CoralReefs.Minibiomes[k].id;

                        break;
                    }
                }
            }
            else
            {

            }
        }
    }
}
