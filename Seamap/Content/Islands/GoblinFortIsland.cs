using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using EEMod.Systems;
using EEMod.Subworlds;

namespace EEMod.Seamap.Content.Islands
{
    public class GoblinFortIsland : Island
    {
        public override string name => "Goblin Fort";
        public override int framecount => 1;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/GoblinFort", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public GoblinFortIsland(Vector2 pos): base(pos)
        {
            
        }

        public override void Interact()
        {
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().prevKey = KeyID.Sea;

            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().myLastBoatPos = SeamapObjects.localship.position;

            Main.LocalPlayer.GetModPlayer<EEPlayer>().Initialize();
            Terraria.Graphics.Effects.Filters.Scene.Deactivate("EEMod:Noise2D");
            SubworldLibrary.SubworldSystem.Enter<GoblinFort>();

            base.Interact();
        }
    }
}