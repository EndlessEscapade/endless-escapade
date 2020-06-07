using Terraria;
using Terraria.ModLoader;
using InteritosMod.Items.Weapons.Mage;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using InteritosMod.NPCs.CoralReefs;


namespace InteritosMod.NPCs
{
    public class InteritosGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<InteritosPlayer>().ZoneCoralReefs)
            {
                pool.Add(ModContent.NPCType<Clam>(), 0.5f);
                pool.Add(ModContent.NPCType<Ball>(), 0.5f);
                pool.Add(ModContent.NPCType<GiantSquid>(), 0.5f);
                pool.Add(ModContent.NPCType<LunaJelly>(), 0.5f);
                pool.Add(ModContent.NPCType<SeaSlug>(), 0.5f);
                pool.Add(ModContent.NPCType<ManoWar>(), 0.5f);
            }
        }
    }
}
