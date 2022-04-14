using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Mounts
{
    public class Lobsterbike : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.spawnDust = DustID.Smoke;
            MountData.buff = ModContent.BuffType<LobsterbikeBuff>();
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
            MountData.playerHeadOffset = 22;

            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            MountData.textureWidth = 82;
            MountData.textureHeight = 52;
        }

        public override void UpdateEffects(Player player)
        {
            // This code spawns some dust if we are moving fast enough.
            if (Math.Abs(player.velocity.X) <= 4f)
            {
                return;
            }
            //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Vroom").WithVolume(.7f).WithPitchVariance(0));
            Rectangle rect = player.getRect();
            Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, DustID.Smoke);
            if (Math.Abs(player.velocity.Y) > 0.0001f && player.direction == 1)
            {
                player.fullRotation = MathHelper.Pi / -6;
            }
            else if (Math.Abs(player.velocity.Y) > 0.0001f && player.direction == -1)
            {
                player.fullRotation = MathHelper.Pi / 6;
            }
            else
            {
                player.fullRotation = 0;
            }
        }

        // Since only a single instance of ModMountData ever exists, we can use player.mount._mountSpecificData to store additional data related to a specific mount.
        // Using something like this for gameplay effects would require ModPlayer syncing, but this example is purely visual.
        internal class LobsterbikeSpecificData
        {
            // count tracks how many balloons are still left. See ExamplePerson.Hurt to see how count decreases whenever damage is taken.
            internal int count;

            internal float[] rotations;

            internal static float[] offsets = new float[] { 0, 14, -14 };

            public LobsterbikeSpecificData()
            {
                count = 3;
                rotations = new float[count];
            }
        }

        public override void SetMount(Player player, ref bool skipDust)
        {
            // When this mount is mounted, we initialize _mountSpecificData with a new CarSpecificData object which will track some extra visuals for the mount.
            player.mount._mountSpecificData = new LobsterbikeSpecificData();

            // This code bypasses the normal mount spawning dust and replaces it with our own visual.
            for (int i = 0; i < 16; i++)
            {
                Dust.NewDustPerfect(player.Center + new Vector2(80, 0).RotatedBy(i * Math.PI * 2 / 16f), MountData.spawnDust);
            }
            skipDust = true;
        }
    }
}