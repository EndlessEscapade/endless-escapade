using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EEMod.Seamap.Content.Islands
{
    public class Iceberg2 : Island
    {
        public override string name => "Iceberg";
        public override int framecount => 1;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/Iceberg2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Iceberg2(Vector2 pos): base(pos)
        {
            
        }

        //public override bool IslandDraw(SpriteBatch spriteBatch)
        //{
        //    Texture2D mask = ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value;
        //
        //    Helpers.DrawAdditive(mask, Center - Main.screenPosition, Color.White * 0.5f, 3f);
        //
        //    spriteBatch.Draw(texture, position - Main.screenPosition, new Rectangle(0, (texture.Height / framecount) * frame, texture.Width, (texture.Height / framecount)), SeamapContent.Seamap.seamapDrawColor);
        //
        //    return false;
        //}
    }
}
