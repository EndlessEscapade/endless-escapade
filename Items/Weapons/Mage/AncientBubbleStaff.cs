using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Mage;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Items.Weapons.Mage
{
    public class AncientBubbleStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble Staff");
            Tooltip.SetDefault("Casts an ancient bubble\nRight click the bubble after shooting it to pop it");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18, 0);
            item.damage = 13;
            item.useTime = 120;
            item.useAnimation = 120;
            item.width = 46;
            item.height = 42;
            item.mana = 12;
            item.shootSpeed = 0f;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<AncientBubbleLarge>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -8);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            Vector2 vec = new Vector2(item.position.X + item.width * 0.5f, item.position.Y + item.height - texture.Height * 0.5f);

            spriteBatch.Draw(ModContent.GetTexture("EEMod/Items/Weapons/Mage/AncientBubbleStaffGlow"), vec.ForDraw(), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}