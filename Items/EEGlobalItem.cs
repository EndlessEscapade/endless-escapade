using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        //public override bool InstancePerEntity => true; // unneeded atm

        //public override bool CloneNewInstances => true;

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            EEPlayer eeplayer = player.GetModPlayer<EEPlayer>();
            if (eeplayer.dalantiniumHood)
            {
                reduce -= 0.05f;
            }
            if (eeplayer.hydriteVisage)
            {
                reduce -= 0.1f;
            }
        }

        /*public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(item.type == ItemID.Boomstick || item.type == ItemID.StarCannon || item.type == ItemID.SnowballCannon || item.type == ItemID.Shotgun || item.type == ItemID.TacticalShotgun || item.type == ItemID.OnyxBlaster || item.type == ItemID.RocketLauncher || item.type == ItemID.JackOLanternLauncher || item.type == ItemID.SnowmanCannon || item.type == ItemID.FireworksLauncher)
            {
                player.velocity += -Vector2.Normalize(Main.MouseWorld - player.Center) * 8;
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }*/
    }
}