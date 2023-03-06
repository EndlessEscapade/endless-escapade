using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace EndlessEscapade.Common.UIElements;

internal class UICheckbox : UIText
{
    public static Asset<Texture2D> checkboxTexture = ModContent.Request<Texture2D>("EndlessEscapade/Assets/UI/UICheckbox", AssetRequestMode.ImmediateLoad);
    public static Asset<Texture2D> checkmarkTexture = ModContent.Request<Texture2D>("EndlessEscapade/Assets/UI/UICheckboxToggled", AssetRequestMode.ImmediateLoad);

    public event EventHandler OnSelectedChanged;

    private bool selected = false;
    private bool disabled = false;
    internal string hoverText;

    public bool Selected {
        get { return selected; }
        set {
            if (value != selected) {
                selected = value;
                OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public UICheckbox(string text, string hoverText, float textScale = 1, bool large = false) : base(text, textScale, large) {
        Left.Pixels += 20;
        text = "   " + text;
        this.hoverText = hoverText;
        SetText(text);
        OnClick += UICheckbox_onLeftClick;
        Recalculate();
    }

    private void UICheckbox_onLeftClick(UIMouseEvent evt, UIElement listeningElement) {
        if (disabled) return;
        Selected = !Selected;
    }

    public void SetDisabled(bool disabled = true) {
        this.disabled = disabled;
        if (disabled) {
            Selected = false;
        }
        TextColor = disabled ? Color.Gray : Color.White;
    }
    public void SetHoverText(string hoverText) {
        this.hoverText = hoverText;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        base.DrawSelf(spriteBatch);

        var innerDimensions = GetInnerDimensions();
        var pos = new Vector2(innerDimensions.X, innerDimensions.Y - 5);

        spriteBatch.Draw(checkboxTexture.Value, pos, null, disabled ? Color.Gray : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        if (Selected)
            spriteBatch.Draw(checkmarkTexture.Value, pos, null, disabled ? Color.Gray : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        if (IsMouseHovering) {
            Main.hoverItemName = hoverText;
        }
    }
}