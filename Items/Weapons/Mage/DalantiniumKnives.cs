using EEMod.Extensions;
using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Mage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class DalantiniumKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Knives");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 13;
            item.magic = true;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 21);
            item.mana = 7;
            item.shootSpeed = 4f;
            item.useTime = 65;
            item.useAnimation = 65;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<DalantiniumFan>();
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
        }

        public override Vector2? HoldoutOffset()
        {
            return Vector2.Zero;
        }
        int powerUp;
        int anim;
        bool isInHand;

        public override void HoldItem(Player player)
        {
            isInHand = true;
            anim++;
            if (powerUp < 2000)
                powerUp++;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (anim < 200)
            {
                Texture2D tex = mod.GetTexture("Projectiles/Mage/DalantiniumFanSheet");
                Rectangle rect = new Rectangle(0, 0, tex.Width, tex.Height);
                spriteBatch.Draw(tex, Main.LocalPlayer.Center.ForDraw() + new Vector2(0, -100 + (float)Math.Sin(anim / 10f)), rect, lightColor, 0f, rect.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void UpdateInventory(Player player)
        {
            if (!isInHand)
            {
                anim = 0;
            }
            isInHand = false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Helpers.TexToDust("thonk",Main.MouseWorld,5,1,100);
            if (player.altFunctionUse == 0)
            {
                type = ModContent.ProjectileType<DalantiniumFan>();
                item.shoot = ModContent.ProjectileType<DalantiniumFan>();
                Projectile projectile = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                powerUp += 200;
                (projectile.modProjectile as DalantiniumFan).boost = powerUp;
                if (Main.netMode != NetmodeID.Server)
                {
                    EEMod.prims.CreateTrail(projectile);
                }
            }
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<DalantiniumFanAlt>();
                item.shoot = ModContent.ProjectileType<DalantiniumFanAlt>();
                Projectile projectile = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                powerUp = 0;
                if (Main.netMode != NetmodeID.Server)
                {
                    EEMod.prims.CreateTrail(projectile);
                }
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return powerUp > 2000;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 7);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }
    }
}
