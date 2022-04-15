using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Summon;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.Audio;
using EEMod.Extensions;
using Terraria.ID;

namespace EEMod.Items.Weapons.Summon.Whips
{
    public class ArclashProj : ModProjectile
    {
        public override string Texture => "EEMod/Items/Weapons/Summon/Whips/ArclashProj";
        private Texture2D tex => ModContent.Request<Texture2D>(Texture).Value;

        private List<Vector2> _whipPointsForCollision = new List<Vector2>();

        private Player player;
        private EEPlayer modPlayer;

        private Rectangle handPos = new Rectangle(0, 0, 0, 0);

        private float rangeMultiplier;
        private float timeToFlyOut = 10;

        public int segments => 20;
        public float rangeMult => 1.2f;
        public int summonTagDamage => 0;
        public int summonTagCrit => 0;
        public int buffGivenToPlayer => -1;
        public int buffTime => 120;
        public  Color stringColor => Color.White;

        public override bool CloneNewInstances => true;

        //Whip behavior credit goes to https://github.com/515T3M/MSB (though I (crown) have heavily modified and adapted it)

        public virtual void SafeSetDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            // Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;

            SafeSetDefaults();
        }

        public override bool? CanCutTiles()
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            player.MinionAttackTargetNPC = target.whoAmI;

            if (buffGivenToPlayer != -1)
            {
                player.AddBuff(buffGivenToPlayer, buffTime);
            }

            NpcEffects(target, damage, knockback, crit);
        }

        public virtual void NpcEffects(NPC target, int damage, float knockback, bool crit)
        {

        }

        private bool hasPlayedSound;
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            modPlayer = player.GetModPlayer<EEPlayer>();

            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity *= rangeMult;
                Projectile.ai[0] = 1;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0] += 1f / player.GetAttackSpeed(DamageClass.Melee);

            GetWhipSettings(Projectile);

            Projectile.Center = GetPlayerArmPosition(Projectile) + Projectile.velocity * (Projectile.ai[0] - 1f);
            Projectile.spriteDirection = ((!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));

            if (Projectile.ai[0] >= timeToFlyOut || player.itemAnimation == 0)
            {
                Projectile.Kill();
                return;
            }

            player.heldProj = Projectile.whoAmI;
            player.itemAnimation = player.itemAnimationMax - (int)(Projectile.ai[0] / Projectile.MaxUpdates);
            player.itemTime = player.itemAnimation;

            if (Projectile.ai[0] >= (int)(timeToFlyOut / 2f) && !hasPlayedSound)
            {
                _whipPointsForCollision.Clear();
                FillWhipControlPoints(Projectile, _whipPointsForCollision);
                Vector2 position = _whipPointsForCollision[_whipPointsForCollision.Count - 1];

                SoundEngine.PlaySound(SoundID.Item153);

                hasPlayedSound = true;
            }

            float t2 = Projectile.ai[0] / timeToFlyOut;
            float num2 = Helpers.FloatLerp(0.1f, 0.7f, t2, true) * Helpers.FloatLerp(0.9f, 0.7f, t2, true);
            if (num2 > 0.1f && Main.rand.NextFloat() < num2 / 2f)
            {
                _whipPointsForCollision.Clear();
                FillWhipControlPoints(Projectile, _whipPointsForCollision);
                Rectangle r2 = Utils.CenteredRectangle(_whipPointsForCollision[_whipPointsForCollision.Count - 1], new Vector2(30f, 30f));
            }
        }

        private void FillWhipControlPoints(Projectile proj, List<Vector2> controlPoints)
        {
            GetWhipSettings(proj);

            float AnimationLevel = proj.ai[0] / timeToFlyOut;
            float Incrementation = 0.5f;
            float Step = 1f + Incrementation;
            float RotationMult = MathHelper.Pi * 10f * (1f - AnimationLevel * Step) * -proj.spriteDirection / segments;
            float AnimPos = AnimationLevel * Step;
            float level = 0f;

            if (AnimPos > 1f)
            {
                level = (AnimPos - 1f) / Incrementation;
                AnimPos = MathHelper.Lerp(1f, 0f, level);
            }

            float useTimeDiff = proj.ai[0] - 1f;
            useTimeDiff = Main.player[proj.owner].HeldItem.useAnimation * 2 * AnimationLevel;

            float SegmentDistMult = proj.velocity.Length() * useTimeDiff * AnimPos * rangeMultiplier / segments;
            float Scale = 1f;

            Vector2 playerArmPosition = GetPlayerArmPosition(proj);
            Vector2 ArmPos = playerArmPosition;

            float rotationDiff = 0f - MathHelper.PiOver2;

            Vector2 value = ArmPos;

            float Rotation = 0f + MathHelper.PiOver2 + MathHelper.PiOver2 * proj.spriteDirection;

            Vector2 value2 = ArmPos;

            float RotationPlusDiff = 0f + MathHelper.PiOver2;

            controlPoints.Add(playerArmPosition);

            for (int i = 0; i < segments; i++)
            {
                float SegmentOffset = i / (float)segments;
                float Inc = RotationMult * SegmentOffset * Scale;

                Vector2 startOff = ArmPos + rotationDiff.ToRotationVector2() * SegmentDistMult;
                Vector2 midOff = value2 + RotationPlusDiff.ToRotationVector2() * (SegmentDistMult * 2f);
                Vector2 endOff = value + Rotation.ToRotationVector2() * (SegmentDistMult * 2f);

                float AnimOffset = 1f - AnimPos;
                float EndVal = 1f - AnimOffset * AnimOffset;

                Vector2 value3 = Vector2.Lerp(midOff, startOff, EndVal * 0.9f + 0.1f);
                Vector2 value4 = Vector2.Lerp(endOff, value3, EndVal * 0.7f + 0.3f);
                Vector2 spinningpoint = playerArmPosition + (value4 - playerArmPosition) * new Vector2(1f, Step);

                float num17 = level;
                num17 *= num17;
                Vector2 item = spinningpoint.RotatedBy(proj.rotation + (MathHelper.PiOver2 * 3) * num17 * proj.spriteDirection, playerArmPosition);
                controlPoints.Add(item);

                rotationDiff += Inc;
                RotationPlusDiff += Inc;
                Rotation += Inc;
                ArmPos = startOff;
                value2 = midOff;
                value = endOff;
            }
        }

        private void GetWhipSettings(Projectile proj)
        {
            timeToFlyOut = Main.player[proj.owner].itemAnimationMax * proj.MaxUpdates;
            rangeMultiplier = 1;
        }

        private Vector2 GetPlayerArmPosition(Projectile proj)
        {
            Player targetSearchResults = Main.player[proj.owner];
            Vector2 value = Main.OffsetsPlayerOnhand[targetSearchResults.bodyFrame.Y / 56] * 2f;

            if (targetSearchResults.direction != 1)
                value.X = targetSearchResults.bodyFrame.Width - value.X;

            if (targetSearchResults.gravDir != 1f)
                value.Y = targetSearchResults.bodyFrame.Height - value.Y;

            value -= new Vector2(targetSearchResults.bodyFrame.Width - targetSearchResults.width, targetSearchResults.bodyFrame.Height - 42) / 2f;

            return targetSearchResults.RotatedRelativePoint(targetSearchResults.MountedCenter - new Vector2(20f, 42f) / 2f + value + Vector2.UnitY * targetSearchResults.gfxOffY);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> controlPoints = new List<Vector2>();
            FillWhipControlPoints(Projectile, controlPoints);

            Rectangle segmentRect = tex.Frame(1, 5);
            int height = segmentRect.Height;

            Vector2 segmentOrigin = segmentRect.Size() / 2f;
            Vector2 playerHandPosition = controlPoints[0];

            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                bool flag = true;

                if (i == 2)
                    segmentRect.Y = height;
                else if (i == segments - 8)
                    segmentRect.Y = height * 2;
                else if (i == segments - 4)
                    segmentRect.Y = height * 3;
                else if (i == segments - 2)
                    segmentRect.Y = height * 4;
                else if (i % 2 == 0)
                    flag = true;
                else
                    flag = false;

                Vector2 currentPoint = controlPoints[i];
                Vector2 vector4 = controlPoints[i + 1] - currentPoint;
                if (flag)
                {
                    float rotation = vector4.ToRotation() - MathHelper.PiOver2;
                    Point lightPos = currentPoint.ToTileCoordinates();

                    Color alpha = Projectile.GetAlpha(Lighting.GetColor(lightPos.X, lightPos.Y) * 1.2f);

                    Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                    if (i < segments - 2)
                    {
                        Vector2 vector5 = controlPoints[i + 2] - currentPoint;
                        for (float m = 0; m < 1; m += (1 / vector5.Length()) * 2f)
                        {
                            EEMod.LightningShader.Parameters["maskTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/WebAlt").Value);
                            EEMod.LightningShader.Parameters["newColor"].SetValue(new Vector4(Color.Gold.R, Color.Gold.G, Color.Gold.B, Color.Gold.A) / 255f);
                            EEMod.LightningShader.CurrentTechnique.Passes[0].Apply();

                            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Textures/PureStripFade").Value, Vector2.Lerp(currentPoint, controlPoints[i + 2], m).ForDraw() + new Vector2(Main.rand.NextFloat(-1f, 1f), 0).RotatedBy(rotation), new Rectangle(0, 0, 2, 6), Color.White, rotation + 1.57f, new Vector2(1f, 4f), 1f, SpriteEffects.None, 0f);
                        }
                    }

                    Main.spriteBatch.End(); Main.spriteBatch.Begin();

                    Main.spriteBatch.Draw(tex, playerHandPosition.ForDraw(), segmentRect, alpha, rotation, segmentOrigin, 1f, SpriteEffects.None, 0f);

                    handPos = new Rectangle((int)playerHandPosition.X, (int)playerHandPosition.Y, segmentRect.Width, segmentRect.Height);

                    //ItemCheck_MeleeHitNPCs(player.HeldItem, handPos, (int)(player.HeldItem.damage * player.GetDamage(DamageClass.Summon)), player.HeldItem.knockBack);
                }
                playerHandPosition += vector4;
            }

            return false;
        }

        private void ApplyNPCOnHitEffects(Item sItem, Rectangle itemRectangle, int damage, float knockBack, int npcIndex, int dmgRandomized, int dmgDone)
        {
            bool fontDeathText = !Main.npc[npcIndex].immortal;

            if (player.beetleOffense && fontDeathText)
            {
                player.beetleCounter += dmgDone;
                player.beetleCountdown = 0;
            }

            if (player.meleeEnchant == 7)
            {
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(Projectile), Main.npc[npcIndex].Center.X, Main.npc[npcIndex].Center.Y, Main.npc[npcIndex].velocity.X, Main.npc[npcIndex].velocity.Y, ProjectileID.ConfettiMelee, 0, 0f, player.whoAmI);
            }

            if (Main.npc[npcIndex].value > 0f && Main.rand.Next(5) == 0)
            {
                int type = 71;
                if (Main.rand.Next(10) == 0)
                {
                    type = 72;
                }
                if (Main.rand.Next(100) == 0)
                {
                    type = 73;
                }
                int Coin = Item.NewItem(new Terraria.DataStructures.EntitySource_Parent(Main.npc[npcIndex]), (int)Main.npc[npcIndex].position.X, (int)Main.npc[npcIndex].position.Y, Main.npc[npcIndex].width, Main.npc[npcIndex].height, type);

                Main.item[Coin].stack = Main.rand.Next(1, 11);
                Main.item[Coin].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                Main.item[Coin].velocity.X = Main.rand.Next(10, 31) * 0.2f * player.direction;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Coin);
                }
            }
        }

        private void ItemCheck_MeleeHitNPCs(Item sItem, Rectangle itemRectangle, int originalDamage, float knockBack)
        {
            for (int HitNPC = 0; HitNPC < 200; HitNPC++)
            {
                NPC hitNpc = Main.npc[HitNPC];
                if (!Main.npc[HitNPC].active || Main.npc[HitNPC].immune[player.whoAmI] != 0 || player.attackCD != 0)
                {
                    continue;
                }

                if (!Main.npc[HitNPC].dontTakeDamage && Main.npc[HitNPC].immune[player.whoAmI] == 0)
                {
                    if (!Main.npc[HitNPC].friendly || (Main.npc[HitNPC].type == NPCID.Guide && player.killGuide) || (Main.npc[HitNPC].type == NPCID.Clothier && player.killClothier))
                    {
                        Rectangle vector = new Rectangle((int)Main.npc[HitNPC].position.X, (int)Main.npc[HitNPC].position.Y, Main.npc[HitNPC].width, Main.npc[HitNPC].height);
                        if (itemRectangle.Intersects(vector) && (Main.npc[HitNPC].noTileCollide || player.CanHit(Main.npc[HitNPC])))
                        {
                            int num = Main.DamageVar(originalDamage);

                            int NPCtype = Item.NPCtoBanner(Main.npc[HitNPC].BannerID());
                            if (NPCtype > 0 && player.HasNPCBannerBuff(NPCtype))
                            {
                                num = ((!Main.expertMode) ? ((int)((float)num * ItemID.Sets.BannerStrength[Item.BannerToItem(NPCtype)].NormalDamageDealt)) : ((int)((float)num * ItemID.Sets.BannerStrength[Item.BannerToItem(NPCtype)].ExpertDamageDealt)));
                            }

                            int RealDamage = Main.DamageVar(num);

                            if (Main.npc[HitNPC].life > 5)
                            {
                                player.OnHit(Main.npc[HitNPC].Center.X, Main.npc[HitNPC].Center.Y, Main.npc[HitNPC]);
                            }

                            //if (player.GetArmorPenetration > 0)
                            //{
                            //    RealDamage += Main.npc[HitNPC].checkArmorPenetration(player.armorPenetration);
                            //}

                            int dmgDone = (int)Main.npc[HitNPC].StrikeNPC(RealDamage, knockBack, player.direction, false);

                            if (sItem.ModItem != null)
                            {
                                sItem.ModItem.OnHitNPC(player, Main.npc[HitNPC], RealDamage, knockBack, false);
                            }

                            OnHitNPC(Main.npc[HitNPC], RealDamage, knockBack, false);
                            ApplyNPCOnHitEffects(sItem, itemRectangle, num, knockBack, HitNPC, RealDamage, dmgDone);

                            int num5 = Item.NPCtoBanner(Main.npc[HitNPC].BannerID());
                            if (num5 >= 0)
                            {
                                player.lastCreatureHit = num5;
                            }

                            if (Main.netMode != NetmodeID.SinglePlayer)
                            {
                                NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, HitNPC, RealDamage, knockBack, player.direction);
                            }

                            if (player.accDreamCatcher)
                            {
                                player.addDPS(RealDamage);
                            }

                            Main.npc[HitNPC].immune[player.whoAmI] = player.itemAnimation;
                            player.attackCD = Math.Max(1, (int)((double)player.itemAnimationMax * 0.33));
                        }
                    }
                }
                else if (Main.npc[HitNPC].type == NPCID.BlueJellyfish || Main.npc[HitNPC].type == NPCID.PinkJellyfish || Main.npc[HitNPC].type == NPCID.GreenJellyfish || Main.npc[HitNPC].type == NPCID.BloodJelly)
                {
                    Rectangle value = new Rectangle((int)Main.npc[HitNPC].position.X, (int)Main.npc[HitNPC].position.Y, Main.npc[HitNPC].width, Main.npc[HitNPC].height);
                    if (itemRectangle.Intersects(value) && (Main.npc[HitNPC].noTileCollide || player.CanHit(Main.npc[HitNPC])))
                    {
                        player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(Main.npc[HitNPC].damage * 1.3), -player.direction);
                        Main.npc[HitNPC].immune[player.whoAmI] = player.itemAnimation;
                        player.attackCD = (int)(player.itemAnimationMax * 0.33);
                    }
                }
            }
        }
    }
}