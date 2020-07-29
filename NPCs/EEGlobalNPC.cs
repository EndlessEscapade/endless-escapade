using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs;
using EEMod.Items.Food;

namespace EEMod.NPCs
{
    public class EEGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<EEPlayer>().ZoneCoralReefs)
            {
                pool.Remove(NPCID.Harpy);
                pool.Remove(NPCID.Piranha);
                pool.Remove(NPCID.Goldfish);
                pool.Remove(NPCID.Skeleton);
                pool.Remove(NPCID.BlueJellyfish);
            }
            if (spawnInfo.player.GetModPlayer<EEPlayer>().ZoneCoralReefs && spawnInfo.player.height <= 12800 && spawnInfo.player.height >= 1200)
            {
                pool.Add(ModContent.NPCType<Clam>(), 0.5f);
                pool.Add(ModContent.NPCType<LunaJelly>(), 0.5f);
                pool.Add(ModContent.NPCType<SeaSlug>(), 0.5f);
                pool.Add(ModContent.NPCType<Seahorse>(), 0.5f);
            }
            else if (spawnInfo.player.GetModPlayer<EEPlayer>().ZoneCoralReefs && spawnInfo.player.height > 12800)
            {
                pool.Add(ModContent.NPCType<Clam>(), 0.5f);
                pool.Add(ModContent.NPCType<Ball>(), 0.5f);
                pool.Add(ModContent.NPCType<GiantSquid>(), 0.5f);
                pool.Add(ModContent.NPCType<SeaSlug>(), 0.5f);
                pool.Add(ModContent.NPCType<ManoWar>(), 0.5f);
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2)
            {
                pool.Add(ModContent.NPCType<CoconutCrab>(), 0.5f);
                pool.Add(ModContent.NPCType<Cococritter>(), 0.5f);
            }
            if(Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2 && !Main.dayTime)
            {
                pool.Add(ModContent.NPCType<CoconutSpider>(), 0.5f);
            }
        }

        public override void NPCLoot(NPC npc)
        {
/*            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().Cheese1 == false)
            {
                if (npc.type == NPCID.KingSlime)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Cheese>(), 1);
                }
            }
            */
            if (npc.type == NPCID.MoonLordCore && !NPC.downedMoonlord)
            {
                EEWorld.EEWorld.GenerateLuminite();
            }
        }
    }
}
