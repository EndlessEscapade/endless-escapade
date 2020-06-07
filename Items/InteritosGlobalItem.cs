using Terraria;
using Terraria.ModLoader;
using InteritosMod.Items.Armor.Dalantinium;

namespace InteritosMod.Items
{
	public class InteritosGlobalItem : GlobalItem
	{
		public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
		{
			if (player.GetModPlayer<InteritosPlayer>().dalantiniumHood)
			{
				reduce -= 0.05f;
			}
            if (player.GetModPlayer<InteritosPlayer>().hydriteVisage)
            {
                reduce -= 0.1f;
            }
        }

        private bool randomAssVanillaAdaptedFlag = false;
        private int debug = 0;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults(Item item)
        {

        }

        public override void UpdateEquip(Item item, Player player)
        {

        }


        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.GetModPlayer<ProphecyPlayer>().isQuartzMeleeOn && item.melee && item.useStyle == 1)
            {
                if (Main.rand.Next(3) == 1)
                {
                    float speed = Main.rand.NextFloat(9, 11);
                    float distX = Main.mouseX + Main.screenPosition.X - player.Center.X;
                    float distY = Main.mouseY + Main.screenPosition.Y - player.Center.Y;
                    float mag = (float)Math.Sqrt(distX * distX + distY * distY);
                    Projectile.NewProjectile(player.position.X, player.position.Y, distX * speed / mag, distY * speed / mag, mod.ProjectileType("QuartzProjSwords"), (int)(item.damage * 0.7f), item.knockBack, player.whoAmI, 0f, 0f);
                    return true;
                }
                else
                    return true;
            }
            return true;
        }
        public override bool UseItem(Item item, Player player)
        {
            if (player.GetModPlayer<ProphecyPlayer>().isQuartzMeleeOn && item.melee && item.useStyle == 1)
            {
                if (Main.rand.Next(68) == 1)
                {
                    float speed = Main.rand.NextFloat(9, 11);
                    float distX = Main.mouseX + Main.screenPosition.X - player.Center.X;
                    float distY = Main.mouseY + Main.screenPosition.Y - player.Center.Y;
                    float mag = (float)Math.Sqrt(distX * distX + distY * distY);
                    Projectile.NewProjectile(player.position.X, player.position.Y, distX * speed / mag, distY * speed / mag, mod.ProjectileType("QuartzProjSwords"), (int)(item.damage * 0.7f), item.knockBack, player.whoAmI, 0f, 0f);
                    return false;
                }
            }
            return false;
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> list)
        {

        }
    }
}