using EEMod.ID;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Summon.Minions;

using EEMod.NPCs.ThermalVents;
using EEMod.NPCs.SurfaceReefs;
using EEMod.NPCs.UpperReefs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class EEGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            //if (npc.type == ModContent.NPCType<Seahorse>() && EEWorld.EEWorld.downedHydros && Main.rand.Next(3) == 0)
            //{
            //    Item.NewItem(npc.getRect(), ModContent.ItemType<HydrosScales>(), Main.rand.Next(1, 4));
            //}
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
