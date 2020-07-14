using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Armor.DevSets.Exitium
{
    [AutoloadEquip(EquipType.Wings)]
    public class ExitCrystal : ModItem
    {
        public bool DevWing;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exitium's Crystal");
            Tooltip.SetDefault("'Great for impersonating mod devs!'");
        }
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 26;
            item.rare = ItemRarityID.Cyan;
            item.accessory = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NPC.downedMechBoss1)
            {
                DevWing = true;
            }
            if (NPC.downedMechBoss2)
            {
                DevWing = true;
            }
            if (NPC.downedMechBoss3)
            {
                DevWing = true;
            }

            if (DevWing == false)
            {
                player.AddBuff(BuffID.Stoned, 1);
            }
            player.wingTimeMax = 150;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            player.yoraiz0rEye = 2;
            ascentWhenFalling = 1f;
            ascentWhenRising = 1f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 1f;
            constantAscend = 1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            player.yoraiz0rEye = 2;
            speed = 7f;
            acceleration *= 1;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            if (player.velocity.Y >= 1)
            {
                player.yoraiz0rEye = 2;
            }
            if (player.velocity.Y <= -1)
            {
                player.yoraiz0rEye = 2;
            }
            if (player.velocity.X >= 1 && player.velocity.Y >= 0.1)
            {
                player.yoraiz0rEye = 2;
            }
            if (player.velocity.X <= -1 && player.velocity.Y >= 0.1)
            {
                player.yoraiz0rEye = 2;
            }
            if (player.velocity.X >= 1 && player.velocity.Y <= -0.1)
            {
                player.yoraiz0rEye = 2;
            }
            if (player.velocity.X <= -1 && player.velocity.Y <= -0.1)
            {
                player.yoraiz0rEye = 2;
            }

            player.armorEffectDrawShadow = true;
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            player.armorEffectDrawShadowBasilisk = true;
            player.armorEffectDrawShadowEOCShield = true;
        }
    }
}