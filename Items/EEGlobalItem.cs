using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        //private bool randomAssVanillaAdaptedFlag = false;
        //private int debug = 0;
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

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

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.GetModPlayer<EEPlayer>().isQuartzMeleeOn && item.melee && item.useStyle == 1 && Main.rand.NextBool(3))
            {
                float speed = Main.rand.NextFloat(9, 11);
                Projectile.NewProjectile(player.position, Vector2.Normalize(Main.MouseWorld - player.Center) * speed, ModContent.ProjectileType<QuartzProjSwords>(), (int)(item.damage * 0.7f), item.knockBack, player.whoAmI, 0f, 0f);
            }
            return true;
        }

        public override bool UseItem(Item item, Player player)
        {
            if (player.GetModPlayer<EEPlayer>().isQuartzMeleeOn && item.melee && item.useStyle == 1 && Main.rand.NextBool(68))
            {
                float speed = Main.rand.NextFloat(9, 11);
                Projectile.NewProjectile(player.position, Vector2.Normalize(Main.MouseWorld - player.Center) * speed, ModContent.ProjectileType<QuartzProjSwords>(), (int)(item.damage * 0.7f), item.knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
            return true;
        }
    }
}