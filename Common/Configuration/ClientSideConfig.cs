using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EndlessEscapade.Common.Configuration;

public class ClientSideConfig : ModConfig
{
    public static ClientSideConfig Instance => ModContent.GetInstance<ClientSideConfig>();

    public override ConfigScope Mode => ConfigScope.ClientSide;
}