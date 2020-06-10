using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Autoloading;
using EEMod.NPCs.Bosses.Akumo;

namespace EEMod
{
    public partial class EEMod
    {
        [Loading(LoadingMode.Client)]
        internal static void ShaderLoading(EEMod mod)
        {
            Ref<Effect> screenRef = new Ref<Effect>(mod.GetEffect("Effects/PracticeEffect"));
            Filters.Scene["EEMod:Akumo"] = new Filter(new AkumoScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.5f, 0.2f).UseOpacity(0.6f), EffectPriority.VeryHigh);
            Filters.Scene["EEMod:Boom"] = new Filter(new ScreenShaderData(screenRef, "DeathAnimation"), EffectPriority.VeryHigh);
            Filters.Scene["EEMod:Boom"].Load();
            SkyManager.Instance["EEMod:Akumo"] = new AkumoSky();
        }
    }
}
