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
            player.GetModPlayer<KelpweaverPlayer>().kelpweaverSet = true;
        }
    }

    public class KelpweaverPlayer : ModPlayer
    {
        public bool kelpweaverSet = false;
        public bool HasInteractedWithSlotBefore;

        private static float gun1rot = 0.79f;
        private static float gun2rot = 2.36f;
        private static float gun3rot = 3.92f;
        private static float gun4rot = 5.49f;

        private static Vector2 currentGunPos = Vector2.Zero;

        private static float Approach(ref float val, float desiredVal, float speed)
        {
            if (Math.Abs(val - desiredVal) < speed) return val;
            else return val < desiredVal ? val += speed : val -= speed;
        }

        public static readonly PlayerLayer KelpweaverArms = new PlayerLayer("EEMod", "MiscEffectsBack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo) 
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player player = drawInfo.drawPlayer;

            KelpweaverPlayer modPlayer = player.GetModPlayer<KelpweaverPlayer>();

            if (modPlayer.kelpweaverSet)
            {
                NPC targetNPC = Main.npc[Helpers.ClosestNPCTo(player.Center)];

                bool gun1 = false;
                bool gun2 = false;
                bool gun3 = false;
                bool gun4 = false;

                float gunRot = Vector2.Normalize(targetNPC.Center - currentGunPos).ToRotation() + 3.142f;
                float playerRot = Vector2.Normalize(targetNPC.Center - player.Center).ToRotation() + 3.142f;

                if (Vector2.Distance(targetNPC.Center, player.Center) <= 640)
                {
                    if (playerRot >= 0 && playerRot < 1.57f)
                    {
                        gun4 = true;

                        if (gun4rot < gunRot && gun4rot < 1.1f)
                        {
                            gun4rot += 0.02f;
                        }

                        if (gun4rot > gunRot && gun4rot > 0.48f)
                        {
                            gun4rot -= 0.02f;
                        }
                    }

                    if (playerRot >= 1.57f && playerRot < 3.14f)
                    {
                        gun1 = true;

                        if (gun1rot < gunRot && gun1rot < 2.67f)
                        {
                            gun1rot += 0.02f;
                        }

                        if (gun1rot > gunRot && gun1rot > 2.05f)
                        {
                            gun1rot -= 0.02f;
                        }
                    }

                    if (playerRot >= 3.14f && playerRot < 4.71f)
                    {
                        gun2 = true;

                        if (gun2rot < gunRot && gun2rot < 4.24f)
                        {
                            gun2rot += 0.02f;
                        }

                        if (gun2rot > gunRot && gun2rot > 3.62f)
                        {
                            gun2rot -= 0.02f;
                        }
                    }

                    if (playerRot >= 4.71f && playerRot < 6.29f)
                    {
                        gun3 = true;

                        if (gun3rot < gunRot && gun3rot < 5.81f)
                        {
                            gun3rot += 0.02f;
                        }

                        if (gun3rot > gunRot && gun3rot > 5.19f)
                        {
                            gun3rot -= 0.02f;
                        }
                    }
                }

                Main.NewText(gunRot);


                if (!gun1)
                {
                    Approach(ref gun1rot, 0.79f, 0.02f);
                }

                if(!gun2)
                {
                    Approach(ref gun2rot, 2.36f, 0.02f);
                }

                if(!gun3)
                {
                    Approach(ref gun3rot, 3.92f, 0.02f);
                }

                if (!gun4)
                {
                    Approach(ref gun4rot, 5.49f, 0.02f);
                }

                //restrict between 0.4 and 0.6, multiply by 1.57


                DrawNewLeg(player.Center, player, gun1rot, gun1, 0);

                DrawNewLeg(player.Center, player, gun2rot, gun2, 1);

                DrawNewLeg(player.Center, player, gun3rot, gun3, 2);

                DrawNewLeg(player.Center, player, gun4rot, gun4, 3);
            }
        });
        
        private static void DrawNewLeg(Vector2 position, Player player, float rot, bool canShoot, int quadrant)
        {
            Texture2D arm = ModContent.GetTexture("EEMod/Items/Armor/Kelpweaver/KelpweaverArm");
            Texture2D glow = ModContent.GetTexture("EEMod/Items/Armor/Kelpweaver/KelpweaverArmGlow");
            Texture2D pistol = ModContent.GetTexture("EEMod/Items/Armor/Kelpweaver/KelpweaverPistol");

            Color lightColor = Lighting.GetColor((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f));
            SpriteEffects dir = rot > 3.14f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            DrawData data = new DrawData(arm, position.ForDraw(), arm.Bounds, lightColor, rot, new Vector2(7, 46), 1f, dir, 0);

            DrawData data2 = new DrawData(glow, position.ForDraw(), glow.Bounds, Color.White, rot, new Vector2(7, 46), 1f, dir, 0);


            Vector2 newPosition = position - new Vector2(0, 40).RotatedBy(rot);
            float newRot = rot + ((rot - (1.57f * quadrant) - 0.79f) * 4);
            SpriteEffects newDir = newRot > 3.14f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            DrawData data3 = new DrawData(arm, newPosition.ForDraw(), arm.Bounds, lightColor, newRot, new Vector2(7, 46), 1f, dir, 0);

            DrawData data4 = new DrawData(glow, newPosition.ForDraw(), glow.Bounds, Color.White, newRot, new Vector2(7, 46), 1f, dir, 0);


            Vector2 pistolPos = newPosition - new Vector2(0, 40).RotatedBy(newRot);
            float pistolRot = newRot;

            SpriteEffects pistolDir = (pistolPos.X > player.Center.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            DrawData data5 = new DrawData(pistol, pistolPos.ForDraw(), pistol.Bounds, lightColor, pistolRot, new Vector2(12 * (pistolPos.X > player.Center.X ? 1 : -1), 8) + pistol.Bounds.Size() / 2f, 1f, pistolDir, 0);

            if (canShoot)
            {
                float desiredRot = (pistolPos + new Vector2(16 * (pistolPos.X > player.Center.X ? -1 : 1), -30).RotatedBy(pistolRot)).ToRotation();

                currentGunPos = pistolPos + new Vector2(16 * (pistolPos.X > player.Center.X ? -1 : 1), -30).RotatedBy(pistolRot);

                if (Main.GameUpdateCount % 30 == 0)
                {
                    Item item = KelpArmorAmmoUI.Slot.Item;

                    item.stack--;

                    Projectile.NewProjectile(pistolPos + new Vector2(16 * (pistolPos.X > player.Center.X ? -1 : 1), -30).RotatedBy(pistolRot), new Vector2(0, -8).RotatedBy(pistolRot), item.shoot, 10, 2f, player.whoAmI);
                }
            }

            Main.playerDrawData.Add(data5);

            Main.playerDrawData.Add(data3);
            Main.playerDrawData.Add(data4);

            Main.playerDrawData.Add(data);
            Main.playerDrawData.Add(data2);
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            KelpweaverArms.visible = true;
            layers.Insert(0, KelpweaverArms);
        }
        public override void PostUpdate()
        {
            if (kelpweaverSet)
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
            }
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
}