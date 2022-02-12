using EEMod.Extensions;
using EEMod.Seamap.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod;
using static EEMod.EEMod;
using System.Diagnostics;
using EEMod.Net;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using EEMod.Prim;
using EEMod.ID;
using EEMod.Seamap.Content.Islands;

namespace EEMod.Seamap.Core.Components
{
    public class Component
    {
        public Entity myEntity;

        public Component(Entity myEntity)
        {

        }

        public virtual void Update()
        {

        }

        public virtual bool PreDraw(SpriteBatch spriteBatch)
        {
            return true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}