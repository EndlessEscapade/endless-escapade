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
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return Shoot(player, ref position, ref velocity.X, ref velocity.Y, ref type, ref damage, ref knockback);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            HoldStyle(player);
        }

        public virtual void HoldStyle(Player player)
        {

        }

        public override void UpdateVanity(Player player)
        {

        }

        public virtual void UpdateVanity(Player player, EquipType type)
        {
        }
    }
}
