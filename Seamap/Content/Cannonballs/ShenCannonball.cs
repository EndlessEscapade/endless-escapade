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
using EEMod.Prim;
using EEMod.Seamap.Core;

namespace EEMod.Seamap.Content.Cannonballs
{
    public class ShenCannonball : SeamapObject
    {
        public ShenCannonball(Vector2 pos, Vector2 vel, Color _color) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 12;
            height = 12;

            cannonballColor = _color;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShenCannonball", AssetRequestMode.ImmediateLoad).Value;
        }

        public int ticks;

        public ShenChildTrail shenTrail1;
        public ShenChildTrail shenTrail2;
        public ShenChildTrail shenTrail3;

        public Color cannonballColor;

        public override void Update()
        {
            if (ticks == 0 || shenTrail1 == null)
            {
                shenTrail1 = new ShenChildTrail(Center + (Vector2.UnitY * 4), Vector2.Zero);
                shenTrail2 = new ShenChildTrail(Center + (Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 3f) * 4), Vector2.Zero);
                shenTrail3 = new ShenChildTrail(Center + (Vector2.UnitY.RotatedBy(MathHelper.TwoPi * 2f / 3f) * 4), Vector2.Zero);

                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(shenTrail1, cannonballColor * 0.75f, 6, 15));
                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(shenTrail2, cannonballColor * 0.75f, 6, 15));
                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(shenTrail3, cannonballColor * 0.75f, 6, 15));

                SeamapObjects.NewSeamapObject(shenTrail1);
                SeamapObjects.NewSeamapObject(shenTrail2);
                SeamapObjects.NewSeamapObject(shenTrail3);
            }

            shenTrail1.Center = Center + velocity + (Vector2.UnitY.RotatedBy((ticks / 10f)) * 4);
            shenTrail2.Center = Center + velocity + (Vector2.UnitY.RotatedBy((ticks / 10f) + (MathHelper.TwoPi / 3f)) * 4);
            shenTrail3.Center = Center + velocity + (Vector2.UnitY.RotatedBy((ticks / 10f) + (MathHelper.TwoPi * 2f / 3f)) * 4);

            ticks++;

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.5f));
            EEMod.MainParticles.SpawnParticles(Center, (velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 0.85f),
                ModContent.Request<Texture2D>("EEMod/Empty").Value, 60, 1f, Color.Lerp(cannonballColor, Color.White, ticks / 180f) * 0.5f,
                new SlowDown(0.98f),
                new RotateTexture(0.3f),
                new SetMask(ModContent.Request<Texture2D>("EEMod/Textures/6PointStar").Value, cannonballColor * 0.5f),
                new SetMask(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, cannonballColor * 0.75f, 2f));

            if (explodeFrame <= 0)
            {
                foreach (SeamapObject obj in SeamapObjects.SeamapEntities)
                {
                    if (obj == null) continue;

                    if (obj.collides && obj.Hitbox.Intersects(Hitbox))
                    {
                        SoundEngine.PlaySound(SoundID.Item14);
                        explodeFrame++;
                    }
                }

                if (ticks == 180)
                {
                    SoundEngine.PlaySound(SoundID.Item14);
                    explodeFrame++;
                }
            }
            else
            {
                shenTrail1.Kill();
                shenTrail2.Kill();
                shenTrail3.Kill();

                for (int i = 0; i < 40; i++)
                {
                    EEMod.MainParticles.SpawnParticles(Center, (oldVelocity * 0.5f) + (Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-3.14f, 3.14f)) * Main.rand.NextFloat(0.65f, 0.85f) * 3f * 0.8f),
                        ModContent.Request<Texture2D>("EEMod/Empty").Value, 30, 1.5f, Color.Lerp(cannonballColor, Color.White, 0.25f) * 0.75f,
                        new SlowDown(0.99f),
                        new RotateTexture(0.02f),
                        new SetMask(ModContent.Request<Texture2D>("EEMod/Textures/6PointStar").Value, cannonballColor * 0.75f),
                        new SetMask(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, cannonballColor * 0.75f, 2f));
                }

                Kill();
            }

            if (explodeFrame >= 1 && ticks % 4 == 0) explodeFrame++;

            rotation = Main.GameUpdateCount / 10f;

            base.Update();
        }

        public float sinkLevel;

        public int explodeFrame;

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            if (explodeFrame >= 1)
            {
                velocity = Vector2.Zero;

                //Texture2D explodeSheet = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/CannonballExplode").Value;
                //Texture2D explodeSheetGlow = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/CannonballExplodeGlow").Value;

                //Main.spriteBatch.Draw(explodeSheet, Center.ForDraw() + new Vector2(-32, -36), new Rectangle(0, explodeFrame * 60, 60, 60), Color.White.LightSeamap(), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                //Main.spriteBatch.Draw(explodeSheet, Center.ForDraw() + new Vector2(-32, -36), new Rectangle(0, explodeFrame * 60, 60, 60), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                if (explodeFrame - 1 >= 6)
                {
                    Kill();
                }

                return false;
            }
            else
            {
                //corona
                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, Center - Main.screenPosition - (velocity), cannonballColor * 0.6f, 0.5f, rotation);

                //outline
                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/SmoothFadeOut").Value, Center - Main.screenPosition, cannonballColor * 0.75f, 0.6f, rotation);
                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/SmoothFadeOut").Value, Center - Main.screenPosition, Color.White, 0.4f, rotation);

                return true;
            }
        }
    }

    public class ShenChildTrail : SeamapObject
    {
        public ShenChildTrail(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 12;
            height = 12;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShenCannonball", AssetRequestMode.ImmediateLoad).Value;
        }

        public override bool PreDraw(SpriteBatch spriteBatch) => false;

        public override bool CustomDraw(SpriteBatch spriteBatch) => true;
    }
}
