using EndlessEscapade.Content.Items.StarfishApprentice;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Projectiles.StarfishApprentice;

public class StarCatcherBobber : ModProjectile
{
    public override void SetDefaults() {
        Projectile.netImportant = true;
        Projectile.bobber = true;
        
        Projectile.width = 14;
        Projectile.height = 14;
        
        Projectile.aiStyle = ProjAIStyleID.Bobber;
    }

    public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor) {
        lineOriginOffset = new Vector2(46, -36);
    }
}
