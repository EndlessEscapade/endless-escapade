using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using InteritosMod.Autoloading;
using InteritosMod.NPCs.Akumo;

namespace InteritosMod
{
    public partial class InteritosMod
    {
        [Loading(LoadingMode.Client)]
        internal static void ShaderLoading(InteritosMod mod)
        {
            Ref<Effect> screenRef = new Ref<Effect>(mod.GetEffect("Effects/PracticeEffect"));
            Filters.Scene["InteritosMod:Akumo"] = new Filter(new AkumoScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.5f, 0.2f).UseOpacity(0.6f), EffectPriority.VeryHigh);
            Filters.Scene["InteritosMod:Boom"] = new Filter(new ScreenShaderData(screenRef, "DeathAnimation"), EffectPriority.VeryHigh);
            Filters.Scene["InteritosMod:Boom"].Load();
            SkyManager.Instance["InteritosMod:Akumo"] = new AkumoSky();
        }
    }
}
