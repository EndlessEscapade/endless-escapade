using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
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
            Ref<Effect> screenRef2 = new Ref<Effect>(mod.GetEffect("Effects/Shockwave"));
            Ref<Effect> screenRef3 = new Ref<Effect>(mod.GetEffect("Effects/Pause"));
            Filters.Scene["EEMod:Akumo"] = new Filter(new AkumoScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.5f, 0.2f).UseOpacity(0.6f), EffectPriority.VeryHigh);
            Filters.Scene["EEMod:Boom"] = new Filter(new ScreenShaderData(screenRef, "DeathAnimation"), EffectPriority.VeryHigh);
            Filters.Scene["EEMod:Boom"].Load();
            Filters.Scene["EEMod:Shockwave"] = new Filter(new ScreenShaderData(screenRef2, "Shockwave"), EffectPriority.VeryHigh);
            Filters.Scene["EEMod:Shockwave"].Load();
            Filters.Scene["EEMod:Pause"] = new Filter(new ScreenShaderData(screenRef3, "Pauses"), EffectPriority.VeryHigh);
            Filters.Scene["EEMod:Pause"].Load();
            SkyManager.Instance["EEMod:Akumo"] = new AkumoSky();
        }
    }
}
