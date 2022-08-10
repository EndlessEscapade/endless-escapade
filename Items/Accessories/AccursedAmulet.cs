using System;
using System.Collections.Generic;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class AccursedAmulet : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Amulet");
            Tooltip.SetDefault("Each nearby enemy increases your damage by 0.5%, however also decreases your health by 1.5%" + "\nMax modifier amount of 15 NPCs");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;

            Item.width = 16;
            Item.height = 16;

            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            const float distanceRadius = 1024f * 1024f;
            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                bool validNPC = npc.active && !npc.friendly && npc.damage > 0;
                bool validDistance = npc.DistanceSQ(player.Center) < distanceRadius;
                    
                if (!validNPC || !validDistance || !player.TryGetModPlayer(out AccursedAmuletPlayer accursedPlayer))
                    continue;

                accursedPlayer.NPCCount++;
            }
        }
    }
    
    public class AccursedAmuletPlayer : ModPlayer
    {
        public const int MaxNPCs = 15;
        
        public int NPCCount { get; set; }
        
        public override void ResetEffects()
        {
            NPCCount = 0;
        }

        public override void UpdateDead()
        {
            NPCCount = 0;
        }

        public override void PostUpdate()
        {
            if (NPCCount <= 0)
                return;
            else if (NPCCount > MaxNPCs)
                NPCCount = MaxNPCs;

            Player.statLifeMax2 -= (int)(Player.statLifeMax * 0.015f) * NPCCount;
            Player.GetDamage(DamageClass.Generic) += 0.05f * NPCCount;
        }
    }

    public class AccursedAmuletPlayerLayer : PlayerDrawLayer
    {
        private Color indicatorColor = Color.DarkGray;
        private float indicatorScale;
        private float indicatorAlpha;

        private int frameY;
        private int oldFrameY;
        
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.NeckAcc);
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            // TODO: Turn into utility method for future uses.
            bool hasAccessory = false;
            
            for (int i = 3; i < 10; i++)
            {
                Item item = drawInfo.drawPlayer.armor[i];

                if (item != null && !item.IsAir && item.active && item.type == ModContent.ItemType<AccursedAmulet>())
                {
                    hasAccessory = true;
                    break;
                }
            }

            if (!hasAccessory)
            {
                indicatorColor = Color.DarkGray;
                indicatorAlpha = 0f;
                indicatorScale = 0f;
            }

            return hasAccessory;
        }
        
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            indicatorColor = Color.Lerp(indicatorColor, Color.White, 0.1f);
            indicatorScale = MathHelper.Lerp(indicatorScale, 1f, 0.1f);
            indicatorAlpha = MathHelper.Lerp(indicatorAlpha, 1f, 0.1f);
            
            DrawIndicator(ref drawInfo);
            DrawGlowmask(ref drawInfo);
        }

        private void DrawIndicator(ref PlayerDrawSet drawInfo)
        {
            if (!drawInfo.drawPlayer.TryGetModPlayer(out AccursedAmuletPlayer accursedPlayer))
                return;
            
            oldFrameY = frameY;
            frameY = accursedPlayer.NPCCount switch
            {
                > 0 and <= 5 => 1, // 1 - 4
                > 5 and <= 10 => 2, // 6 - 10
                > 10 and < 15 => 3, // 11 - 14
                15 => 4, // 15
                _ => 0 // Default
            };
            
            if (frameY != oldFrameY)
                indicatorColor = Color.DarkGray;
            
            Texture2D texture = Mod.Assets.Request<Texture2D>("Textures/AccursedAmuletIndicator").Value;
            Vector2 sineOffset = new(0f, MathF.Sin(Main.GameUpdateCount * 0.05f) * 2f);
            Vector2 position = drawInfo.drawPlayer.MountedCenter - Main.screenPosition + new Vector2(0f, drawInfo.drawPlayer.gfxOffY) - new Vector2(0f, 48f) + sineOffset;
            Rectangle sourceRect = new(0, frameY * 28, 34, 28);
            DrawData data = new(texture, position, sourceRect, indicatorColor * indicatorAlpha, 0f, sourceRect.Size() / 2f, indicatorScale, SpriteEffects.None, 0);
            
            drawInfo.DrawDataCache.Add(data);
        }

        private void DrawGlowmask(ref PlayerDrawSet drawInfo)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Accessories/AccursedAmulet_Neck").Value;
            
            Vector2 position = new Vector2(
                    (int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2f + drawInfo.drawPlayer.width / 2f),
                    (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)
                ) +
                drawInfo.drawPlayer.bodyPosition +
                drawInfo.drawPlayer.bodyFrame.Size() / 2f;
            
            DrawData data = new(texture, position, drawInfo.drawPlayer.bodyFrame, indicatorColor * indicatorAlpha, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);

            drawInfo.DrawDataCache.Add(data);
        }
    }
}