using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using EEMod.Prim;

namespace EEMod.Items.Weapons.Melee
{
    public class StormGauntlet : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Gauntlet");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.shootSpeed = 0f;
            Item.knockBack = 6.5f;
            Item.width = 54;
            Item.height = 60;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            // Item.autoReuse = false;
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<StormGauntletProj>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LythenBar>(), 10).AddTile(TileID.Anvils).Register();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<StormGauntletProj>()] >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int myProj = -1;
        public override bool AltFunctionUse(Player player)
        {
            if (myProj != -1)
            {
                Main.projectile[myProj].Kill();
                myProj = -1;
            }


            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            myProj = Projectile.NewProjectile(new ProjectileSource_Item(player, Item), position, new Vector2(speedX, speedY), type, damage, knockBack);

            //Main.NewText("Shooting ish");

            //if (Main.netMode != NetmodeID.Server)
            //{
            //PrimitiveSystem.primitives.CreateTrail(new StormGauntletPrimTrail(funi, 2f, 1f));

                //Main.NewText("Shooting properly");
            //}

            return false;
        }
    }
}