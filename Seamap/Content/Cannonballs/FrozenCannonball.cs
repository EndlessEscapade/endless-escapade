using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.ID;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ID;
using EEMod.Seamap.Content;
using System.Diagnostics;
using EEMod.Extensions;
using ReLogic.Content;
using EEMod.Seamap.Core;

namespace EEMod.Seamap.Content.Cannonballs
{
    public class FrozenCannonball : Cannonball
    {
        public FrozenCannonball(Vector2 pos, Vector2 vel, TeamID team) : base(pos, vel, team)
        {
            width = 12;
            height = 12;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/Cannonballs/FrozenCannonball", AssetRequestMode.ImmediateLoad).Value;
        }

        public int ticks;
        public override void Update()
        {
            ticks++;

            foreach (SeamapObject obj in SeamapObjects.SeamapEntities)
            {
                if (obj == null) continue;

                if (obj.collides && obj.CheckCollision(Hitbox))
                {
                    SoundEngine.PlaySound(SoundID.Item14);
                    Explode();
                }
            }

            if (ticks == 100)
            {
                SoundEngine.PlaySound(SoundID.Item14);
                Explode();
            }
            
            rotation = velocity.ToRotation();

            base.Update();
        }

        public void Explode()
        {
            int count = Main.rand.Next(3, 6);

            for (int i = 0; i < count; i++)
            {
                SeamapObjects.NewSeamapObject(new FrozenCannonballFragment(Center, (oldVelocity * 1f) + (Vector2.UnitX.RotatedBy(MathHelper.TwoPi * (i / (float)count)) * 2f), team));
            }

            SeamapObjects.DestroyObject(this);
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            if (ticks > 95) scale += 0.1f;

            return true;
        }

        public override bool CustomDraw(SpriteBatch spriteBatch) => false;
    }

    public class FrozenCannonballFragment : Cannonball
    {
        public FrozenCannonballFragment(Vector2 pos, Vector2 vel, TeamID team) : base(pos, vel, team)
        {
            width = 10;
            height = 10;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/Cannonballs/FrozenCannonballFragment", AssetRequestMode.ImmediateLoad).Value;
        }

        public int ticks;
        public override void Update()
        {
            ticks++;

            if(ticks == 1)
            {
                frame = Main.rand.Next(3);
            }

            if (ticks > 5)
            {
                foreach (SeamapObject obj in SeamapObjects.SeamapEntities)
                {
                    if (obj == null) continue;

                    if (obj.collides && obj.CheckCollision(Hitbox))
                    {
                        SoundEngine.PlaySound(SoundID.Item14);
                        Explode();
                    }
                }
            }

            if (ticks == 40)
            {
                SoundEngine.PlaySound(SoundID.Item14);
                Explode();
            }

            rotation = velocity.ToRotation();

            base.Update();
        }

        public int frame;

        public void Explode()
        {
            SeamapObjects.DestroyObject(this);
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Seamap/Content/Cannonballs/FrozenCannonballFragment").Value, Center - Main.screenPosition, new Rectangle(10 * frame, 0, 10, 10), Color.White, rotation, new Vector2(5, 5), 1f, SpriteEffects.None, 0f);
            
            return false;
        }

        public override bool CustomDraw(SpriteBatch spriteBatch) => false;
    }
}
