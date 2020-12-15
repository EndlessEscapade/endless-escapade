using EEMod.Seamap.SeamapAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEMod;
namespace EEMod.Seamap.SeamapContent
{
    public partial class Seamap
    {
        public static void UpdateShipMovement()
        {
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            //TODO: Refactor integrating lighting system into Islands
            for (int i = 0; i < eePlayer.SeaObject.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                {
                    Lighting.AddLight(eePlayer.SeaObject[i].posToScreen, .4f, .4f, .4f);
                }

                if (i == 1)
                {
                    Lighting.AddLight(eePlayer.SeaObject[i].posToScreen, .15f, .15f, .15f);
                }

                if (i == 2)
                {
                    Lighting.AddLight(eePlayer.SeaObject[i].posToScreen, .4f, .4f, .4f);
                }

                if (i == 4)
                {
                    Lighting.AddLight(eePlayer.SeaObject[i].posToScreen, .15f, .15f, .15f);
                }

                if (i == 7)
                {
                    Lighting.AddLight(eePlayer.SeaObject[i].posToScreen, .4f, .4f, .4f);
                }

                if (i == 0)
                {
                    Lighting.AddLight(eePlayer.SeaObject[i].posToScreen, .4f, .4f, .4f);
                }
            }
            SeamapPlayerShip ship = SeamapPlayerShip.localship;
            #region Clamp in screen
            ship.position.X = MathHelper.Clamp(ship.position.X, Main.screenWidth * 0.6f, Main.screenWidth);
            ship.position.Y = MathHelper.Clamp(ship.position.Y, 0, Main.screenHeight);
            #endregion

            #region Player controls(movement and shooting)
            if (!Main.gamePaused)
            {
                ship.position += ship.velocity;
                if (player.controlUp)
                {
                    ship.velocity.Y -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlDown)
                {
                    ship.velocity.Y += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlRight)
                {
                    ship.velocity.X += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlLeft)
                {
                    ship.velocity.X -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlUseItem && ship.cannonDelay <= 0 && eePlayer.cannonballType != 0)
                {
                    switch (eePlayer.cannonballType)
                    {
                        case 1:
                            Projectile.NewProjectile(ship.position + Main.screenPosition, -Vector2.Normalize(ship.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyCannonball>(), 0, 0);
                            break;

                        case 2:
                            Projectile.NewProjectile(ship.position + Main.screenPosition, -Vector2.Normalize(ship.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyExplosiveCannonball>(), 0, 0);
                            break;

                        case 3:
                            Projectile.NewProjectile(ship.position + Main.screenPosition, -Vector2.Normalize(ship.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyHallowedCannonball>(), 0, 0);
                            break;

                        case 4:
                            Projectile.NewProjectile(ship.position + Main.screenPosition, -Vector2.Normalize(ship.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyChlorophyteCannonball>(), 0, 0);
                            break;

                        case 5:
                            Projectile.NewProjectile(ship.position + Main.screenPosition, -Vector2.Normalize(ship.position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyLuminiteCannonball>(), 0, 0);
                            break;
                    }
                    Main.PlaySound(SoundID.Item61);
                    ship.cannonDelay = 60;
                }
                ship.cannonDelay--;
            }

            Vector2 v = new Vector2(eePlayer.boatSpeed);
            ship.velocity = Vector2.Clamp(ship.velocity, -v, v);
            //ship.velocity.X = Helpers.Clamp(ship.velocity.X, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            //ship.velocity.Y = Helpers.Clamp(ship.velocity.Y, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);

            if (!Main.gamePaused)
            {
                ship.velocity *= 0.98f;
            }
            #endregion

            ship.flash += 0.01f;
            if (ship.flash == 2)
            {
                ship.flash = 10;
            }
        }
    }
}

