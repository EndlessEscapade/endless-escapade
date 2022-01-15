using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace EEMod.Extensions
{
    public static class ModExtensions
    {
        private static Func<Mod, TmodFile> _getFile;

        //public static int GetMusicSoundSlot(this Mod mod, string name) => mod.GetSoundSlot(SoundType.Music, "Sounds/Music" + name);

        public static TmodFile GetFile(this Mod mod)
        {
            if (_getFile == null)
                _getFile = typeof(Mod).GetProperty("File", Helpers.FlagsInstance).GetGetMethod(true).CreateDelegate<Func<Mod, TmodFile>>();
            return _getFile(mod);
        }
    }
}
