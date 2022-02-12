using EEMod.Seamap.Content;
using EEMod.Seamap.Content.Cannonballs;
using EEMod.Seamap.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Shipyard.Cannonballs
{
    public class MeteorCannonball : EEItem
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
        }
    }

    public class MeteorCannonballInfo : ShipyardInfo
    {
        public override SeamapObject GetCannonball()
        {
            return new ShenCannonball(Vector2.Zero, Vector2.Zero, Color.Goldenrod);
        }
    }
}