using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        //public override bool CloneNewInstances => true;

        public bool caught;
        public int fishLength = 0;
        public readonly int[] averageSizeFish = { ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.CrimsonTigerfish, ItemID.Ebonkoi, ItemID.Obsidifish, ItemID.SpecularFish, ItemID.Stinkfish, ItemID.Tuna };
        public readonly int[] smallSizeFish = { ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.Hemopiranha, ItemID.NeonTetra, ItemID.PrincessFish, ItemID.RedSnapper, /*ItemID.RockLobster, */ ItemID.Salmon, ItemID.Trout };
        public readonly int[] bigSizeFish = { ItemID.ChaosFish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.FlarefinKoi, /*ItemID.Flouder, */ ItemID.Prismite, ItemID.VariegatedLardfish };

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

        /*public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (averageSizeFish.Contains(item.type) || smallSizeFish.Contains(item.type) || bigSizeFish.Contains(item.type))
            {
                TooltipLine newLine = new TooltipLine(mod, "FishLength", "Length: " + item.GetGlobalItem<EEGlobalItem>().fishLength + " inches");
                newLine.overrideColor = Color.Gold;
                tooltips.Add(newLine);
            }
        }*/

        /*public override void SetDefaults(Item item)
        {
            if (averageSizeFish.Contains(item.type) || smallSizeFish.Contains(item.type) || bigSizeFish.Contains(item.type))
                item.maxStack = 1;
        }*/

        /*public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (averageSizeFish.Contains(item.type) && item.GetGlobalItem<EEGlobalItem>().fishLength == 32)
                Main.spriteBatch.Draw(TextureCache.Star, position, Color.White);
            if (smallSizeFish.Contains(item.type) && item.GetGlobalItem<EEGlobalItem>().fishLength == 16)
                Main.spriteBatch.Draw(TextureCache.Star, position, Color.White);
            if (bigSizeFish.Contains(item.type) && item.GetGlobalItem<EEGlobalItem>().fishLength == 44)
                Main.spriteBatch.Draw(TextureCache.Star, position, Color.White);
        }*/

        public override bool OnPickup(Item item, Player player)
        {
            if (!caught)
            {
                EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
                int current = 0;
                if (modPlayer.fishLengths.ContainsKey(item.type))
                {
                    current = modPlayer.fishLengths[item.type];
                }
                if (averageSizeFish.Contains(item.type))
                {
                    modPlayer.fishLengths[item.type] = Math.Max(Helpers.Clamp(Main.rand.Next(12, 33) * (1 + player.fishingSkill / 100), 0, 32), current);
                }
                if (smallSizeFish.Contains(item.type))
                {
                    modPlayer.fishLengths[item.type] = Math.Max(Helpers.Clamp(Main.rand.Next(8, 17) * (1 + player.fishingSkill / 100), 0, 16), current);
                }
                if (bigSizeFish.Contains(item.type))
                {
                    modPlayer.fishLengths[item.type] = Math.Max(Helpers.Clamp(Main.rand.Next(18, 45) * (1 + player.fishingSkill / 100), 0, 44), current);
                }
                caught = true;
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