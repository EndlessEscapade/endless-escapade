using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EEMod.Autoloading;
using EEMod.Autoloading.AutoloadTypes;
using EEMod.Extensions;
using EEMod.Systems;

namespace EEMod
{

    public class MechanicManager : AutoloadTypeManager<Mechanic>
    {
        public static MechanicManager Instance => AutoloadTypeManager.GetManager<MechanicManager>();

        public override void CreateInstance(Type type)
        {
            if(type.CouldBeInstantiated() && type.TryCreateInstance(out Mechanic mechanic))
            {
                //Main.NewText(mechanic); // ?
                ContentInstance.Register(mechanic);
                mechanic.Load();
            }
        }
    }
}