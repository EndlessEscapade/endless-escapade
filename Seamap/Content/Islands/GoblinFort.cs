using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using EEMod.Systems;
using EEMod.Systems.Subworlds.EESubworlds;

namespace EEMod.Seamap.Content.Islands
{
    public class GoblinFort : Island
    {
        public override string name => "Goblin Fort";
        public override int framecount => 1;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/GoblinFort", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public GoblinFort(Vector2 pos): base(pos)
        {
            
        }

        public override void Interact()
        {
            EEPlayer.prevKey = KeyID.Sea;

            Main.LocalPlayer.GetModPlayer<EEPlayer>().Initialize();
            Terraria.Graphics.Effects.Filters.Scene.Deactivate("EEMod:Noise2D");
            SubworldManager.EnterSubworld<Systems.Subworlds.EESubworlds.GoblinFort>();

            base.Interact();
        }
    }
}