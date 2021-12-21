using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EEMod.Seamap.SeamapAssets
{
    public class Rock1 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Rock1(Vector2 pos): base(pos)
        {
            
        }
    }

    public class Rock2 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Rock2(Vector2 pos) : base(pos)
        {

        }
    }

    public class Rock3 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Rock3(Vector2 pos) : base(pos)
        {

        }
    }

    public class Rock4 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Rock4(Vector2 pos) : base(pos)
        {

        }
    }

    public class Rock5 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock5", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Rock5(Vector2 pos) : base(pos)
        {

        }

        public override void PostDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock5Front").Value, position - Main.screenPosition, new Rectangle(0, (texture.Height / framecount) * frame, texture.Width, (texture.Height / framecount)), SeamapContent.Seamap.seamapDrawColor);

            base.PostDraw(spriteBatch);
        }
    }

    public class Rock6 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock6", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Rock6(Vector2 pos) : base(pos)
        {

        }
    }
}
