using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.ID;
using EEMod.NPCs.Friendly;

namespace EEMod.Systems
{
	public class SailorDialogue : Dialogue
	{
		public override void StartDialogueRequiringNPC(int associatedNPC)
		{
			Portraits.Add(ModContent.Request<Texture2D>("EEMod/Systems/Dialogues/SailorPortrait", AssetRequestMode.ImmediateLoad).Value);
			Name = Main.npc[associatedNPC].FullName;
			AssociatedNPC = associatedNPC;
			ThemeColor = new Color(125, 175, 255);
			LockPlayerMovement = true;

			DialoguePieces = new List<string>()
			{
				/*0*/ "Sometimes I keep thinking about me old days, always sailing 'round the world.",
				/*1*/ "Can you help me repair that ship?",
				/*2*/ "What made you stop?",
				/*3*/ "Bye!",
				/*4*/ $"That ship sitting broken off the pier used to be one of the best vessels in the land. ",
				/*5*/ $"I'm past my days of sailing, but you look like you want to see the seven seas. ",
				/*6*/ $"If you brought me [c/E4A214:{"150§Wood"}] [i:{ItemID.Wood}] and [c/E4A214:{"20§Silk"}] [i:{ItemID.Silk}] along with a solid payment of [c/E4A214:{"5§gold§coins"}] [i:{ItemID.GoldCoin}], I'd get her fixed right up for you.",
				/*7*/ "Here you go!",
				/*8*/ "No, bye.",
				/*9*/ "Sorry, you don't have enough resources for me to repair the ship.",
				/*10*/"Oh, well, a while ago a storm hit my ship, and my brother was cast into the depths.",
				/*11*/"After that, I couldn't stomach going sailing the seas again. Brings back too much for me, ye know.",
				/*12*/"My memories with him are too fond to lose, though, so I stay by the sea in hopes that one day he'll come back. ",
				/*13*/"Sorry about that",
				/*14*/"Ah well, it's fine; it was years ago. I just hope one day I can see him again."
			};
			if (!Main.player[Main.myPlayer].GetModPlayer<DialoguePlayer>().HasTalkedToSailor)
            {
				SayPiece(0);
				Main.player[Main.myPlayer].GetModPlayer<DialoguePlayer>().HasTalkedToSailor = true;
			}
            else
            {
				SayPiece(0);
			}

			base.StartDialogueRequiringNPC(associatedNPC);
		}
		public override void OnDialoguePieceFinished(int piece)
		{
			switch (piece)
            {
                case (0):
					PresentResponses((!EEWorld.EEWorld.boatPlaced) ? new int[3] { 2, 1, 3 } : new int[2] { 2, 3 });
					break;
				case (1):
					//PresentResponses(new int[1] { 4 });
					SayPiece(4);
					break;
				case (2):
					SayPiece(10);
					break;
				case (10):
					SayPiece(11);
					break;
				case (11):
					SayPiece(12);
					break;
				case (12):
					PresentResponses(new int[1] { 13 });
					break;
				case (13):
					SayPiece(14);
					break;
				case (14):
					CloseDialogue();
					break;
				case (4):
					//PresentResponses(new int[1] { 5 });
					SayPiece(5);
					break;
				case (5):
					//PresentResponses(new int[1] { 6 });
					SayPiece(6);
					break;
				case (6):
					PresentResponses(new int[2] { 7, 8 });
					break;
				case (7):
					bool goodOnWood = false;
					bool goodOnSilk = false;
					bool goodOnMoney = false;

					for (int array = 0; array < 58; array++)
					{
						if (ItemID.Wood == Main.LocalPlayer.inventory[array].type && Main.LocalPlayer.inventory[array].stack >= 150)
						{
							goodOnWood = true;
							break;
						}
					}

					for (int array = 0; array < 58; array++)
					{
						if (ItemID.Silk == Main.LocalPlayer.inventory[array].type && Main.LocalPlayer.inventory[array].stack >= 20)
						{
							goodOnSilk = true;
							break;
						}
					}

					for (int array = 0; array < 58; array++)
					{
						if (ItemID.GoldCoin == Main.LocalPlayer.inventory[array].type && Main.LocalPlayer.inventory[array].stack >= 5)
						{
							goodOnMoney = true;
							break;
						}
					}

					if (goodOnWood && goodOnSilk && goodOnMoney)
					{
						(Main.npc[AssociatedNPC].ModNPC as Sailor).ticker = 0;
						(Main.npc[AssociatedNPC].ModNPC as Sailor).cutsceneOpacity = 0;
						(Main.npc[AssociatedNPC].ModNPC as Sailor).cutsceneActive = true;

						for (int array = 0; array < 58; array++)
						{
							if (ItemID.Wood == Main.LocalPlayer.inventory[array].type && Main.LocalPlayer.inventory[array].stack >= 150)
							{
								Main.LocalPlayer.inventory[array].stack -= 150;
								break;
							}
						}

						for (int array = 0; array < 58; array++)
						{
							if (ItemID.Silk == Main.LocalPlayer.inventory[array].type && Main.LocalPlayer.inventory[array].stack >= 20)
							{
								Main.LocalPlayer.inventory[array].stack -= 20;
								break;
							}
						}

						for (int array = 0; array < 58; array++)
						{
							if (ItemID.GoldCoin == Main.LocalPlayer.inventory[array].type && Main.LocalPlayer.inventory[array].stack >= 5)
							{
								Main.LocalPlayer.inventory[array].stack -= 5;
								break;
							}
						}

						CloseDialogue();
					}
					else
                    {
						SayPiece(9);
					}
					break;
				default:
					CloseDialogue();
					break;
			}
		}
	}
}
