using EndlessEscapade.Common.Seamap;
using EndlessEscapade.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade;

public sealed class EndlessEscapade : Mod
{
    public static EndlessEscapade Instance => ModContent.GetInstance<EndlessEscapade>();

    public override void Load() {
        On_Main.DrawProjectiles += static (orig, self) => {
            if (!Main.dedServ)
            {
                if (!SubworldSystem.IsActive<Sea>())
                {
                }
                else
                {
                    Seamap.Render();
                }
            }
        };
    }
}
