using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace EndlessEscapade.Common.UIElements;

internal class UIHoverImageButton : UIImageButton
{
    internal string hoverText { get; set; }

    public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture) {
        this.hoverText = hoverText;
    }

    public void SetHoverText(string hoverText) {
        this.hoverText = hoverText;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (IsMouseHovering) {
            Main.hoverItemName = hoverText;
        }
    }
}