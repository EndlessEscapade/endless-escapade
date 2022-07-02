using EEMod.Extensions;
using EEMod.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace EEMod.UI.States
{
    public class DialogueUI : UIState
    {
        public static Dialogue CurrentDialogueSystem;
        public static UIElement Box;
        public static UIImage Portrait;
        public static string Name;
        public static DialogueBoxDivider DialogueBoxDivider;
        public static UIElement NamePoint;
        public static UIElement DialoguePoint;
        public static DialogueBox Background;
        public static List<Response> AllResponses;
        public static UIList ResponsesList;
        public static int ScrollHandler;
        public static string Dialogue;
        public static bool CanClickAButton = false;
        public string CurrentDialogueText = "";
        public string CurrentDialogueTextThatShouldBeDrawn = "";
        public bool IsHandlingTag;
        public bool JustStartedHandlingTag;
        public int AddLetterTimer;
        public int CurrentLetter;
        public int MoveTimer;
        public bool MovingUpwards;
        public override void OnInitialize()
        {
            AllResponses = new List<Response>();

            Box = new UIElement();
            Box.Width.Set(780, 0f);
            Box.Height.Set(200, 0f);
            Box.HAlign = 0.5f;
            Box.VAlign = 0.95f;

            Background = new DialogueBox();
            Background.HAlign = 0.5f;
            Background.VAlign = 1f;

            Portrait = new UIImage(ModContent.Request<Texture2D>("EEMod/Systems/Dialogues/SailorPortrait", AssetRequestMode.ImmediateLoad).Value);
            Portrait.HAlign = 0.05f;
            Portrait.VAlign = 0.250f;

            DialogueBoxDivider = new DialogueBoxDivider();
            DialogueBoxDivider.HAlign = 0.22f;
            DialogueBoxDivider.VAlign = 0.02125f;

            DialoguePoint = new UIElement();
            DialoguePoint.HAlign = 0.25f;
            DialoguePoint.VAlign = 0.15f;

            NamePoint = new UIElement();
            NamePoint.HAlign = 0.115f;
            NamePoint.VAlign = 0.7f;

            ResponsesList = new UIList();
            ResponsesList.Width.Set(678, 0f); //756
            ResponsesList.Height.Set(160, 0f);
            ResponsesList.HAlign = 0.5f;
            ResponsesList.VAlign = 0.5f;
            ResponsesList.ListPadding = 5f;

            Append(Box);
            Box.Append(Background);
            Box.Append(Portrait);
            Box.Append(DialogueBoxDivider);
            Box.Append(DialoguePoint);
            Box.Append(NamePoint);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Main.npcChatText != "")
            {
                Main.player[Main.myPlayer].sign = -1;
                Main.editSign = false;
                Main.player[Main.myPlayer].SetTalkNPC(-1, false);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = "";
            }
            if (CurrentDialogueSystem.LockPlayerMovement || Box.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (AllResponses.Count > 3)
            {
                if (Terraria.GameInput.PlayerInput.ScrollWheelDelta > 0 && ScrollHandler > 0)
                {
                    ResponsesList.Clear();
                    ResponsesList.Add(AllResponses[ScrollHandler - 1]);
                    ResponsesList.Add(AllResponses[ScrollHandler]);
                    ResponsesList.Add(AllResponses[ScrollHandler + 1]);
                    ScrollHandler--;
                    Main.LocalPlayer.ScrollHotbar(Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120);
                }
                if (Terraria.GameInput.PlayerInput.ScrollWheelDelta < 0 && ScrollHandler < AllResponses.Count - 3)
                {
                    ResponsesList.Clear();
                    ResponsesList.Add(AllResponses[ScrollHandler + 1]);
                    ResponsesList.Add(AllResponses[ScrollHandler + 2]);
                    ResponsesList.Add(AllResponses[ScrollHandler + 3]);
                    ScrollHandler++;
                    Main.LocalPlayer.ScrollHotbar(Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120);
                }
            }
            if (!string.IsNullOrEmpty(Dialogue))
            {
                if (CurrentLetter < Dialogue.Length && ++AddLetterTimer >= 2)
                {
                    var letter = Dialogue[CurrentLetter];
                    if (letter == '[')
                    {
                        while (Dialogue[CurrentLetter - 1] != ':')
                        {
                            CurrentLetter++;
                            CurrentDialogueText += letter;
                            letter = Dialogue[CurrentLetter];
                        }
                        if (Dialogue[CurrentLetter - 2] == 'i')
                        {
                            while (Dialogue[CurrentLetter + 1] != ']')
                            {
                                CurrentLetter++;
                                CurrentDialogueText += letter;
                                letter = Dialogue[CurrentLetter];
                            }
                        }
                        IsHandlingTag = true;
                        JustStartedHandlingTag = true;
                    }
                    if (letter == ']')
                    {
                        IsHandlingTag = false;
                        JustStartedHandlingTag = false;
                        CurrentLetter++;
                        CurrentDialogueText += letter;
                        letter = Dialogue[CurrentLetter];
                        CurrentDialogueText = CurrentDialogueText.Remove(CurrentDialogueText.Length - 1);
                    }
                    if (IsHandlingTag && !JustStartedHandlingTag)
                    {
                        CurrentDialogueText = CurrentDialogueText.Remove(CurrentDialogueText.Length - 1);
                    }
                    CurrentLetter++;
                    CurrentDialogueText += letter;
                    if (JustStartedHandlingTag)
                    {
                        JustStartedHandlingTag = false;
                    }
                    if (IsHandlingTag)
                    {
                        CurrentDialogueText += ']';
                    }
                    if (letter == '.' || letter == '?')
                    {
                        AddLetterTimer -= 8;
                    }
                    else if (letter == ',')
                    {
                        AddLetterTimer -= 5;
                    }
                    AddLetterTimer -= 2;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Don't ask why this has to be here and not in Update I couldn't answer you
            if (!CanClickAButton && Main.mouseLeftRelease) CanClickAButton = true;
            if (CurrentDialogueText != "" && ResponsesList.Count == 0 && CurrentDialogueSystem.LockPlayerMovement && Main.mouseLeft && Main.mouseLeftRelease || CurrentDialogueText != "" && ResponsesList.Count == 0 && Box.IsMouseHovering && Main.mouseLeft && Main.mouseLeftRelease)
            {
                if (CurrentDialogueText == Dialogue)
                {
                    CurrentDialogueText = "";
                    CurrentLetter = 0;
                    CurrentDialogueSystem.OnDialoguePieceFinished(CurrentDialogueSystem.Piece);
                    CanClickAButton = false;
                }
                else
                {
                    CurrentDialogueText = Dialogue;
                    CurrentLetter = Dialogue.Length;
                }
            }
            var dimensions = DialoguePoint.GetDimensions();
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, CurrentDialogueText, new Vector2(dimensions.X, dimensions.Y), Color.White, 0f, Vector2.Zero, new Vector2(1.15f, 1.15f));

            var dimensions2 = NamePoint.GetDimensions();
            if (Name.Contains("\n"))
            {
                var firstName = Name.Substring(0, Name.IndexOf("\n"));
                Vector2 size = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(firstName);
                Vector2 size2 = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(Name.Replace(firstName + "\n", ""));
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, firstName, new Vector2(dimensions2.X - (size.X / 2), dimensions2.Y), Color.White, 0f, Vector2.Zero, Vector2.One);
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, Name.Replace(firstName + "\n", ""), new Vector2(dimensions2.X - (size2.X / 2), dimensions2.Y + size2.Y), Color.White, 0f, Vector2.Zero, Vector2.One);
            }
            else
            {
                Vector2 size = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(Name);
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, Name, new Vector2(dimensions2.X - (size.X / 2), dimensions2.Y), Color.White, 0f, Vector2.Zero, Vector2.One);
            }
            if (AllResponses.Count > 3)
            {
                Texture2D arrow = ModContent.Request<Texture2D>("EEMod/UI/ScrollArrowWarning", AssetRequestMode.ImmediateLoad).Value;
                CalculatedStyle boxDimensions = Box.GetDimensions();
                int x = (int)(boxDimensions.X + (Box.Width.Pixels / 2) + (arrow.Size().X / 2));
                int y = (int)(boxDimensions.Y + (Box.Height.Pixels / 2) + (arrow.Size().Y / 2));
                if (MoveTimer++ > 30)
                {
                    MovingUpwards = !MovingUpwards;
                    MoveTimer = 0;
                }
                if (ScrollHandler > 0)
                {
                    spriteBatch.Draw(arrow, new Vector2(x - (Box.Width.Pixels / 2) + 30, y - (Box.Height.Pixels / 2) + MathHelper.SmoothStep(MovingUpwards ? 30f : 20f, MovingUpwards ? 20f : 30f, MoveTimer / 29f)), null, Color.White, 0f, arrow.Size(), 1f, SpriteEffects.FlipVertically, 0f);
                    spriteBatch.Draw(arrow, new Vector2(x + (Box.Width.Pixels / 2) - 30, y - (Box.Height.Pixels / 2) + MathHelper.SmoothStep(MovingUpwards ? 30f : 20f, MovingUpwards ? 20f : 30f, MoveTimer / 29f)), null, Color.White, 0f, arrow.Size(), 1f, SpriteEffects.FlipVertically, 0f);
                }
                if (ScrollHandler < AllResponses.Count - 3)
                {
                    spriteBatch.Draw(arrow, new Vector2(x - (Box.Width.Pixels / 2) + 30, y + (Box.Height.Pixels / 2) - MathHelper.SmoothStep(MovingUpwards ? 30f : 20f, MovingUpwards ? 20f : 30f, MoveTimer / 29f)), null, Color.White, 0f, arrow.Size(), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(arrow, new Vector2(x + (Box.Width.Pixels / 2) - 30, y + (Box.Height.Pixels / 2) - MathHelper.SmoothStep(MovingUpwards ? 30f : 20f, MovingUpwards ? 20f : 30f, MoveTimer / 29f)), null, Color.White, 0f, arrow.Size(), 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
    public class DialogueBox : UIImageButton
    {
        public Texture2D Texture;
        public Color ThemeColor;
        public DialogueBox() : base(ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxBackground"))
        {
            Texture = ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxBackground", AssetRequestMode.ImmediateLoad).Value;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var dimensions = GetDimensions();
            int x = (int)(dimensions.X + Texture.Size().X / 2);
            int y = (int)(dimensions.Y);
            spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor * 0.9f, 0f, Texture.Size(), 1f, SpriteEffects.None, 0f);
        }
    }
    public class DialogueBoxDivider : UIImage
    {
        public Texture2D Texture;
        public Color ThemeColor;
        public DialogueBoxDivider() : base(ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxDivider"))
        {
            Texture = ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxDivider", AssetRequestMode.ImmediateLoad).Value;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var dimensions = GetDimensions();
            int x = (int)(dimensions.X + Texture.Size().X / 2);
            int y = (int)(dimensions.Y + Texture.Size().Y);
            spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor * 0.9f, 0f, Texture.Size(), 1f, SpriteEffects.None, 0f);
        }
    }
    public class Response : UIImageButton
    {
        public Texture2D Texture;
        public Color ThemeColor;
        public int Piece;
        public string Text;
        public Response(int piece, string text, Color themeColor) : base(ModContent.Request<Texture2D>("EEMod/UI/DialogueResponse"))
        {
            Piece = piece;
            Text = text;
            ThemeColor = themeColor;
            Texture = ModContent.Request<Texture2D>("EEMod/UI/DialogueResponse", AssetRequestMode.ImmediateLoad).Value;
        }
        public override void Click(UIMouseEvent evt)
        {
            if (DialogueUI.CanClickAButton)
            {
                DialogueUI.CurrentDialogueSystem.OnDialoguePieceFinished(Piece);
                DialogueUI.Box.Append(DialogueUI.Portrait);
                DialogueUI.Box.Append(DialogueUI.DialogueBoxDivider);
                DialogueUI.Name = DialogueUI.CurrentDialogueSystem.Name.FormatString(20);
                DialogueUI.AllResponses.Clear();
                DialogueUI.ResponsesList.Clear();
                DialogueUI.ScrollHandler = 0;
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var dimensions = GetDimensions();
            int x = (int)(dimensions.X + Texture.Size().X);
            int y = (int)(dimensions.Y + Texture.Size().Y);
            float transparency = IsMouseHovering ? 0.9f : 0.4f;
            spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor * transparency, 0f, Texture.Size(), 1f, SpriteEffects.None, 0f);

            Vector2 size = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(Text);
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, Text, new Vector2(dimensions.X + (Texture.Size().X / 2) - (size.X / 2), dimensions.Y + (Texture.Size().Y / 2) - (size.Y / 2) + 2), Color.White, 0f, Vector2.Zero, Vector2.One);
        }
    }
}