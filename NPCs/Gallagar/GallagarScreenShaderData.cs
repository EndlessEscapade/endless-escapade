using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace InteritosMod.NPCs.Gallagar
{
    public class GallagarScreenShaderData : ScreenShaderData
    {
        // Thanks OS for code!
        private int GallagarIndex;

        public GallagarScreenShaderData(string passName) : base(passName)
        {
        }

        private void UpdateGallagarIndex()
        {
            int num = ModContent.NPCType<Gallagar>();
            if (GallagarIndex >= 0 && Main.npc[GallagarIndex].active && Main.npc[GallagarIndex].type == num)
            {
                return;
            }
            GallagarIndex = Helpers.FirstNPCIndex(num);
        }

        public override void Apply()
        {
            UpdateGallagarIndex();
            if (GallagarIndex != -1)
            {
                base.UseTargetPosition(Main.npc[GallagarIndex].Center);
            }
            base.Apply();
        }
    }
}
