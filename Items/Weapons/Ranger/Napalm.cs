using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Ranger;
using Terraria.ID;

namespace EEMod.Items.Weapons.Ranger
{
	public class Napalm : ModItem
	{
		public override string Texture => "PLACEHOLDER";


        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Napalm");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 4f;
			item.shoot = ModContent.ProjectileType<NapalmProjectile>();
			item.maxStack = 30;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.Green;
			item.useTime = 40;
			item.useAnimation = 40;
			item.consumable = true;
			item.width = 12;
			item.height = 12;
			item.value = Item.sellPrice(0, 0, 75);
		}
	}
}