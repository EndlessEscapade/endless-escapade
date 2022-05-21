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
using EEMod.Tiles.Furniture.Shipyard;
using Terraria.GameContent.Personalities;

namespace EEMod.NPCs.Friendly
{
    [AutoloadHead]
    public class Sailor : EENPC
    {
        public override string Texture => "EEMod/NPCs/Friendly/Sailor";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor");

            Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
            NPC.Happiness
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Love) // Example Person prefers the forest.
                .SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate) // Loves living near the dryad.
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Love); // Hates living near the demolitionist.
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

        public override List<string> SetNPCNameList()
        {
            return new List<string>() { "Skipper" };
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

        public float cutsceneOpacity = 0;

        public int ticker = 0;

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
                Main.hideUI = true;

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
                    EEWorld.EEWorld.ClearRegion(45, 45, new Vector2((int)EEWorld.EEWorld.shipCoords.X - 2, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2));

                    Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/builtboat.lcs")).PlaceAt((int)EEWorld.EEWorld.shipCoords.X - 2 + 7, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2, false, false);

                    for(int i = (int)EEWorld.EEWorld.shipCoords.X - 2; i < (int)EEWorld.EEWorld.shipCoords.X - 2 + 50; i++)
                    {
                        for(int j = (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2; j < (int)EEWorld.EEWorld.shipCoords.Y - 18 + 50 - 2; j++)
                        {
                            if(Framing.GetTileSafely(i, j).WallType != WallID.None)
                            {
                                Framing.GetTileSafely(i, j).LiquidAmount = 0;
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

                    Main.projectile[EEWorld.EEWorld.tetherProj].Kill();
                    Main.projectile[EEWorld.EEWorld.sailProj].Kill();

                    EEWorld.EEWorld.boatPlaced = true;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }

                    WorldGen.PlaceTile((int)EEWorld.EEWorld.shipCoords.X - 2 + 7 + 12, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2 + 25, ModContent.TileType<WoodenShipsWheelTile>());

                    WorldGen.PlaceTile((int)EEWorld.EEWorld.shipCoords.X - 2 + 7 + 12 - 9, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2 + 25 + 5, ModContent.TileType<FigureheadTile>());

                    WorldGen.PlaceTile((int)EEWorld.EEWorld.shipCoords.X - 2 + 7 + 12 + 12, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2 + 25 + 1, ModContent.TileType<CannonTile>());

                    WorldGen.PlaceTile((int)EEWorld.EEWorld.shipCoords.X - 2 + 7 + 12 + 24, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2 + 25 + 1, ModContent.TileType<MapTable>());

                    WorldGen.PlaceTile((int)EEWorld.EEWorld.shipCoords.X - 2 + 7 + 12 + 12 + 9, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2 + 25 + 1 + 7, ModContent.TileType<ShipStorage>());

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, (int)EEWorld.EEWorld.shipCoords.X - 2 + 7, (int)EEWorld.EEWorld.shipCoords.Y - 18 - 2, 50, 50); // Immediately inform clients of new world state.
                    }
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

                    Main.hideUI = false;
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

        public bool shipAlreadyOpen = false;
        public bool cutsceneActive = false;

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
                shipAlreadyOpen = false;
            }

            /*else
            {
                if (!EEWorld.EEWorld.boatPlaced)
                {
                    if (shipAlreadyOpen)
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
            }*/
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Telescope>());
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<FishermansLog>());

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