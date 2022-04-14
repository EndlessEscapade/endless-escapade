using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Mounts
{
    public class SurfboardMount : ModMount
    {
        public override void SetStaticDefaults()
        {
            //MountData.spawnDust = DustID.Smoke;
            MountData.buff = ModContent.BuffType<SurfboardBuff>();
            MountData.heightBoost = 10;
            MountData.fallDamage = 0.5f;
            MountData.runSpeed = 11f;
            MountData.dashSpeed = 8f;
            MountData.flightTimeMax = 0;
            MountData.fatigueMax = 0;
            MountData.jumpHeight = 10;
            MountData.acceleration = 0.19f;
            MountData.jumpSpeed = 8f;
            MountData.blockExtraJumps = true;
            MountData.totalFrames = 1;
            MountData.constantJump = true;
            int[] array = new int[MountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 20;
            }
            MountData.playerYOffsets = array;
            MountData.xOffset = 0;
            MountData.yOffset = 0;
            MountData.playerHeadOffset = 32;
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            MountData.textureWidth = 56;
            MountData.textureHeight = 14;
        }

        public override void UpdateEffects(Player player)
        {
            if (player.wet)
            {
                if (player.velocity.Y > -3f) player.velocity.Y -= 0.2f;

                Main.NewText("yuppie");

                MountData.runSpeed = 10f;
            }
            else
            {
                MountData.runSpeed = 2f;
            }
        }

        // Since only a single instance of ModMountData ever exists, we can use player.mount._mountSpecificData to store additional data related to a specific mount.
        // Using something like this for gameplay effects would require ModPlayer syncing, but this example is purely visual.
        internal class SurfboardSpecificData
        {
            // count tracks how many balloons are still left. See ExamplePerson.Hurt to see how count decreases whenever damage is taken.
            internal int count;

            internal float[] rotations;

            internal static float[] offsets = new float[] { 0, 14, -14 };

            public SurfboardSpecificData()
            {
                count = 3;
            }
        }
    }
}