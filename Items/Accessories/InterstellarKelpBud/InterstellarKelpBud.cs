using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace EEMod.Items.Accessories.InterstellarKelpBud
{
    public class InterstellarKelpBud : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Interstellar Kelp Bud");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 18;
            item.value = Item.buyPrice(0, 0, 20, 0);
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
            item.defense = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<KelpBudPlayer>().interstellarKelpBud = true;
        }
    }

    public class KelpBudPlayer : ModPlayer
    {
        public bool interstellarKelpBud = false;
        public bool inKelpRing = false;

        public override void NaturalLifeRegen(ref float regen)
        {
            if (inKelpRing)
            {
                regen *= 1.3f;
            }
        }
    }


    public class KelpBudNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (Main.rand.NextBool(5) && Main.LocalPlayer.GetModPlayer<KelpBudPlayer>().interstellarKelpBud)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(0, 0.5f), ModContent.ProjectileType<KelpBudProjectile>(), 0, 0f);
            }
        }

        public override bool PreNPCLoot(NPC npc)
        {
            return base.PreNPCLoot(npc);
        }
    }

    public class KelpBudProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Interstellar Kelp Bud");
        }

        public override void SetDefaults()
        {
            projectile.tileCollide = true;
            projectile.width = 12;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 1240;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.1f;

            projectile.ai[0]++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Vector2 ringOffset = new Vector2(-6, 0);
        private bool isDying = false;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] < 120)
            {
                Texture2D tex = ModContent.GetTexture("EEMod/Items/Accessories/InterstellarKelpBud/KelpBudProjectile");
                Main.spriteBatch.Draw(tex, projectile.position + new Vector2(0, 10) - Main.screenPosition, tex.Bounds, Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16) * (1 - (projectile.alpha / 255f)), 0f, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            else if (projectile.ai[0] >= 60 && projectile.ai[0] < 180)
            {
                Texture2D tex = ModContent.GetTexture("EEMod/Items/Accessories/InterstellarKelpBud/InterstellarKelpBudMid");
                Main.spriteBatch.Draw(tex, projectile.position + new Vector2(0, 8) - Main.screenPosition, tex.Bounds, Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16) * (1 - (projectile.alpha / 255f)), 0f, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Texture2D tex = ModContent.GetTexture("EEMod/Items/Accessories/InterstellarKelpBud/InterstellarKelpBudBig");
                Main.spriteBatch.Draw(tex, projectile.position + new Vector2(0, 4) - Main.screenPosition, tex.Bounds, Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16) * (1 - (projectile.alpha / 255f)), 0f, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);

                Texture2D ringTex = ModContent.GetTexture("EEMod/Textures/InverseMask");

                float ringScale = (MathHelper.Clamp((float)Math.Sqrt((projectile.ai[0] - 180) / 60f), 0f, 1f) + ((projectile.ai[0] >= 270) ? (float)(Math.Sin(Main.GameUpdateCount / 60f) / 35f) : 0)) * MathHelper.Clamp((float)Math.Sqrt((1160 - projectile.ai[0]) / 40f), 0f, 1f);

                Helpers.DrawAdditive(ringTex, projectile.Center - Main.screenPosition + ringOffset, Color.Goldenrod, ringScale);


                if (projectile.ai[0] >= 270 && projectile.ai[0] < 1160)
                {
                    float ringScale2 = MathHelper.Clamp((float)Math.Sqrt(projectile.ai[0] % 90.0 / 80.0), 0f, 1f);

                    Helpers.DrawAdditive(ringTex, projectile.Center - Main.screenPosition + ringOffset, Color.DarkGoldenrod * ((70 - (projectile.ai[0] % 90)) / 80f), ringScale2);

                    Color chosen = Color.Lerp(Color.Gold, Color.DarkGoldenrod, Main.rand.NextFloat(1f));

                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.05f));

                    Vector2 location = new Vector2(Main.rand.Next(0, (int)(90 * ringScale)), 0).RotatedByRandom(6.28f) + projectile.Center + ringOffset;
                    Vector2 tileLocation = location / 16;

                    if (!Main.tile[(int)tileLocation.X, (int)tileLocation.Y].active())
                    {
                        EEMod.MainParticles.SpawnParticles(location, Vector2.Zero, mod.GetTexture("Particles/BigPlusSign"), 30, Main.rand.NextFloat(1f, 1.5f), chosen, new SetMask(ModContent.GetInstance<EEMod>().GetTexture("Textures/RadialGradient"), 0.6f));
                    }
                }

                if (Vector2.Distance(Main.player[projectile.owner].Center, projectile.Center + ringOffset) < 200 * ringScale)
                {
                    Main.player[projectile.owner].GetModPlayer<KelpBudPlayer>().inKelpRing = true;
                }
                else
                {
                    Main.player[projectile.owner].GetModPlayer<KelpBudPlayer>().inKelpRing = false;
                }

                Lighting.AddLight(projectile.Center, Color.Gold.ToVector3() * ringScale / 2f);

                if((projectile.ai[0] >= 1160 && ringScale <= 0.05f) || isDying)
                {
                    isDying = true;

                    projectile.ai[1]++;

                    projectile.alpha += 16;

                    float ringScale2 = MathHelper.Clamp((float)Math.Sqrt((projectile.ai[1] % 120) / 120), 0f, 1f);

                    Helpers.DrawAdditive(ringTex, projectile.Center - Main.screenPosition + ringOffset, Color.DarkGoldenrod * (1 - (projectile.alpha / 255f)), ringScale2);
                }
            }

            return false;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].GetModPlayer<KelpBudPlayer>().inKelpRing = false;
        }
    }
}