using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Hooks
{
    public class SailorsClaspProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
        }

        public override void SetDefaults()
        {
            /*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
            Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
        }

        // Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            for (int l = 0; l < 1000; l++)
            {
                if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type)
                {
                    hooksOut++;
                }
            }
            if (hooksOut > 3) // This hook can have 3 hooks out.
            {
                return false;
            }
            return true;
        }

        public override float GrappleRange()
        {
            return 600f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 3;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 14f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 14f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = playerCenter - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distanceSQ = distToProj.LengthSquared();
            while (distanceSQ > 30f && !float.IsNaN(distanceSQ))
            {
                distToProj.Normalize();                 //get unit vector
                distToProj *= 24f;                      //speed = 24
                center += distToProj;                   //update draw position
                distToProj = playerCenter - center;    //update distance
                //distance = distToProj.Length();
                Color drawColor = lightColor;

                //Draw chain
                Main.spriteBatch.Draw(EEMod.Instance.Assets.Request<Texture2D>("Projectiles/Hooks/SailorsClaspChain").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 16, 6), drawColor, projRotation,
                    new Vector2(Terraria.GameContent.TextureAssets.Chain30.Value.Width * 0.5f, Terraria.GameContent.TextureAssets.Chain30.Value.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}