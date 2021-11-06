using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using EEMod.Extensions;
using EEMod.Items.Weapons.Melee;
using EEMod.Prim;

namespace EEMod.Items.Armor.StormKnight
{
    [AutoloadEquip(EquipType.Head)]
    public class StormKnightCrest : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Knight's Crest");
            Tooltip.SetDefault("5% increased melee damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 21);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StormKnightBreastplate>() && legs.type == ModContent.ItemType<StormKnightLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.05f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Dealing melee damage fills up your storm bar\nWhen you have a full storm bar, a lightning bolt will shoot out with your next attack";
            player.GetModPlayer<StormKnightPlayer>().setComplete = true;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LythenBar>(), 11).AddTile(TileID.Anvils).Register();
        }
    }

    public class StormKnightPlayer : ModPlayer
    {
        public int tallyDamage;
        public bool setComplete;

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (setComplete)
            {
                if (item.DamageType == DamageClass.Melee)
                {
                    tallyDamage += (int)damage;
                }

                if (tallyDamage >= 200)
                {
                    int test = Helpers.GetNearestNPC(Player.Center, false, false);

                    if (test >= 0)
                    {
                        NPC npc = Main.npc[test];

                        int ballfart = Projectile.NewProjectile(new ProjectileSource_Item(Player, item), Player.Center, Vector2.Normalize(npc.Center - Player.Center) * 15f, ModContent.ProjectileType<AxeLightning>(), (int)(item.damage * 0.25f), 2.5f);
                        if (Main.netMode != NetmodeID.Server)
                        {
                            PrimitiveSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[ballfart], 4, 0.75f));
                        }

                        tallyDamage = 0;
                    }
                }
            }
        }
    }

    public class StormKnightCrestGlow : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return !Main.gameMenu && drawInfo.drawPlayer.armor[0].type == ModContent.ItemType<StormKnightCrest>();
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D glow = ModContent.Request<Texture2D>("EEMod/Items/Armor/StormKnight/StormKnightCrestGlow").Value;

            Texture2D bar = ModContent.Request<Texture2D>("EEMod/Items/Armor/StormKnight/BarBg").Value;
            Texture2D fill = ModContent.Request<Texture2D>("EEMod/Items/Armor/StormKnight/BarFill").Value;

            Player player = drawInfo.drawPlayer;

            DrawData data = new DrawData(glow, player.position - Main.screenPosition - new Vector2(10, 10), new Rectangle(0, player.headFrame.Y, 40, 56), Color.White, player.headRotation, Vector2.Zero, 1f, player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            drawInfo.DrawDataCache.Add(data);


            int rectWidth = (int)MathHelper.Clamp(42 * (drawInfo.drawPlayer.GetModPlayer<StormKnightPlayer>().tallyDamage / 200f), 0, 42);

            Vector2 offset = Vector2.Zero;

            if (rectWidth >= 42) offset = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));

            DrawData data2 = new DrawData(bar, player.Center - Main.screenPosition + new Vector2(0, -40) + offset, null, Color.White, player.headRotation, bar.Bounds.Size() / 2f, 1f, SpriteEffects.None, 0);
            DrawData data3 = new DrawData(fill, player.Center - Main.screenPosition + new Vector2(-1, -40) + offset, new Rectangle(0, 0, rectWidth, 6), Color.White, player.headRotation, fill.Bounds.Size() / 2f, 1f, SpriteEffects.None, 0);

            drawInfo.DrawDataCache.Add(data2);
            drawInfo.DrawDataCache.Add(data3);
        }
    }
}