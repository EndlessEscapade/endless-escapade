using EEMod.Seamap.SeamapAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEMod;
namespace EEMod.Seamap.SeamapContent
{
    public class SeamapUpdates
    {
        public static void UpdateShipMovement()
        {
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            //TODO: Refactor integrating lighting system into Islands
            for (int i = 0; i < eePlayer.objectPos.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }

                if (i == 1)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                }

                if (i == 2)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }

                if (i == 4)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                }

                if (i == 7)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }

                if (i == 0)
                {
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                }
            }
            instance.position.X = MathHelper.Clamp(instance.position.X, Main.screenWidth * 0.6f, Main.screenWidth);
            instance.position.Y = MathHelper.Clamp(instance.position.Y, 0, Main.screenHeight);
            if (!Main.gamePaused)
            {
                instance.position += instance.velocity;
                if (player.controlJump)
                {
                    instance.velocity.Y -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlDown)
                {
                    instance.velocity.Y += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlRight)
                {
                    instance.velocity.X += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlLeft)
                {
                    instance.velocity.X -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlUseItem && instance.cannonDelay <= 0 && eePlayer.cannonballType != 0)
                {
                    switch (eePlayer.cannonballType)
                    {
                        case 1:
                            Projectile.NewProjectile(instance.position + Main.screenPosition, -Vector2.Normalize(instance.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyCannonball>(), 0, 0);
                            break;

                        case 2:
                            Projectile.NewProjectile(instance.position + Main.screenPosition, -Vector2.Normalize(instance.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyExplosiveCannonball>(), 0, 0);
                            break;

                        case 3:
                            Projectile.NewProjectile(instance.position + Main.screenPosition, -Vector2.Normalize(instance.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyHallowedCannonball>(), 0, 0);
                            break;

                        case 4:
                            Projectile.NewProjectile(instance.position + Main.screenPosition, -Vector2.Normalize(instance.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyChlorophyteCannonball>(), 0, 0);
                            break;

                        case 5:
                            Projectile.NewProjectile(instance.position + Main.screenPosition, -Vector2.Normalize(instance.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyLuminiteCannonball>(), 0, 0);
                            break;
                    }
                    Main.PlaySound(SoundID.Item61);
                    instance.cannonDelay = 60;
                }
                instance.cannonDelay--;
            }

            instance.velocity.X = Helpers.Clamp(instance.velocity.X, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            instance.velocity.Y = Helpers.Clamp(instance.velocity.Y, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);

            if (!Main.gamePaused)
            {
                instance.velocity *= 0.98f;
            }
            
            instance.flash += 0.01f;
            if (instance.flash == 2)
            {
                instance.flash = 10;
            }
        }
    }
}

