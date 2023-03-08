using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace EndlessEscapade.Common.UIElements;

internal class UIHoverImageButton : UIImageButton
{
    internal string HoverText { get; set; }

    public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture) {
        HoverText = hoverText;
    }

    public void SetHoverText(string hoverText) {
        HoverText = hoverText;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (IsMouseHovering) Main.hoverItemName = HoverText;
    }
}