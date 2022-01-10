using EEMod.Extensions;
using EEMod.Prim;
using EEMod.Tiles.Furniture.Chests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins
{
    public class GoblinGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if(npc.type == ModContent.NPCType<Shaman.GoblinShaman>() ||
               npc.type == ModContent.NPCType<Bard.CymbalBard>() ||
               npc.type == ModContent.NPCType<Berserker.GoblinBerserker>())
            {
                int proj = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(npc), npc.Center, Vector2.Zero, ModContent.ProjectileType<GoblinDeathBolt>(), 0, 0);

                Vector2 tilePos = Vector2.Zero;

                for(int i = 0; i < Main.maxTilesX; i++)
                {
                    for(int j = 0; j < Main.maxTilesY; j++)
                    {
                        if(Framing.GetTileSafely(i, j).type == ModContent.TileType<ShadowflameHexChestTile>())
                        {
                            tilePos = new Vector2((i * 16) + 16, (j * 16) + 16);

                            (Main.projectile[proj].ModProjectile as GoblinDeathBolt).Target = tilePos;

                            return;
                        }
                    }
                }
            }
        }
    }

    public class GoblinDeathBolt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.alpha = 0;

            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 10000;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Target - Projectile.Center) * 6, 0.02f);
            
            if(Vector2.DistanceSquared(Target, Projectile.Center) <= 8 * 8)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Kill();
            }
        }

        public Vector2 Target;

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, 0, 0, DustID.CrystalSerpent_Pink);
                Main.dust[dust].velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Texture2D mask = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Extra_49").Value;
            Texture2D bolt = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Goblins/Shaman/ShadowflameHexBolt").Value;

            Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition, Color.Violet, 0.5f, 0f);

            //Helpers.DrawAdditive(bolt, Projectile.Center - Main.screenPosition, Color.DarkViolet, 0.15f, 0f);

            //Main.spriteBatch.Draw(bolt, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(6, 6), Projectile.scale, SpriteEffects.None, 0);

            //lightColor = Color.White;

            return false;
        }
    }
}