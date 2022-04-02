using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using EEMod.Subworlds.CoralReefs;

namespace EEMod.Seamap.Content.Islands
{
    public class CoralReefsIsland : Island
    {
        public override string name => "Coral Reefs";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/CoralReefsIsland", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public CoralReefsIsland(Vector2 pos): base(pos)
        {
            
        }

        public override void Interact()
        {
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().prevKey = KeyID.Sea;

            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().myLastBoatPos = SeamapObjects.localship.position;

            Main.LocalPlayer.GetModPlayer<EEPlayer>().Initialize();
            Terraria.Graphics.Effects.Filters.Scene.Deactivate("EEMod:Noise2D");
            SubworldLibrary.SubworldSystem.Enter<CoralReefs>();

            base.Interact();
        }
    }
}
