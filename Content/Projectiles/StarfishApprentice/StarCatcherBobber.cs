using EndlessEscapade.Content.Items.StarfishApprentice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
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

    public override bool PreDraw(ref Color lightColor) {
        var texture = ModContent.Request<Texture2D>(Texture + "_Outline").Value;
        var effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        return true;
    }
}
