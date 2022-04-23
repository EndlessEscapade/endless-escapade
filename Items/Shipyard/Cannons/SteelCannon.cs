using EEMod.NPCs.Goblins.Shaman;
using EEMod.Prim;
using EEMod.Seamap.Content;
using EEMod.Seamap.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Shipyard.Cannons
{
    public class SteelCannon : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Steel Cannon");
            Tooltip.SetDefault("haha this shoots balls lol");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
            Item.GetGlobalItem<ShipyardGlobalItem>().Tag = ItemTags.Cannon;
            Item.GetGlobalItem<ShipyardGlobalItem>().info = new SteelCannonInfo();
        }
    }

    public class SteelCannonInfo : ShipyardInfo
    {
        public override void LeftClickAbility(SeamapPlayerShip boat, SeamapObject cannonball)
        {
            cannonball.Center = boat.Center + new Vector2(0, 41);
            cannonball.velocity = boat.velocity - (Vector2.UnitX.RotatedBy(boat.CannonRestrictRange()) * 4);

            boat.velocity -= Vector2.Normalize(Main.MouseWorld - boat.Center) * 0.5f;

            SeamapObjects.NewSeamapObject(cannonball);

            SoundEngine.PlaySound(SoundID.Item38);
        }
    }
}