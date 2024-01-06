using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.StarfishApprentice;

public class StarCatcher : ModItem
{
    public override void SetDefaults() {
        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.useStyle = ItemUseStyleID.Swing;
        
        Item.fishingPole = 15;

        Item.width = 52;
        Item.height = 44;

        Item.shoot = ModContent.ProjectileType<Projectiles.StarfishApprentice.StarCatcherBobber>();
        Item.shootSpeed = 10f;
        
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
    }
    
    public override void HoldItem(Player player) {
        player.accFishingLine = true;
    }
}
