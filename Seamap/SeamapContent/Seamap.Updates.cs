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
            for (int i = 0; i < SeamapObjects.IslandEntities.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                {
                    Lighting.AddLight(SeamapObjects.IslandEntities[i].posToScreen, .4f, .4f, .4f);
                }

                if (i == 1)
                {
                    Lighting.AddLight(SeamapObjects.IslandEntities[i].posToScreen, .15f, .15f, .15f);
                }

                if (i == 2)
                {
                    Lighting.AddLight(SeamapObjects.IslandEntities[i].posToScreen, .4f, .4f, .4f);
                }

                if (i == 4)
                {
                    Lighting.AddLight(SeamapObjects.IslandEntities[i].posToScreen, .15f, .15f, .15f);
                }

                if (i == 7)
                {
                    Lighting.AddLight(SeamapObjects.IslandEntities[i].posToScreen, .4f, .4f, .4f);
                }

                if (i == 0)
                {
                    Lighting.AddLight(SeamapObjects.IslandEntities[i].posToScreen, .4f, .4f, .4f);
                }
            }
            SeamapPlayerShip ship = SeamapPlayerShip.localship;

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

            Vector2 v = new Vector2(eePlayer.boatSpeed * 4);
            ship.velocity = Vector2.Clamp(ship.velocity, -v, v);

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

        public static void UpdateSeamap()
        {
            MechanicManager.MidUpdateProjectileItem();
            EEPlayer.UpdateOceanMapElements();
            UpdateShipMovement();
        }
    }
}

