using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;
using CustomSlot;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using EEMod.Extensions;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using ReLogic.Content;
using EEMod.Systems;

namespace EEMod.UI.States
{
	public class DialogueUI : UIState
	{
		public static Dialogue CurrentDialogueSystem;
		public static UIElement Box;
		public static UIImage Portrait;
		public static UIElement DialoguePoint;
		public static DialogueBox Background;
		public static UIList ResponsesList;
		public static string Dialogue;
		public string CurrentDialogueText = "";
		public int AddLetterTimer;
		public int CurrentLetter;
        public override void OnInitialize()
		{
			Box = new UIElement();
			Box.Width.Set(780, 0f);
			Box.Height.Set(200, 0f);
			Box.HAlign = 0.5f;
			Box.VAlign = 1f;

			Background = new DialogueBox();
			Background.HAlign = 0.5f;
			Background.VAlign = 1f;

			Portrait = new UIImage(ModContent.Request<Texture2D>("EEMod/icon", AssetRequestMode.ImmediateLoad).Value);
			Portrait.HAlign = 0.09f;
			Portrait.VAlign = 0.5f;

			DialoguePoint = new UIElement();
			DialoguePoint.HAlign = 0.25f;
			DialoguePoint.VAlign = 0.18f;

			ResponsesList = new UIList();
			ResponsesList.Width.Set(756, 0f);
			ResponsesList.Height.Set(180, 0f);
			ResponsesList.HAlign = 0.5f;
			ResponsesList.VAlign = 0.5f;
			ResponsesList.ListPadding = 8f;

			Append(Box);
			Box.Append(Background);
			Box.Append(Portrait);
			Box.Append(DialoguePoint);
			Box.Append(ResponsesList);
		}
		public override void Update(GameTime gameTime)
		{ 
            base.Update(gameTime);
			if (CurrentLetter < Dialogue.Length && ++AddLetterTimer >= 2)
			{
				AddLetterTimer -= 2;
				var letter = Dialogue[CurrentLetter];
				CurrentLetter++;
				CurrentDialogueText += letter;
				if (letter == '.' || letter == '?')
				{
					AddLetterTimer -= 8;
				}
				else if (letter == ',')
				{
					AddLetterTimer -= 5;
				}
			}
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			//Don't ask why this has to be here and not in Update I couldn't answer you
			if (CurrentDialogueSystem.LockPlayerMovement && Main.mouseLeft && Main.mouseLeftRelease || Box.IsMouseHovering && Main.mouseLeft && Main.mouseLeftRelease)
			{
				if (CurrentDialogueText == Dialogue)
                {
					CurrentDialogueText = "";
					CurrentLetter = 0;
					CurrentDialogueSystem.OnDialoguePieceFinished(CurrentDialogueSystem.Piece);
                }
                else
                {
					CurrentDialogueText = Dialogue;
					CurrentLetter = Dialogue.Length;
				}
			}
			var dimensions = DialoguePoint.GetDimensions();
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, CurrentDialogueText, new Vector2(dimensions.X, dimensions.Y), Color.White, 0f, Vector2.Zero, new Vector2(1.15f, 1.15f));
		}
	}
	public class DialogueBox : UIImageButton
	{
		public Texture2D Texture;
		public Color ThemeColor;
		public DialogueBox() : base(ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxBackground"))
		{
			Texture = ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxBackground", AssetRequestMode.ImmediateLoad).Value;
			ThemeColor = Color.LightBlue;
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{ 
			var dimensions = GetDimensions();
			int x = (int)(dimensions.X + Texture.Size().X / 2);
			int y = (int)(dimensions.Y);
			spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor * 0.9f, 0f, Texture.Size(), 1f, SpriteEffects.None, 0f);
        }
    }
    public class Response : UIImageButton
    {
		public Texture2D Texture;
		public Color ThemeColor;
		public int Piece;
		public Response(int piece) : base(ModContent.Request<Texture2D>("EEMod/UI/DialogueResponse"))
		{
			Piece = piece;
			Texture = ModContent.Request<Texture2D>("EEMod/UI/DialogueResponse", AssetRequestMode.ImmediateLoad).Value;
			ThemeColor = Color.LightBlue;
		}
        public override void Click(UIMouseEvent evt)
        {
			Main.NewText(Piece);
			DialogueUI.CurrentDialogueSystem.OnDialoguePieceFinished(Piece);
			DialogueUI.Box.Append(DialogueUI.Portrait);
			DialogueUI.ResponsesList.Clear();
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			var dimensions = GetDimensions();
			int x = (int)(dimensions.X + Texture.Size().X);
			int y = (int)(dimensions.Y + Texture.Size().Y);
			spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor * 0.9f, 0f, Texture.Size(), 1f, SpriteEffects.None, 0f);
		}
	}
}