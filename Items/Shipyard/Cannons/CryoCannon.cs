using EEMod.Seamap.Content;
using EEMod.Seamap.Content.Cannonballs;
using EEMod.Seamap.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Shipyard.Cannons
{
    public class CryoCannon : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryo Cannon");
            Tooltip.SetDefault("Converts all cannonballs into frozen cannonballs\nFrozen cannonbaalls explode into shards on impact");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
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
            Item.GetGlobalItem<ShipyardGlobalItem>().info = new CryoCannonInfo();
        }
    }

    public class CryoCannonInfo : ShipyardInfo
    {
        public override void LeftClickAbility(SeamapPlayerShip boat, SeamapObject cannonball)
        {
            cannonball = new FrozenCannonball(Vector2.Zero, Vector2.Zero, (TeamID)boat.myPlayer.team);

            cannonball.Center = boat.Center;
            cannonball.velocity = boat.velocity - (Vector2.UnitX.RotatedBy(boat.CannonRestrictRange()) * 4);

            boat.velocity -= Vector2.Normalize(Main.MouseWorld - boat.Center) * 0.5f;

            SeamapObjects.NewSeamapObject(cannonball);

            SoundEngine.PlaySound(SoundID.Item38);
        }
    }
}