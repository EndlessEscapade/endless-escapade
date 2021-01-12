using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;
using EEMod.NPCs.CoralReefs;

namespace EEMod.Projectiles.Enemy
{
    public class SpireLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.timeLeft = 30000;
            projectile.ignoreWater = true;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.hide = true;
            projectile.tileCollide = true;
        }

        /*public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Tile currentTile = Main.tile[(int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16)];
            if (currentTile != null && currentTile.active() && currentTile.type == ModContent.TileType<EmptyTile>())
            {
                Bounce(projectile.modProjectile, oldVelocity);
            }
            else
            {
                projectile.Kill();
            }
            return false;
        }*/

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1.5f)
        {
            Projectile projectile = modProj.projectile;
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X * bouncyness;
            }

            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y * bouncyness;
            }
            Main.PlaySound(SoundID.DD2_WitherBeastDeath, projectile.Center);
            projectile.ai[0]++;
        }

        public override void AI()
        {
            NPC spire = null;
            for(int i = 0; i < Main.npc.Length - 1; i++)
            {
                if(Main.npc[i].type == ModContent.NPCType<AquamarineSpire>())
                {
                    spire = Main.npc[i];
                }
            }

            if (spire != null)
            {
                if (Vector2.DistanceSquared(spire.Center, projectile.Center) <= 12 * 12 && projectile.ai[0] > 1)
                {
                    projectile.Kill();
                }
            }


            Tile currentTile = Main.tile[(int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16)];
            if (currentTile != null && currentTile.active() && currentTile.type == ModContent.TileType<EmptyTile>())
            {
                Bounce(projectile.modProjectile, projectile.oldVelocity);
            }
            else if (currentTile != null && currentTile.active() && currentTile.type != ModContent.TileType<EmptyTile>())
            {
                projectile.Kill();
            }
        }
    }
}