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
using EEMod.Seamap.SeamapAssets;
using System.Diagnostics;
using EEMod.Extensions;
using ReLogic.Content;
using EEMod.Prim;

namespace EEMod.Seamap.SeamapContent
{
    public class ShenCannonball : SeamapObject
    {
        public ShenCannonball(Vector2 pos, Vector2 vel, Color _color) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 12;
            height = 12;

            color = _color;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShenCannonball", AssetRequestMode.ImmediateLoad).Value;
        }

        public int ticks;

        public ShenChildTrail shenTrail1;
        public ShenChildTrail shenTrail2;
        public ShenChildTrail shenTrail3;

        public Color color;

        public override void Update()
        {
            if (ticks == 0 || shenTrail1 == null)
            {
                shenTrail1 = new ShenChildTrail(Center + (Vector2.UnitY * 4), Vector2.Zero);
                shenTrail2 = new ShenChildTrail(Center + (Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 3f) * 4), Vector2.Zero);
                shenTrail3 = new ShenChildTrail(Center + (Vector2.UnitY.RotatedBy(MathHelper.TwoPi * 2f / 3f) * 4), Vector2.Zero);

                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(shenTrail1, color * 0.75f, 6, 15));
                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(shenTrail2, color * 0.75f, 6, 15));
                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(shenTrail3, color * 0.75f, 6, 15));

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
                ModContent.Request<Texture2D>("EEMod/Empty").Value, 60, 2f, Color.Lerp(color, Color.White, ticks / 180f) * 0.5f,
                new SlowDown(0.98f),
                new RotateTexture(0.02f),
                new SetMask(EEMod.Instance.Assets.Request<Texture2D>("Textures/6PointStar").Value, 1f),
                new AfterImageTrail(0.9f),
                new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)));

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

                Texture2D explodeSheet = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/CannonballExplode").Value;
                Texture2D explodeSheetGlow = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/CannonballExplodeGlow").Value;

                Main.spriteBatch.Draw(explodeSheet, Center.ForDraw() + new Vector2(-32, -36), new Rectangle(0, explodeFrame * 60, 60, 60), Color.White.LightSeamap(), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                Main.spriteBatch.Draw(explodeSheet, Center.ForDraw() + new Vector2(-32, -36), new Rectangle(0, explodeFrame * 60, 60, 60), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                if (explodeFrame - 1 >= 6)
                {
                    Kill();
                }

                return false;
            }
            else
            {
                //corona
                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, Center - Main.screenPosition - (velocity), color * 0.6f, 0.5f, rotation);

                //outline
                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/SmoothFadeOut").Value, Center - Main.screenPosition, color * 0.75f, 0.6f, rotation);
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
