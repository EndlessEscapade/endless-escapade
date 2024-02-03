using EndlessEscapade.Content.Items.Armor.StormKnight;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items.Armor;

public sealed class StormKnightCrestLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() {
        return new AfterParent(PlayerDrawLayers.Head);
    }

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
        return !Main.gameMenu && drawInfo.drawPlayer.HasHelmet<StormKnightCrest>();
    }

    protected override void Draw(ref PlayerDrawSet drawInfo) {
        var texture = Mod.Assets.Request<Texture2D>("Content/Items/Armor/StormKnight/StormKnightCrest_Head_Glow").Value;

        var position = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X) + (drawInfo.drawPlayer.width - drawInfo.drawPlayer.bodyFrame.Width) / 2,
            (int)(drawInfo.Position.Y - Main.screenPosition.Y) + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4);
        var data = new DrawData(texture,
            position + drawInfo.drawPlayer.headPosition + drawInfo.rotationOrigin,
            drawInfo.drawPlayer.bodyFrame,
            Color.White,
            drawInfo.drawPlayer.headRotation,
            drawInfo.rotationOrigin,
            1f,
            drawInfo.playerEffect);

        drawInfo.DrawDataCache.Add(data);
    }
}
