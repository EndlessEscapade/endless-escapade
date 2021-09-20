using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using EEMod.Extensions;
using EEMod.UI.States;

namespace EEMod.Items.Armor.Kelpweaver
{
    [AutoloadEquip(EquipType.Head)]
    public class KelpweaverHead : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpweaver Head");
            Tooltip.SetDefault("Creepy and crawly");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<KelpweaverBody>() && legs.type == ModContent.ItemType<KelpweaverLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.GetModPlayer<KelpweaverPlayer>().kelpweaverSet = true;
        }
    }

    public class KelpweaverPlayer : ModPlayer
    {
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            //kelpLayer.Draw
        }

        //public KelpweaverLayer kelpLayer;

        //public bool kelpweaverSet = false;
        public bool HasInteractedWithSlotBefore;

        public override void PostUpdate()
        {
            /*if (kelpweaverSet)
            {
                if (Main.playerInventory)
                {
                    if (!HasInteractedWithSlotBefore)
                    {
                        EEMod.UI.SetState("IndicatorsInterface", "IndicatorsUI");
                    }
                    EEMod.UI.SetState("KelpArmorAmmoInterface", "KelpArmorAmmoUI");
                }
                else
                {
                    if (!HasInteractedWithSlotBefore)
                    {
                        EEMod.UI.RemoveState("IndicatorsInterface");
                    }
                    EEMod.UI.RemoveState("KelpArmorAmmoInterface");
                }
            }*/
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["HasInteractedWithSlotBefore"] = HasInteractedWithSlotBefore
            };
        }

        public override void Load(TagCompound tag)
        {
            tag.TryGetRef("HasInteractedWithSlotBefore", ref HasInteractedWithSlotBefore);
        }
    }

    /*public class KelpweaverLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return default;
        }

        private static float Approach(ref float val, float desiredVal, float speed)
        {
            if (Math.Abs(val - desiredVal) < speed) return val;
            else return val < desiredVal ? val += speed : val -= speed;
        }

        public static float EnsurePositiveAngle(float val)
        {
            float val2 = ((val < 0f ? (val + 6.28f) : val));

            return (val2 > 6.28f ? (val2 - 6.28f) : val2);
        }

        public static float[] joint1Rots = new float[4];
        public static float[] joint2Rots = new float[4];

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player player = drawInfo.drawPlayer;

            KelpweaverPlayer modPlayer = player.GetModPlayer<KelpweaverPlayer>();

            //if (modPlayer.kelpweaverSet)
            {
                Texture2D arm = ModContent.Request<Texture2D>("EEMod/Items/Armor/Kelpweaver/KelpweaverArm").Value;
                Texture2D armGlow = ModContent.Request<Texture2D>("EEMod/Items/Armor/Kelpweaver/KelpweaverArmGlow").Value;

                NPC targetNPC = Main.npc[Helpers.ClosestNPCTo(player.Center)];

                Vector2 targetPos = targetNPC.Center;

                Vector2 myPos = player.Center;

                float angleOfFreedom = 0.785f;

                for (int i = 0; i < 4; i++)
                {
                    float coneMinAngle = EnsurePositiveAngle(i * 1.57f);
                    float coneMaxAngle = EnsurePositiveAngle((i + 1) * 1.57f);

                    float midConeAngle = EnsurePositiveAngle((coneMinAngle + coneMaxAngle) / 2f);

                    float dist = Vector2.Distance(targetPos, myPos);

                    //base arm angle caps at 

                    if (EnsurePositiveAngle((targetPos - myPos).ToRotation()) > coneMinAngle && EnsurePositiveAngle((targetPos - myPos).ToRotation()) < coneMaxAngle && dist < (16 * 32f))
                    {
                        midConeAngle = EnsurePositiveAngle((coneMinAngle + coneMaxAngle) / 2f);

                        float rot = EnsurePositiveAngle((targetPos - myPos).ToRotation());
                        float rotWithinCone = EnsurePositiveAngle(rot - midConeAngle);

                        if (EnsurePositiveAngle(midConeAngle - rot) < angleOfFreedom / 2f)
                        {
                            //If the enemy is within the original arm's angle of freedom

                            if (EnsurePositiveAngle(midConeAngle - rot) > 3.14f)
                            {
                                joint1Rots[i] = Approach(ref joint1Rots[i], midConeAngle - (angleOfFreedom / 2f), 0.075f);
                            }
                            else
                            {
                                joint1Rots[i] = Approach(ref joint1Rots[i], midConeAngle + (angleOfFreedom / 2f), 0.075f);
                            }
                        }
                        else
                        {
                            //If the enemy is outside of the original arm's angle of freedom

                            if (EnsurePositiveAngle(midConeAngle - rot) > 3.14f)
                            {
                                joint1Rots[i] = Approach(ref joint1Rots[i], midConeAngle + (angleOfFreedom / 2f), 0.075f);
                            }
                            else
                            {
                                joint1Rots[i] = Approach(ref joint1Rots[i], midConeAngle - (angleOfFreedom / 2f), 0.075f);
                            }
                        }

                        joint2Rots[i] = Approach(ref joint2Rots[i], rot, 0.075f);
                    }
                    else
                    {
                        //if not targeting something
                        midConeAngle = EnsurePositiveAngle((coneMinAngle + coneMaxAngle) / 2f);

                        float joint1factor = 0.3f;
                        float joint2factor = 2.1f;

                        if (i == 0)
                            joint1Rots[i] = Approach(ref joint1Rots[i], 1.57f - joint1factor, 0.075f);
                        else if (i == 1)
                            joint1Rots[i] = Approach(ref joint1Rots[i], 1.57f + joint1factor, 0.075f);
                        else if (i == 2)
                            joint1Rots[i] = Approach(ref joint1Rots[i], 4.71f - joint1factor, 0.075f);
                        else if (i == 3)
                            joint1Rots[i] = Approach(ref joint1Rots[i], 4.71f + joint1factor, 0.075f);

                        if (i == 0)
                            joint2Rots[i] = Approach(ref joint2Rots[i], joint1Rots[i] - joint2factor, 0.075f);
                        else if (i == 1)
                            joint2Rots[i] = Approach(ref joint2Rots[i], joint1Rots[i] + joint2factor, 0.075f);
                        else if (i == 2)
                            joint2Rots[i] = Approach(ref joint2Rots[i], joint1Rots[i] - joint2factor, 0.075f);
                        else if (i == 3)
                            joint2Rots[i] = Approach(ref joint2Rots[i], joint1Rots[i] + joint2factor, 0.075f);
                    }

                    Vector2 joint2Orig = myPos + (Vector2.UnitY.RotatedBy(joint1Rots[i] - 1.57f) * 40);

                    //Vector2 gunOrig = joint2Orig + (Vector2.UnitY.RotatedBy(joint1rot + joint2rot) * 46);

                    //Need to add support for spriteDirections(if the player flips, flip the arms)
                    //Also add support for targeting NPCs independently in each quadrant

                    Main.spriteBatch.Draw(arm, myPos.ForDraw(), arm.Bounds, Lighting.GetColor((int)(myPos.X / 16f), (int)(myPos.Y / 16f)), EnsurePositiveAngle(joint1Rots[i] + 1.57f), new Vector2(7, 46), 1f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(arm, joint2Orig.ForDraw(), arm.Bounds, Lighting.GetColor((int)(myPos.X / 16f), (int)(myPos.Y / 16f)), EnsurePositiveAngle(joint2Rots[i] + 1.57f), new Vector2(7, 46), 1f, SpriteEffects.None, 0);

                    Main.spriteBatch.Draw(armGlow, myPos.ForDraw(), arm.Bounds, Color.White, EnsurePositiveAngle(joint1Rots[i] + 1.57f), new Vector2(7, 46), 1f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(armGlow, joint2Orig.ForDraw(), arm.Bounds, Color.White, EnsurePositiveAngle(joint2Rots[i] + 1.57f), new Vector2(7, 46), 1f, SpriteEffects.None, 0);
                }
            }
        }
    }*/
}