using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.NPCs.CoralReefs;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Summon;
using EEMod.Items.Materials;

namespace EEMod.NPCs
{
    public class EEGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
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
            if (Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2 && !Main.dayTime)
            {
                pool.Add(ModContent.NPCType<CoconutSpider>(), 0.5f);
            }
        }

        public override void NPCLoot(NPC npc)
        {
            /*if (Main.LocalPlayer.GetModPlayer<EEPlayer>().Cheese1 == false)
            {
                if (npc.type == NPCID.KingSlime)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Cheese>(), 1);
                }
            }*/
            if (npc.type == NPCID.MoonLordCore && !NPC.downedMoonlord)
            {
                EEWorld.EEWorld.GenerateLuminite();
            }
            if ((npc.type == ModContent.NPCType<Seahorse>() || npc.type == ModContent.NPCType<SmallClam>()) && Main.rand.Next(200) == 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem(npc.getRect(), ModContent.ItemType<AbyssalScimitar>(), 1);
                        break;
                    case 1:
                        Item.NewItem(npc.getRect(), ModContent.ItemType<AbyssalPistol>(), 1);
                        break;
                    case 2:
                        Item.NewItem(npc.getRect(), ModContent.ItemType<AbyssalSceptre>(), 1);
                        break;
                }
            }
            if (npc.type == ModContent.NPCType<Seahorse>() && EEWorld.EEWorld.downedHydros && Main.rand.Next(3) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<HydrosScales>(), Main.rand.Next(1, 4));
            }
            if (npc.type == ModContent.NPCType<GiantSquid>() && Main.rand.Next(50) == 0)
            {
                Item.NewItem(npc.getRect(), ItemID.BlackInk, 1);
            }
            if (npc.type == ModContent.NPCType<SmallClam>() && Main.rand.Next(50) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<ClamStaff>(), 1);
            }
        }
    }
}
