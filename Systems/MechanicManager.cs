using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public static MechanicManager Instance => GetManager<MechanicManager>();

        internal static IList<Mechanic> Instances;

        public override void Initialize()
        {
            Instances = new List<Mechanic>();
        }

        public override void CreateInstance(Type type)
        {
            if (type.CouldBeInstantiated() && type.TryCreateInstance(out Mechanic mechanic))
            {
                ContentInstance.Register(mechanic);
                mechanic.Load();
                Instances.Add(mechanic);
            }
        }

        public override void Unload()
        {
            Instances?.Clear();
            Instances = null;
        }

        public static void PostUpdateWorld()
        {
            if (Instances != null)
                foreach (Mechanic instance in Instances)
                    instance.PostUpdateWorld();
        }

        public static void PreUpdateEntities()
        {
            if (Instances != null)
                foreach (Mechanic instance in Instances)
                    instance.PreUpdateEntities();
        }

        public static void PostDrawTiles()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.PostDrawTiles();
        }

        public static void MidUpdateProjectileItem()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.MidUpdateProjectileItem();
        }

        public static void MidUpdateNPCGore()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.MidUpdateNPCGore();
        }

        public static void MidUpdateDustTime()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.MidUpdateDustTime();
        }

        public static void PreDrawNPCs()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.PreDrawNPCs();
        }

        public static void PostDrawNPCs()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.PostDrawNPCs();
        }

        public static void PreDrawProjectiles()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.PreDrawProjectiles();
        }

        public static void PostDrawProjectiles()
        {
            if (Instances != null)
                foreach (Mechanic mechanic in Instances)
                    mechanic.PostDrawProjectiles();
        }
    }

    class MechanicWorld : ModWorld
    {
        public override void PostUpdate()
        {
            MechanicManager.PostUpdateWorld();
        }
        public override void PostDrawTiles()
        {
            MechanicManager.PostDrawTiles();
        }
    }
}