using EEMod.Extensions;
using EEMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Shivs
{
    public abstract class Rapier : EEProjectile
    {
        int stealyourmom = 0;
        bool isClicking;
        public float timeForSwing;
        Vector2 firstClickPos = new Vector2();
        Vector2 midPoint = new Vector2();
        Vector2 lastClickPos = new Vector2();
        List<Vector2> positionBuffer = new List<Vector2>();
        protected Player projOwner => Main.player[Projectile.owner];

        public virtual List<int> exclude => new List<int> { };
        public float xDis;

        public override void AI()
        {
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = projOwner.Center.X - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - Projectile.height / 2;

            if (projOwner.itemAnimation <= 1 && timeForSwing > 0.999f)
            {
                Main.NewText("f");
                Projectile.Kill();
            }
            if (Main.LocalPlayer.controlUseItem)
            {
                flag = false;
                timeForSwing = 0;
                if (Main.LocalPlayer.controlUseItem)
                {
                    positionBuffer.Add(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y));
                    stealyourmom++;
                    if (stealyourmom == 5)
                    {
                        midPoint = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                    }
                }
                if (Main.LocalPlayer.controlUseItem && !isClicking)
                {


                    firstClickPos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                    isClicking = true;
                }
                else
                {
                    midPoint = positionBuffer[positionBuffer.Count / 2];
                    lastClickPos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                }
            }
            else
            {
                positionBuffer.Clear();
                stealyourmom = 0;
                if (timeForSwing < 1f)
                    timeForSwing += (1 - timeForSwing) / 16f;
                isClicking = false;

                Vector2 mClickToDraw = midPoint.ForDraw();
                Vector2 fClickToDraw = firstClickPos.ForDraw();
                Vector2 lClickToDraw = lastClickPos.ForDraw();

                float x = Helpers.X(timeForSwing, fClickToDraw.X, mClickToDraw.X, mClickToDraw.X, lClickToDraw.X);
                float y = Helpers.Y(timeForSwing, fClickToDraw.Y, mClickToDraw.Y, mClickToDraw.Y, lClickToDraw.Y);
                float distX = x - ppostodraw.X;
                float distY = y - ppostodraw.Y;

                rotation = new Vector2(distX, distY).ToRotation();
            }
            //Vector2 Norm = Vector2.Normalize(Main.MouseWorld - projOwner.Center); //unused
        }
        bool flag;
        float rotations;
        float rotation;
        float projLerp;
        Vector2 ppostodraw => projOwner.Center.ForDraw();
        public override bool PreDraw(ref Color lightColor)
        {
            float timeForSwingSin = (float)Math.Sin(timeForSwing * 3.14f) + 0.2f;


            if (!Main.LocalPlayer.controlUseItem)
            {
                int lightningproj = 0;
                projLerp = 0;
                if (!flag)
                {
                    lightningproj = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), firstClickPos,Vector2.Zero, ModContent.ProjectileType<RapierProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    (Main.projectile[lightningproj].ModProjectile as RapierProj).mid = midPoint;
                    (Main.projectile[lightningproj].ModProjectile as RapierProj).start = firstClickPos;
                    (Main.projectile[lightningproj].ModProjectile as RapierProj).end = lastClickPos;
                    flag = true;
                    EEMod.primitives.CreateTrail(new Prim.SwordPrimTrail(Projectile, lastClickPos, midPoint, firstClickPos));
                }
                //Helpers.DrawLine(fClickToDraw, lClickToDraw);
                //Helpers.DrawLine(fClickToDraw, mClickToDraw);
                //Helpers.DrawLine(mClickToDraw, lClickToDraw);
                //Helpers.DrawBezier(Terraria.GameContent.TextureAssets.MagicPixel.Value, Color.White* timeForSwingSin, lastClickPos,firstClickPos, midPoint, 1f, new Rectangle(0, 0, 1, 1));
                //Helpers.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, fClickToDraw, Color.White, 1f, new Rectangle(0,0,3,3));
                //Helpers.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, mClickToDraw, Color.White, 1f, new Rectangle(0, 0, 3, 3));
                //Helpers.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, lClickToDraw, Color.White, 1f, new Rectangle(0, 0, 3, 3));
            }
            rotations += (rotation - rotations) / 12f;
            float length = 30;
            Helpers.DrawLine(ppostodraw, ppostodraw + new Vector2(1, 0).RotatedBy(rotations) * length);
            return false;
        }
    }
}