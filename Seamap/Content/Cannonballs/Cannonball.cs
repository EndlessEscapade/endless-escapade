using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.ID;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ID;
using EEMod.Seamap.Content;
using System.Diagnostics;
using EEMod.Extensions;
using ReLogic.Content;
using EEMod.Seamap.Core;

namespace EEMod.Seamap.Content.Cannonballs
{
    public class Cannonball : SeamapObject
    {
        public TeamID team;

        public Cannonball(Vector2 pos, Vector2 vel, TeamID team) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            this.team = team;
        }
    }

    public enum TeamID
    {
        Enemy = -1,
        Neutral = 0,
        TeamRed = 1,
        TeamGreen = 2,
        TeamBlue = 3,
        TeamYellow = 4,
        TeamPink = 5
    }
}
