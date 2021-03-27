using EEMod.ID;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Summon;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.CoralReefs.MechanicalReefs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public partial class EEGlobalNPC : GlobalNPC
    {
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