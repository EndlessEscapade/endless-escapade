using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Combo;

namespace EEMod.Items.Weapons.Classes
{
    public abstract class ComboWeaponItem : EEItem
    {
        public int CurrentCombo;
        public abstract int NumberOfCombinations { get; }
        public abstract int ComboProjectile { get; }
        public abstract void ComboChangeBehaviour();
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            ComboChangeBehaviour();
            Projectile projectile = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_Item(player, Item), position, new Vector2(speedX, speedY), ComboProjectile, damage, knockBack, player.whoAmI);
            try
            {
                (projectile.ModProjectile as ComboWeapon).SetCombo(CurrentCombo);
            }
            catch
            {
                Main.NewText("Not a combo class weapon dummy");
            }
            return false;
        }
    }
}