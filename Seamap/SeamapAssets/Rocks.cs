using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class Rock1 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public override IslandID id => IslandID.Default;

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

        public override IslandID id => IslandID.Default;

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

        public override IslandID id => IslandID.Default;

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

        public override IslandID id => IslandID.Default;

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

        public override IslandID id => IslandID.Default;

        public Rock5(Vector2 pos) : base(pos)
        {

        }
    }

    public class Rock6 : Island
    {
        public override string name => "Rock";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Rock6", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public override IslandID id => IslandID.Default;

        public Rock6(Vector2 pos) : base(pos)
        {

        }
    }
}
