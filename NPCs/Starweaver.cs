using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.Tiles.Furniture.OrbHolder;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Enemy;

namespace EEMod.NPCs
{
    public class Starweaver : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starweaver");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = false;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.alpha = 0;
            npc.lifeMax = 200;
            npc.width = 32;
            npc.height = 44;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.damage = 10;
            npc.knockBackResist = 0f;
        }

        private bool spawningStars;
        private int chosenConstellation;
        private int index;
        private Vector2[][] constellations = new Vector2[][]
        {
            new Vector2[] { new Vector2(-64, -64), new Vector2(-32, -50), new Vector2(0, -50), new Vector2(32, -64), new Vector2(24, -80), new Vector2(64, -96), new Vector2(64, -72) }
        };

        public override void AI()
        {
            Player target = Main.player[npc.FindClosestPlayer()];

            npc.ai[0]++;

            if(npc.ai[0] % 900 == 0)
            {
                spawningStars = true;
                chosenConstellation = Main.rand.Next(0, constellations.Length);
                index = 0;
            }

            if(spawningStars && npc.ai[0] % 60 == 0 && index < constellations[chosenConstellation].Length)
            {
                Projectile.NewProjectile(npc.Center + constellations[chosenConstellation][index], Vector2.Zero, ModContent.ProjectileType<StarweaverStar>(), 40, 2f);

                index++;
            }
        }
    }
}