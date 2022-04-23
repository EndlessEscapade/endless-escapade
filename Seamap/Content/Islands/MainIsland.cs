using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EEMod.Seamap.Content.Islands
{
    public class MainIsland : Island
    {
        public override string name => "Main Island";
        public override int framecount => 1;
        public override int framespid => 0;
        public override bool cancollide => true;

        public override int interactDistance => 300;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/MainIsland", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public MainIsland(Vector2 pos): base(pos)
        {
            width = 402;
            height = 118;
        }

        public override void Interact()
        {
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamapKey = "Main";
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamap = true;

            base.Interact();
        }
    }
}
