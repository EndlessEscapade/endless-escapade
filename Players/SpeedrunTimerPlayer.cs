using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod.Players
{
    internal class SpeedrunTimerPlayer : ModPlayer
    {
        public int hours;
        public int minutes;
        public int seconds;
        public int milliseconds;

        public override void PreUpdate()
        {
            if (Main.frameRate != 0)
            {
                milliseconds += 1000 / Main.frameRate;
            }

            if (milliseconds >= 1000)
            {
                milliseconds = 0;
                seconds++;
            }
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }
            if (minutes >= 60)
            {
                minutes = 0;
                hours++;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            /*
            tag["Hours"] = Hours;
            tag["Minutes"] = Minutes;
            tag["Seconds"] = Seconds;
            tag["Milliseconds"] = Milliseconds;
            */
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
        }
    }
}
