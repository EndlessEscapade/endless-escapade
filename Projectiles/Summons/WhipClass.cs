using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Summons
{
    public abstract class WhipClass : ModProjectile
    {
        private List<Vector2> _whipPointsForCollision = new List<Vector2>();
        private Player player;
        public float rangeMult = 1f;
        public int summonTagDamage;
        public int summonTagCrit;
        public int buffGivenToPlayer;
        public int buffTime;

        private Rectangle handPos = new Rectangle(0, 0, 0, 0);

        private float rangeMultiplier;
        private float timeToFlyOut;
        private int segments;

        public override bool CloneNewInstances => true;

        public virtual void SafeSetDefaults()
        { }
        public override void SetDefaults()
        {

            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.ownerHitCheck = true;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            summonTagDamage = 0;
            summonTagCrit = 0;
            buffGivenToPlayer = -1;
            buffTime = 120;
            
            
            SafeSetDefaults();
            //projectile.MaxUpdates *= rangeMult;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            player.MinionAttackTargetNPC = target.whoAmI;

            if (buffGivenToPlayer != -1)
                player.AddBuff(buffGivenToPlayer, buffTime);

            //player.GetModPlayer<EEPlayer>().summonTagDamage = summonTagDamage;
            //player.GetModPlayer<EEPlayer>().summonTagCrit = summonTagCrit;
        }

        public override void AI()
        {
            if(projectile.ai[0] == 0)
            {
                projectile.velocity *= rangeMult;
                projectile.ai[0] = 1;
            }

            player = Main.player[projectile.owner];
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            projectile.ai[0] += 1f / player.meleeSpeed;
            GetWhipSettings(projectile);
            projectile.Center = GetPlayerArmPosition(projectile) + projectile.velocity * (projectile.ai[0] - 1f);
            projectile.spriteDirection = ((!(Vector2.Dot(projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));

            if (projectile.ai[0] >= timeToFlyOut || player.itemAnimation == 0)
            {
                projectile.Kill();
                return;
            }

            player.heldProj = projectile.whoAmI;
            player.itemAnimation = player.itemAnimationMax - (int)(projectile.ai[0] / (float)projectile.MaxUpdates);
            player.itemTime = player.itemAnimation;

            if (projectile.ai[0] == (float)(int)(timeToFlyOut / 2f))
            {
                _whipPointsForCollision.Clear();
                FillWhipControlPoints(projectile, _whipPointsForCollision);
                Vector2 position = _whipPointsForCollision[_whipPointsForCollision.Count - 1];
                Main.PlaySound(SoundID.Item1, position);
            }

            float t2 = projectile.ai[0] / timeToFlyOut;
            float num2 = GetLerpValue(0.1f, 0.7f, t2, clamped: true) * GetLerpValue(0.9f, 0.7f, t2, clamped: true);
            if (num2 > 0.1f && Main.rand.NextFloat() < num2 / 2f)
            {
                _whipPointsForCollision.Clear();
                FillWhipControlPoints(projectile, _whipPointsForCollision);
                Rectangle r2 = Utils.CenteredRectangle(_whipPointsForCollision[_whipPointsForCollision.Count - 1], new Vector2(30f, 30f));

            }
        }

        public void FillWhipControlPoints(Projectile proj, List<Vector2> controlPoints)
        {
            GetWhipSettings(proj);
            float AnimationLevel = proj.ai[0] / timeToFlyOut;
            float Incrementation = 0.5f;
            float Step = 1f + Incrementation;
            float RotationMult = (float)Math.PI * 10f * (1f - AnimationLevel * Step) * (float)(-proj.spriteDirection) / (float)segments;
            float AnimPos = AnimationLevel * Step;
            float level = 0f;
            if (AnimPos > 1f)
            {
                level = (AnimPos - 1f) / Incrementation;
                AnimPos = MathHelper.Lerp(1f, 0f, level);
            }
            float useTimeDiff = proj.ai[0] - 1f;
            useTimeDiff = (float)(Main.player[proj.owner].HeldItem.useAnimation * 2) * AnimationLevel;
            float SegmentDistMult = proj.velocity.Length() * useTimeDiff * AnimPos * rangeMultiplier / (float)segments;
            float Scale = 1f;
            Vector2 playerArmPosition = GetPlayerArmPosition(proj);
            Vector2 ArmPos = playerArmPosition;
            float rotationDiff = 0f - (float)Math.PI / 2f;
            Vector2 value = ArmPos;
            float Rotation = 0f + (float)Math.PI / 2f + (float)Math.PI / 2f * (float)proj.spriteDirection;
            Vector2 value2 = ArmPos;
            float RoationPlusDiff = 0f + (float)Math.PI / 2f;
            controlPoints.Add(playerArmPosition);
            for (int i = 0; i < segments; i++)
            {
                float SegmentOffset = (float)i / (float)segments;
                float Inc = RotationMult * SegmentOffset * Scale;
                Vector2 startOff = ArmPos + rotationDiff.ToRotationVector2() * SegmentDistMult;
                Vector2 midOff = value2 + RoationPlusDiff.ToRotationVector2() * (SegmentDistMult * 2f);
                Vector2 endOff = value + Rotation.ToRotationVector2() * (SegmentDistMult * 2f);
                float AnimOffset = 1f - AnimPos;
                float EndVal = 1f - AnimOffset * AnimOffset;
                Vector2 value3 = Vector2.Lerp(midOff, startOff, EndVal * 0.9f + 0.1f);
                Vector2 value4 = Vector2.Lerp(endOff, value3, EndVal * 0.7f + 0.3f);
                Vector2 spinningpoint = playerArmPosition + (value4 - playerArmPosition) * new Vector2(1f, Step);
                float num17 = level;
                num17 *= num17;
                Vector2 item = spinningpoint.RotatedBy(proj.rotation + 4.712389f * num17 * (float)proj.spriteDirection, playerArmPosition);
                controlPoints.Add(item);
                rotationDiff += Inc;
                RoationPlusDiff += Inc;
                Rotation += Inc;
                ArmPos = startOff;
                value2 = midOff;
                value = endOff;
            }
        }

        public void GetWhipSettings(Projectile proj)
        {
            timeToFlyOut = Main.player[proj.owner].itemAnimationMax * proj.MaxUpdates;
            segments = 20;
            rangeMultiplier = 1;
        }

        public float GetLerpValue(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                    {
                        return 0f;
                    }
                    if (t > to)
                    {
                        return 1f;
                    }
                }
                else
                {
                    if (t < to)
                    {
                        return 1f;
                    }
                    if (t > from)
                    {
                        return 0f;
                    }
                }
            }
            return (t - from) / (to - from);
        }

        public Vector2 GetPlayerArmPosition(Projectile proj)
        {
            Player targetSearchResults = Main.player[proj.owner];
            Vector2 value = Main.OffsetsPlayerOnhand[targetSearchResults.bodyFrame.Y / 56] * 2f;
            if (targetSearchResults.direction != 1)
            {
                value.X = (float)targetSearchResults.bodyFrame.Width - value.X;
            }
            if (targetSearchResults.gravDir != 1f)
            {
                value.Y = (float)targetSearchResults.bodyFrame.Height - value.Y;
            }
            value -= new Vector2(targetSearchResults.bodyFrame.Width - targetSearchResults.width, targetSearchResults.bodyFrame.Height - 42) / 2f;
            return targetSearchResults.RotatedRelativePoint(targetSearchResults.MountedCenter - new Vector2(20f, 42f) / 2f + value + Vector2.UnitY * targetSearchResults.gfxOffY);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            List<Vector2> list = new List<Vector2>();
            FillWhipControlPoints(projectile, list);
            Texture2D value = mod.GetTexture("Projectiles/Whips/WhipTexture");
            Microsoft.Xna.Framework.Rectangle value2 = value.Frame();
            Vector2 origin = new Vector2(value2.Width / 2, 2f);
            Microsoft.Xna.Framework.Color originalColor = Microsoft.Xna.Framework.Color.White;

            Vector2 value3 = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 vector = list[i];
                Vector2 vector2 = list[i + 1] - vector;
                float rotation = vector2.ToRotation() - (float)Math.PI / 2f;
                Microsoft.Xna.Framework.Color color = GetColor(vector.ToTileCoordinates(), originalColor);
                Vector2 scale = new Vector2(1f, (vector2.Length() + 2f) / (float)value2.Height);
                spriteBatch.Draw(value, value3 - Main.screenPosition, value2, color, rotation, origin, scale, SpriteEffects.None, 0f);
                value3 += vector2;
            }

            DrawWhip(projectile, list, spriteBatch);

            return false;
        }

        private Vector2 DrawWhip(Projectile proj, List<Vector2> controlPoints, SpriteBatch spriteBatch)
        {
            Texture2D value = ModContent.GetTexture(Texture);
            Rectangle rectangle = value.Frame(1, 5);
            int height = rectangle.Height;
            rectangle.Height -= 2;
            Vector2 vector = rectangle.Size() / 2f;
            Vector2 vector2 = controlPoints[0];
            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                bool flag = true;
                Vector2 origin = vector;
                switch (i)
                {
                    case 0:
                        origin.Y -= 4f;
                        break;
                    case 3:
                        rectangle.Y = height;
                        break;
                    case 5:
                    case 7:
                        
                    case 9:
                    case 11:
                    case 13:
                        rectangle.Y = height * 2;
                        break;
                    case 15:
                    case 17:
                        rectangle.Y = height * 3;
                        break;
                    case 19:
                        rectangle.Y = height * 4;
                        break;
                    default:
                        flag = false;
                        break;
                }
                Vector2 vector3 = controlPoints[i];
                Vector2 vector4 = controlPoints[i + 1] - vector3;
                if (flag)
                {
                    float rotation = vector4.ToRotation() - (float)Math.PI / 2f;
                    Color alpha = proj.GetAlpha(GetColor(vector3.ToTileCoordinates(), Color.White));
                    spriteBatch.Draw(value, vector2 - Main.screenPosition, rectangle, alpha, rotation, origin, 1f, SpriteEffects.None, 0f);
                    handPos = new Rectangle((int)vector2.X, (int)vector2.Y, rectangle.Width, rectangle.Height);
                    ItemCheck_MeleeHitNPCs(player.HeldItem, handPos, (int)(player.HeldItem.damage * player.minionDamage), player.HeldItem.knockBack);
                    
                }
                vector2 += vector4;
            }
            return vector2;


        }

        public Color GetColor(Point tileCoords, Color originalColor)
        {
            if (Main.gameMenu)
            {
                return originalColor;
            }
            return new Color(Lighting.GetColor(tileCoords.X, tileCoords.Y).ToVector3() * originalColor.ToVector3());
        }

        public void ApplyNPCOnHitEffects(Item sItem, Rectangle itemRectangle, int damage, float knockBack, int npcIndex, int dmgRandomized, int dmgDone)
        {
            bool fontDeathText = !Main.npc[npcIndex].immortal;

            if (player.beetleOffense && fontDeathText)
            {
                player.beetleCounter += dmgDone;
                player.beetleCountdown = 0;
            }

            if (player.meleeEnchant == 7)
            {
                Projectile.NewProjectile(Main.npc[npcIndex].Center.X, Main.npc[npcIndex].Center.Y, Main.npc[npcIndex].velocity.X, Main.npc[npcIndex].velocity.Y, ProjectileID.ConfettiMelee, 0, 0f, player.whoAmI);
            }


            if (Main.npc[npcIndex].value > 0f && player.coins && Main.rand.Next(5) == 0)
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
                int Coin = Item.NewItem((int)Main.npc[npcIndex].position.X, (int)Main.npc[npcIndex].position.Y, Main.npc[npcIndex].width, Main.npc[npcIndex].height, type);
                Main.item[Coin].stack = Main.rand.Next(1, 11);
                Main.item[Coin].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                Main.item[Coin].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)player.direction;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Coin);
                }
            }
        }

        public void ItemCheck_MeleeHitNPCs(Item sItem, Rectangle itemRectangle, int originalDamage, float knockBack)
        {
            for (int HitNPC = 0; HitNPC < 200; HitNPC++)
            {
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
                            int num = Main.DamageVar(originalDamage); ;

                            int NPCtype = Item.NPCtoBanner(Main.npc[HitNPC].BannerID());
                            if (NPCtype > 0 && player.NPCBannerBuff[NPCtype] == true)
                            {
                                num = ((!Main.expertMode) ? ((int)((float)num * ItemID.Sets.BannerStrength[Item.BannerToItem(NPCtype)].NormalDamageDealt)) : ((int)((float)num * ItemID.Sets.BannerStrength[Item.BannerToItem(NPCtype)].ExpertDamageDealt)));
                            }



                            int RealDamage = Main.DamageVar(num);

                            if (Main.npc[HitNPC].life > 5)
                            {
                                player.OnHit(Main.npc[HitNPC].Center.X, Main.npc[HitNPC].Center.Y, Main.npc[HitNPC]);
                            }
                            if (player.armorPenetration > 0)
                            {
                                RealDamage += Main.npc[HitNPC].checkArmorPenetration(player.armorPenetration);
                            }
                            int dmgDone = (int)Main.npc[HitNPC].StrikeNPC(RealDamage, knockBack, player.direction, false);
                            if (sItem.modItem != null)
                            {



                                sItem.modItem.OnHitNPC(player, Main.npc[HitNPC], RealDamage, knockBack, false);
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
                        player.Hurt(PlayerDeathReason.LegacyDefault(), (int)((double)Main.npc[HitNPC].damage * 1.3), -player.direction);
                        Main.npc[HitNPC].immune[player.whoAmI] = player.itemAnimation;
                        player.attackCD = (int)((double)player.itemAnimationMax * 0.33);
                    }
                }


            }
        }
        
    }
}
