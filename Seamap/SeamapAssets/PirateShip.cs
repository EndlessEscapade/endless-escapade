using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class PirateShip : SeamapObject
    {
        public override void OnSpawn()
        {
            texture = ModContent.GetTexture("EEMod/Seamap/SeamapAssets/PirateShip");

            width = 44;
            height = 52;
        }

        public PirateShip(Vector2 pos, Vector2 vel) : base(pos, vel) { }

        public override void Update()
        {
            Vector2 moveTo = SeamapPlayerShip.localship.Center;

            float speed = .002f;

            Vector2 move = moveTo - Center;

            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            float turnResistance = 10f;
            move = (velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            velocity = move;

            rotation = velocity.X * 100;

            spriteDirection = velocity.X < 0 ? 1 : -1;

            position += velocity;

            alpha = 0.5f;
        }
    }
}
