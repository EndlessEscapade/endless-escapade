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
        public override bool collides => true;

        public override int interactDistance => width * 2;


        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/GoblinFortIsland", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public GoblinFortIsland(Vector2 pos): base(pos)
        {
            width = 70;
            height = 56;
        }

        public override void Interact()
        {
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().prevKey = KeyID.Sea;
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().myLastBoatPos = SeamapObjects.localship.position;
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamap = true;
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamapKey = KeyID.GoblinFort;


            EEMod.isSaving = true;

            base.Interact();
        }
    }
}