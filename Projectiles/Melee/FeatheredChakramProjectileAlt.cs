using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class FeatheredChakramProjectileAlt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 60;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
        }

        public void Draw(GraphicsDevice device)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];

            //method to make it look less horrible
            int currentIndex = 0;
            void AddVertex(Vector2 position, Color color, Vector2 uv)
            {
                vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
            }
            AddVertex(projectile.Center - new Vector2(-200,0), Color.White, Vector2.Zero);
            AddVertex(projectile.Center - new Vector2(200, 0), Color.White, new Vector2(1,0));
            AddVertex(projectile.Center - new Vector2(200, 200), Color.White, new Vector2(1, 1));
            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);
        }
        public override void AI()
        {
            Draw(Main.spriteBatch.GraphicsDevice);
            if (projectile.ai[1] == 0)
            {

            }
            int dust = Dust.NewDust(projectile.Center, 0, 0, 127);
            projectile.Center = Main.player[projectile.owner].Center;
            projectile.rotation += 5;
            projectile.ai[1]++;
            projectile.ai[0]+=0.02f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}