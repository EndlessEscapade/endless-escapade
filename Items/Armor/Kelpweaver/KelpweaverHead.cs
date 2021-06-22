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

namespace EEMod.Items.Armor.Kelpweaver
{
    [AutoloadEquip(EquipType.Head)]
    public class KelpweaverHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpweaver Head");
            Tooltip.SetDefault("Creepy and crawly");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<KelpweaverBody>() && legs.type == ModContent.ItemType<KelpweaverLegs>();
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.02f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+1 max minions";
            player.maxMinions++;

            player.GetModPlayer<KelpweaverPlayer>().kelpweaverSet = true;
        }
    }

    public class KelpweaverPlayer : ModPlayer
    {
        public bool kelpweaverSet;

        private static float gun1rot = 5.49f;
        private static float gun2rot = 0.79f;
        private static float gun3rot = 3.92f;
        private static float gun4rot = 2.36f;

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

                float normalRot = Vector2.Normalize(targetNPC.Center - player.Center).ToRotation();

                if (Vector2.Distance(targetNPC.Center, player.Center) <= 320)
                {
                    if (normalRot >= 0 && normalRot < 1.57f)
                    {
                        gun1 = true;

                        /*if (gun1rot < normalRot && gun1rot < )
                        {
                            gun1rot += 0.02f;
                        }

                        if (gun1rot > normalRot)
                        {
                            gun1rot -= 0.02f;
                        }*/
                    }

                    if (normalRot >= 1.57f && normalRot < 3.14f)
                    {
                        gun2 = true;
                    }

                    if (normalRot >= 3.14f && normalRot < 4.71f)
                    {
                        gun3 = true;
                    }

                    if (normalRot >= 4.71f && normalRot < 6.29f)
                    {
                        gun4 = true;
                    }
                }

                //restrict between 0.4 and 0.6, multiply by 1.57

                DrawNewLeg(player.Center, player, gun1rot, gun3, 3);

                DrawNewLeg(player.Center, player, gun2rot, gun4, 4);

                DrawNewLeg(player.Center, player, gun3rot, gun2, 2);

                DrawNewLeg(player.Center, player, gun4rot, gun1, 1);
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

                if (Main.GameUpdateCount % 30 == 0)
                {
                    Projectile.NewProjectile(pistolPos + new Vector2(16 * (pistolPos.X > player.Center.X ? -1 : 1), -30).RotatedBy(pistolRot), new Vector2(0, -8).RotatedBy(pistolRot), ProjectileID.Bullet, 10, 2f, player.whoAmI);
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
    }
}