using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public class MLObject
    {
        public void Initialize()
        {
            OnInitialize();
        }
        public void Update()
        {
            OnUpdate();
        }
        public virtual void OnInitialize() {; }

        public virtual void OnUpdate() {; }
    }
}