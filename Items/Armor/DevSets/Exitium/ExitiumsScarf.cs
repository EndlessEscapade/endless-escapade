using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Neck)]

    public class ExitiumsScarf : ModItem
    {
public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Scarf");
            Tooltip.SetDefault("'Appear weak when you are strong, and strong when you are weak.'\n'Great for impersonating mod devs!'");

        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 10;
            item.rare = ItemRarityID.Cyan;
            item.vanity = true;
            item.accessory = true;
        }
    }
}