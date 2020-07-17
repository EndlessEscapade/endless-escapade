﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Methods with this attribute will be called during <see cref="EEMod.Unload"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class UnloadingMethodAttribute : Attribute
    {
    }
}
