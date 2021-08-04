using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Items.Weapons.Mage
{
    public class AncientBubbleStaff : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble Staff");
            Tooltip.SetDefault("Casts an ancient bubble\nRight click the bubble after shooting it to pop it");
        }

        public override void SetDefaults()
        {
            Item.melee = false;
            Item.magic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 18, 0);
            Item.damage = 13;
            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.width = 46;
            Item.height = 42;
            Item.mana = 12;
            Item.shootSpeed = 0f;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.HoldingUp;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<AncientBubbleLarge>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -8);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture;
            texture = Main.itemTexture[Item.type];
            Vector2 vec = new Vector2(Item.position.X + Item.width * 0.5f, Item.position.Y + Item.height - texture.Height * 0.5f);

            spriteBatch.Draw(ModContent.GetTexture("EEMod/Items/Weapons/Mage/AncientBubbleStaffGlow"), vec.ForDraw(), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}