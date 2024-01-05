using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Projectiles.StarfishApprentice;

public class SpinnerFish : ModProjectile
{
    public ref float Timer => ref Projectile.ai[0];

    public Player Owner => Main.player[Projectile.owner];
    
    public override void SetDefaults() {
        Projectile.friendly = true;

        Projectile.width = 26;
        Projectile.height = 26;

        Projectile.aiStyle = -1;
        AIType = -1;
        
        Projectile.timeLeft = 300;
    }

    public override void AI() {
        Owner.heldProj = Projectile.whoAmI;
        
        if (Owner.channel) {
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
        }
    }
}
