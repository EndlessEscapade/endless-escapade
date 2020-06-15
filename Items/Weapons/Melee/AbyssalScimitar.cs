using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class AbyssalScimitar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Scimitar");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3.5f; // 5 and 1/4
            item.useTime = 22;
            item.useAnimation = 22;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.damage = 13;
            item.width = 32;
            item.height = 32;
            item.UseSound = SoundID.Item1;
        }
    }
}