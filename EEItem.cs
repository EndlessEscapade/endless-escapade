using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace EEMod
{
    public abstract class EEItem : ModItem
    {
        public virtual bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return Shoot(player, new ProjectileSource_Item_WithAmmo(player, Item, Item.useAmmo), position, new Vector2(speedX, speedY), type, damage, knockBack);
        }

        public virtual void HoldStyle(Player player)
        {
            HoldStyle(player, Item.getRect());
        }

        public virtual void UpdateVanity(Player player, EquipType type)
        {
            UpdateVanity(player);
        }
    }
}
