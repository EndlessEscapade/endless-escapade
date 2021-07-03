using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using EEMod.Extensions;

namespace EEMod.Items.Armor.StormKnight
{
    [AutoloadEquip(EquipType.Head)]
    public class StormKnightCrest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Knight's Crest");
            Tooltip.SetDefault("5% increased melee damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 21);
            item.rare = ItemRarityID.Green;
            item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StormKnightBreastplate>() && legs.type == ModContent.ItemType<StormKnightLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.05f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "True melee hits call down a lightning strike from the sky";
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LythenBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class StormKnightCrestLayer : ModPlayer
    {
        public static readonly PlayerLayer StormKnightCrestGlow = new PlayerLayer("EEMod", "StormKnightCrest", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            Texture2D glow = ModContent.GetTexture("EEMod/Items/Armor/StormKnight/StormKnightCrestGlow");

            Player player = drawInfo.drawPlayer;

            DrawData data = new DrawData(glow, player.position - Main.screenPosition, new Rectangle(0, player.headFrame.Y, 40, 56), Color.Green, player.headRotation, Vector2.Zero, 1f, player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            Main.NewText("" + player.headFrameCounter);
            Main.playerDrawData.Add(data);
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            StormKnightCrestGlow.visible = true;
            layers.Add(StormKnightCrestGlow);
        }
    }
}