using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.Tiles.Furniture.OrbHolder;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Enemy;

namespace EEMod.Tiles.Foliage.ThermalVents
{
    public class Amogus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SUS");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 200;
            npc.width = 30;
            npc.height = 36;
            npc.noGravity = false;
            npc.lavaImmune = true;
            npc.noTileCollide = false;
            npc.damage = 0;
            npc.knockBackResist = 0f;
        }
    }
}