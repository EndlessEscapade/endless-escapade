using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EEMod.Items
{
    public class EEGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        //public override bool CloneNewInstances => true;

        //public override bool CloneNewInstances => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }

        public bool caught;
        public int fishLength = 0;
        public List<int> averageSizeFish = new() { ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.CrimsonTigerfish, ItemID.Ebonkoi, ItemID.Obsidifish, ItemID.SpecularFish, ItemID.Stinkfish, ItemID.Tuna };
        public List<int> smallSizeFish = new() { ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.Hemopiranha, ItemID.NeonTetra, ItemID.PrincessFish, ItemID.RedSnapper, /*ItemID.RockLobster, */ ItemID.Salmon, ItemID.Trout };
        public List<int> bigSizeFish = new() { ItemID.ChaosFish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.FlarefinKoi, /*ItemID.Flouder, */ ItemID.Prismite, ItemID.VariegatedLardfish };

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            /*var eeplayer = player.GetModPlayer<EEPlayer>();
            if (eeplayer.hydriteVisage)
            {
                reduce -= 0.1f;
            }*/
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

        /*public override void SetStaticDefaults(Item item)
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
                int length = 0;
                if (modPlayer.fishLengths.ContainsKey(item.type))
                {
                    current = modPlayer.fishLengths[item.type];
                }
                if (averageSizeFish.Contains(item.type))
                {
                    length = Helpers.Clamp(Main.rand.Next(12, 33) * (1 + player.fishingSkill / 100), 0, 32);
                    modPlayer.fishLengths[item.type] = Math.Max(length, current);

                    if (length > current && current != 0)
                        CombatText.NewText(player.getRect(), Color.Yellow, $"New Record! {length} cm", true);
                    else
                        CombatText.NewText(player.getRect(), Color.Cyan, $"Length: {length} cm");
                }
                if (smallSizeFish.Contains(item.type))
                {
                    length = Helpers.Clamp(Main.rand.Next(8, 17) * (1 + player.fishingSkill / 100), 0, 16);
                    modPlayer.fishLengths[item.type] = Math.Max(length, current);

                    if (length > current && current != 0)
                        CombatText.NewText(player.getRect(), Color.Yellow, $"New Record! {length} cm", true);
                    else
                        CombatText.NewText(player.getRect(), Color.Cyan, $"Length: {length} cm");
                }
                if (bigSizeFish.Contains(item.type))
                {
                    length = Helpers.Clamp(Main.rand.Next(18, 45) * (1 + player.fishingSkill / 100), 0, 44);
                    modPlayer.fishLengths[item.type] = Math.Max(length, current);

                    if (length > current && current != 0)
                        CombatText.NewText(player.getRect(), Color.Yellow, $"New Record! {length} cm", true);
                    else
                        CombatText.NewText(player.getRect(), Color.Cyan, $"Length: {length} cm");
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
        /*public override bool NeedsSaving(Item item)
        {
            return true;
        }
        public override TagCompound Save(Item item)
        {
            return new TagCompound { 
                ["caught"] = caught
            };
        }
        public override void Load(Item item, TagCompound tag)
        {
            tag.TryGetRef("caught", ref caught);
        }*/
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(caught);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            caught = reader.ReadBoolean();
        }
    }
}