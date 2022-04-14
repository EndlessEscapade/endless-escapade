using EEMod.Seamap.Content.Cannonballs;
using EEMod.Seamap.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Shipyard.Cannonballs
{
    public class LeadCannonball : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lead Cannonball");
            Tooltip.SetDefault("Use this to make your boat shoot cannonballs.");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.GetGlobalItem<ShipyardGlobalItem>().Tag = ItemTags.Cannonball;
            Item.GetGlobalItem<ShipyardGlobalItem>().info = new LeadCannonballInfo();
        }
    }

    public class LeadCannonballInfo : ShipyardInfo
    {
        public override SeamapObject GetCannonball(int team)
        {
            return new FriendlyCannonball(Vector2.Zero, Vector2.Zero, (TeamID)team);
        }
    }
}