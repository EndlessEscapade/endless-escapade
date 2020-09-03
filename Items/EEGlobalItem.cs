using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        //public override bool CloneNewInstances => true;

        public int fishLength = 0;
        private int[] averageSizeFish = { ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.CrimsonTigerfish, ItemID.Ebonkoi, ItemID.Obsidifish, ItemID.SpecularFish, ItemID.Stinkfish, ItemID.Tuna };
        private int[] smallSizeFish =  { ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.Hemopiranha, ItemID.NeonTetra, ItemID.PrincessFish, ItemID.RedSnapper, /*ItemID.RockLobster, */ ItemID.Salmon, ItemID.Trout };
        private int[] bigSizeFish = { ItemID.ChaosFish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.FlarefinKoi, /*ItemID.Flouder, */ ItemID.Prismite, ItemID.VariegatedLardfish };

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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (averageSizeFish.Contains(item.type) || smallSizeFish.Contains(item.type) || bigSizeFish.Contains(item.type))
            {
                TooltipLine newLine = new TooltipLine(mod, "FishLength", "Length: " + item.GetGlobalItem<EEGlobalItem>().fishLength + " inches");
                newLine.overrideColor = Color.Gold;
                tooltips.Add(newLine);
            }
        }

        public override void SetDefaults(Item item)
        {
            if (averageSizeFish.Contains(item.type) || smallSizeFish.Contains(item.type) || bigSizeFish.Contains(item.type))
                item.maxStack = 1;
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (averageSizeFish.Contains(item.type) && item.GetGlobalItem<EEGlobalItem>().fishLength == 32)
                Main.spriteBatch.Draw(TextureCache.Star, position, Color.White);
            if (smallSizeFish.Contains(item.type) && item.GetGlobalItem<EEGlobalItem>().fishLength == 16)
                Main.spriteBatch.Draw(TextureCache.Star, position, Color.White);
            if (bigSizeFish.Contains(item.type) && item.GetGlobalItem<EEGlobalItem>().fishLength == 44)
                Main.spriteBatch.Draw(TextureCache.Star, position, Color.White);
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (averageSizeFish.Contains(item.type))
            {
                if (item.GetGlobalItem<EEGlobalItem>().fishLength == 0)
                {
                    item.GetGlobalItem<EEGlobalItem>().fishLength = Main.rand.Next(12, 33) * (int)(1 + Main.player[Main.myPlayer].fishingSkill/100);
                    if (item.GetGlobalItem<EEGlobalItem>().fishLength > 32) fishLength = 32;
                    if (fishLength == 32)
                        item.value = item.value * 2;
                    else
                        item.value = item.GetGlobalItem<EEGlobalItem>().fishLength * (item.value / 100) * 5;
                }
            }
            if (smallSizeFish.Contains(item.type))
            {
                if (item.GetGlobalItem<EEGlobalItem>().fishLength == 0)
                {
                    item.GetGlobalItem<EEGlobalItem>().fishLength = Main.rand.Next(8, 17) * (int)(1 + Main.player[Main.myPlayer].fishingSkill / 100);
                    if (item.GetGlobalItem<EEGlobalItem>().fishLength > 16) fishLength = 16;
                    if (fishLength == 16)
                        item.value = item.value * 2;
                    else
                        item.value = item.GetGlobalItem<EEGlobalItem>().fishLength * (item.value / 100) * 5;
                }
            }
            if(bigSizeFish.Contains(item.type))
            {
                if (item.GetGlobalItem<EEGlobalItem>().fishLength == 0)
                {
                    item.GetGlobalItem<EEGlobalItem>().fishLength = Main.rand.Next(18, 45) * (int)(1 + Main.player[Main.myPlayer].fishingSkill / 100);
                    if (item.GetGlobalItem<EEGlobalItem>().fishLength > 44) fishLength = 44;
                    if (fishLength == 44)
                        item.value = item.value * 2;
                    else
                        item.value = item.GetGlobalItem<EEGlobalItem>().fishLength * (item.value / 100) * 5;
                }
            }
            return true;
        }

        /*public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(item.type == ItemID.Boomstick || item.type == ItemID.StarCannon || item.type == ItemID.SnowballCannon || item.type == ItemID.Shotgun || item.type == ItemID.TacticalShotgun || item.type == ItemID.OnyxBlaster || item.type == ItemID.RocketLauncher || item.type == ItemID.JackOLanternLauncher || item.type == ItemID.SnowmanCannon || item.type == ItemID.FireworksLauncher)
            {
                player.velocity += -Vector2.Normalize(Main.MouseWorld - player.Center) * 8;
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }*/
    }
}