using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class SOS : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Of The Shores");
        }

        public override void SetDefaults()
        {
            Item.width = 16;   //The size in width of the sprite in pixels.
            Item.height = 18;    //The size in height of the sprite in pixels.
            Item.value = Item.buyPrice(0, 0, 20, 0); //  How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 2 gold)
            Item.rare = ItemRarityID.Blue;          //The color the title of your Weapon when hovering over it ingame
            Item.accessory = true;  //this make the item an accessory, so you can equip it
            Item.defense = 1;   //this sets the item defense given when equipped
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.dash = 3;
        }
    }
}