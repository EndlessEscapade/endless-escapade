using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Akumo
{
    public class AkumoScreenShaderData : ScreenShaderData
    {
        // Thanks OS for code!
        private int akumoIndex;

        public AkumoScreenShaderData(string passName) : base(passName)
        {
        }

        private void UpdateAkumoIndex()
        {
            int num = ModContent.NPCType<Akumo>();
            if (akumoIndex >= 0 && Main.npc[akumoIndex].active && Main.npc[akumoIndex].type == num)
            {
                return;
            }
            akumoIndex = Helpers.FirstNPCIndex(num);
        }

        public override void Apply()
        {
            UpdateAkumoIndex();
            if (akumoIndex != -1)
            {
                base.UseTargetPosition(Main.npc[akumoIndex].Center);
            }
            base.Apply();
        }
    }
}