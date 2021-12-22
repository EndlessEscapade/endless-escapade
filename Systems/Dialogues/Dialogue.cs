using System;
using System.Reflection;
using NVorbis;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using EEMod.UI.States;
using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace EEMod.Systems
{
	public class Dialogue
	{
		public List<string> DialoguePieces = new List<string>();
		public List<Texture2D> Portraits = new List<Texture2D>();
		public string Name;
		public int AssociatedNPC;
		public int CurrentPortrait;
		public Color ThemeColor;
		public bool LockPlayerMovement;
		public int Piece;

		/*
		public virtual void StartDialogue()
		{
			EEMod.UI.SetState("DialogueInterface", "DialogueUI");
			DialogueUI.CurrentDialogueSystem = this;
			DialogueUI.Background.ThemeColor = ThemeColor;
			DialogueUI.Portrait.SetImage(Portraits[0]);
			DialogueUI.Dialogue = DialoguePieces[0].FormatString(60);
		}
		*/
		public virtual void StartDialogueRequiringNPC(int associatedNPC)
		{
			EEMod.UI.SetState("DialogueInterface", "DialogueUI");
			AssociatedNPC = associatedNPC;
			DialogueUI.CurrentDialogueSystem = this;
			DialogueUI.Background.ThemeColor = ThemeColor;
			DialogueUI.Portrait.SetImage(Portraits[0]);
			DialogueUI.Name = Name.FormatString(20);
			DialogueUI.DialogueBoxDivider.ThemeColor = ThemeColor;
			DialogueUI.Dialogue = DialoguePieces[0].FormatString(64);
			//You can guess by now that I have no idea why these 2 are needed but they are
			DialogueUI.ResponsesList.Add(new Response(0, "", ThemeColor));
			DialogueUI.ResponsesList.Clear();
		}
		public virtual void SayPiece(int piece) 
		{
			DialogueUI.Dialogue = (DialoguePieces[piece].FormatString(64));
			Piece = piece;
		}
		public virtual void PresentResponses(int[] responses)
		{
			DialogueUI.Dialogue = "";
			DialogueUI.Portrait.Remove();
			DialogueUI.DialogueBoxDivider.Remove();
			DialogueUI.Name = "";
			for (int i = 0; i < responses.Length; i++)
            {
				DialogueUI.ResponsesList.Add(new Response(responses[i], DialoguePieces[responses[i]], ThemeColor));
			}
			DialogueUI.Box.Append(DialogueUI.ResponsesList);
		}
		public virtual void CloseDialogue() 
		{
			EEMod.UI.RemoveState("DialogueInterface");
		}
		public virtual void OnDialoguePieceFinished(int piece) { }
    }
}
