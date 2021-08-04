using System;
using System.Reflection;
using NVorbis;
using Terraria.ModLoader.Audio;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using EEMod.NPCs.Friendly;
using Microsoft.Xna.Framework;

namespace EEMod.Systems
{
	public class JonathanSeagullIntroduction : Dialogue
	{
		public override void StartDialogueRequiringNPC(int associatedNPC)
		{
			MainPortrait = ModContent.GetTexture("EEMod/icon");
			Name = "Jonathan the Cool Seagull";
			AssociatedNPC = associatedNPC;
			ThemeColor = new Color(106, 255, 89);
			DialoguePieces = new List<string>()
			{
				/*0*/ "Hey bro, this place is pretty nice isn't it? The breeze is soooo cool.",
				/*1*/ "Anyways, you wouldn't happen to have seen DN around here somewhere would you?",
				/*2*/ "Who's DN?",
				/*3*/ "Yeah, I saw them over by that island up north.",
				/*4*/ "No I haven't, sorry.",
				/*5*/ "Deez Nuts.",
				/*6*/ "Thanks bro, I'll go check there.",
				/*7*/ "That's a shame, if you happen to find them please tell me."
			};
			base.StartDialogueRequiringNPC(associatedNPC);
		}
		public override void OnDialoguePieceFinished(int piece)
		{
			switch (piece) 
			{
				case 0:
					SayPiece(1);
					break;
				case 1:
					PresentResponses(new int[3] { 2, 3, 4 });
					break;
				case 2:
					SayPiece(5);
					break;
				case 3:
					SayPiece(6);
					break;
				case 4:
					SayPiece(7);
					break;
				default:
					CloseDialogue();
					(Main.npc[AssociatedNPC].modNPC as JonathanSeagull).IntroductionDialogue = true;
					break;
			}
		}
	}
}
