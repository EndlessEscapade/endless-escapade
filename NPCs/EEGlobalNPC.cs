using System.Collections.Generic;
using Terraria.ModLoader;
using EEMod.NPCs.CoralReefs;
using Terraria;
using Terraria.ID;

namespace EEMod.NPCs
{
    public class EEGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<EEPlayer>().ZoneCoralReefs)
            {
                pool.Add(ModContent.NPCType<Clam>(), 0.5f);
                pool.Add(ModContent.NPCType<Ball>(), 0.5f);
                pool.Add(ModContent.NPCType<GiantSquid>(), 0.5f);
                pool.Add(ModContent.NPCType<LunaJelly>(), 0.5f);
                pool.Add(ModContent.NPCType<SeaSlug>(), 0.5f);
                pool.Add(ModContent.NPCType<ManoWar>(), 0.5f);
            }
        }
        public override void NPCLoot(NPC npc)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().Cheese1 == false)
            {
                if (npc.type == NPCID.KingSlime)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("Cheese"), 1);
                }
            }

            if(npc.type == NPCID.MoonLordCore && !NPC.downedMoonlord)
            {
                EEWorld.EEWorld.GenerateLuminite();
            }
        }
    }
}
