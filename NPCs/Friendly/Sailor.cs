using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System;
using EEMod.Items.Placeables.Furniture;
using System.Collections.Generic;
using Terraria.Utilities;
using EEMod.Systems;
using EEMod.Items;
using EEMod.Items.Shipyard;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using EEMod.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Audio;
using System.Diagnostics;
using ReLogic.Content;

namespace EEMod.NPCs.Friendly
{
    [AutoloadHead]
    public class Sailor : EENPC
    {
        public override string Texture => "EEMod/NPCs/Friendly/Sailor";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor");

            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;

            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 30;
            NPC.height = 50;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }
            }

            return false;
        }

        public override string TownNPCName()
        {
            return "Skipper";
            /*switch (WorldGen.genRand.Next(8))
            {
                case 0:
                    return "James";
                case 1:
                    return "Peter";
                case 2:
                    return "Francis";
                case 3:
                    return "John";
                case 4:
                    return "Ferdinand";
                case 5:
                    return "Herman";
                case 6:
                    return "Christopher";
                case 7:
                    return "Jack";
                default:
                    return "Popeye";
            }*/
        }

        float cutsceneOpacity = 0;

        int ticker = 0;

        public override bool PreAI()
        {
            if (NPC.CountNPCS(Type) > 1)
            {
                NPC.active = false;
            }
            if (Main.npcChatText != "")
            {
                Main.player[Main.myPlayer].sign = -1;
                Main.editSign = false;
                Main.player[Main.myPlayer].SetTalkNPC(-1, false);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = "";
                new SailorDialogue().StartDialogueRequiringNPC(NPC.whoAmI);
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.homeTileX = (int)EEWorld.EEWorld.shipCoords.X + 108 + 7;
            NPC.homeTileY = (int)EEWorld.EEWorld.shipCoords.Y - 8;

            if (cutsceneActive)
            {
                if (ticker == 0)
                {
                    Filters.Scene.Activate("EEMod:Ripple", Main.LocalPlayer.Center).GetShader().UseOpacity(cutsceneOpacity);
                }

                Filters.Scene["EEMod:Ripple"].GetShader().UseOpacity(cutsceneOpacity);

                if (ticker > 0 && ticker < 240)
                    cutsceneOpacity = (ticker / 60f) * 1000;
                else if (ticker >= 240)
                    cutsceneOpacity = ((600 - ticker) / 60f) * 1000;

                if(ticker % 60 == 0 && ticker > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item37, Main.LocalPlayer.Center);
                }

                if(ticker == 150)
                {
                    EEWorld.EEWorld.ClearRegion(50, 50, new Vector2((int)EEWorld.EEWorld.shipCoords.X - 2, (int)EEWorld.EEWorld.shipCoords.Y - 18));

                    Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/builtboat.lcs")).PlaceAt((int)EEWorld.EEWorld.shipCoords.X - 2, (int)EEWorld.EEWorld.shipCoords.Y - 18, false, false);

                    for(int i = (int)EEWorld.EEWorld.shipCoords.X - 2; i < (int)EEWorld.EEWorld.shipCoords.X - 2 + 50; i++)
                    {
                        for(int j = (int)EEWorld.EEWorld.shipCoords.Y - 18; j < (int)EEWorld.EEWorld.shipCoords.Y - 18 + 50; j++)
                        {
                            if(Main.tile[i, j].wall != WallID.None)
                            {
                                Main.tile[i, j].LiquidAmount = 0;
                            }
                        }
                    }

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].type == ModContent.ProjectileType<TornSails>()
                            || Main.projectile[i].type == ModContent.ProjectileType<TileExperimentation>()
                            || Main.projectile[i].type == ModContent.ProjectileType<Bridge>())
                        {
                            Main.projectile[i].Kill();
                        }
                    }

                    Main.projectile[Main.LocalPlayer.GetModPlayer<EEPlayer>().tetherProj].Kill();
                    Main.projectile[Main.LocalPlayer.GetModPlayer<EEPlayer>().sailProj].Kill();

                    EEWorld.EEWorld.boatPlaced = true;

                    Debug.WriteLine(EEWorld.EEWorld.boatPlaced);

                    WorldGen.PlaceTile((int)EEWorld.EEWorld.shipCoords.X + 10, (int)EEWorld.EEWorld.shipCoords.Y + 7, ModContent.TileType<WoodenShipsWheelTile>());
                }

                if(ticker == 240)
                {
                    Main.LocalPlayer.position = new Vector2(15 * 16, 6 * 16) + (EEWorld.EEWorld.shipCoords * 16);
                    Main.LocalPlayer.direction = 1;

                    NPC.position = new Vector2(20 * 16, 6 * 16) + (EEWorld.EEWorld.shipCoords * 16);
                    NPC.direction = -1;
                }

                if (ticker == 300)
                {
                    if (Filters.Scene["EEMod:Ripple"].IsActive()) Filters.Scene.Deactivate("EEMod:Ripple");

                    cutsceneActive = false;
                }

                ticker++;
            }
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int angler = NPC.FindFirstNPC(NPCID.Angler);

            if (angler >= 0)
            {
                if (Main.dayTime)
                {
                    chat.Add("I always love stepping out on the pier at the crack of dawn.");
                    chat.Add("The ocean's so enticing today, don't ya think?");
                    chat.Add("The sharks seem excited today.");
                }
                else
                {
                    chat.Add("The ocean waves are always so calm at nighttime.");
                    chat.Add("The moon looks so beautiful on the water.");
                    chat.Add("I love the glow of the jellies.");

                    if (Main.moonType == 4)
                        chat.Add("The new moon is a sign that the jellyfish over the Coral Reefs are on the move.");
                }

                if (Main.raining)
                {
                    chat.Add("I hope this rain doesn't mean a hurricane's coming!");
                    chat.Add("I lost my rain slicker in a windy day a few years ago. Wish I had another one.");
                }
            }
            else
            {
                chat.Add("Have you seen my son anywhere? He went out on a fishing trip recently and hasn't come back.");
                chat.Add("My son knows the waves well, so I hope he's alright...");
                chat.Add("Have you seen my son? He wears a beige cap and a vest.");
            }

            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            int angler = NPC.FindFirstNPC(NPCID.Angler);

            if (angler >= 0)
            {
                button = Language.GetTextValue("LegacyInterface.28");
                button2 = "Ship";
            }
        }

        bool shipAlreadyOpen = false;
        bool cutsceneActive = false;

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
                shipAlreadyOpen = false;
            }

            else
            {
                if (!EEWorld.EEWorld.boatPlaced)
                {
                    if (shipAlreadyOpen /*check conditions for placing the boat*/)
                    {
                        ticker = 0;

                        cutsceneOpacity = 0;

                        cutsceneActive = true;
                    }

                    Main.playerInventory = true;
                    shipAlreadyOpen = true;

                    Main.npcChatText = $"The old ship sitting broken off the pier used to be my old vessel. " +
                        $"I'm past my days of sailing, but you look like you want to see the seven seas. " +
                        $"If you brought me [c/E4A214:{"150 Wood"}] [i:{ItemID.Wood}] and [c/E4A214:{"20 Silk"}] [i:{ItemID.Silk}] " +
                        $"along with a solid payment of [c/E4A214:{"5 gold coins"}] [i:{ItemID.GoldCoin}], I'd get her fixed right up for you.";
                }
                else
                {
                    Main.npcChatText = $"Before you go sailing, don't forget to make a [c/E4A214:{"Wooden Ship's Wheel"}] [i:{ModContent.ItemType<WoodenShipsWheel>()}] to steer your boat" +
                        $", and don't forget that you can always stop by and see if I've designed new sails, wheels, cannons, or other bits and bobs for sailing.";
                }
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Telescope>());
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<FishermansLog>());
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<SwiftSail>());

            nextSlot++;
            shipAlreadyOpen = false;
        }

        public override void OnKill()
        {
            //Item.NewItem(npc.getRect(), ModContent.ItemType<>());

            //Sailor's Hat?
        }

        // Make this Town NPC teleport to the King and/or Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Anchor;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}