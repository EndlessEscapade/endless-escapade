using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class AccursedAmulet : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Amulet");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;

            Item.width = 16;
            Item.height = 16;

            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 64 tiles.
            const float distanceRadius = 1024f * 1024f;

            const int minNPCs = 4;
            const int maxNPCs = 15;

            int npcCount = 0;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.DistanceSQ(player.Center) > distanceRadius)
                    continue;
                
                npcCount++;
            }

            if (npcCount < minNPCs)
                return;

            if (npcCount > maxNPCs)
                npcCount = maxNPCs;

            // 1.5% life loss for each NPC.
            player.statLifeMax2 -= (int)(player.statLifeMax * 0.015f) * npcCount;
        
            // 0.5% damage increase for each NPC.
            player.GetDamage(DamageClass.Generic) += 0.05f * npcCount;
        }
    }
}