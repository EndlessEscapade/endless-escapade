using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience;

public class RainBubbleEffects : ILoadable
{
    void ILoadable.Load(Mod mod){ On_Rain.Update += RainUpdateHook; }

    void ILoadable.Unload() { }
    
    private static void RainUpdateHook(On_Rain.orig_Update orig, Rain self) {
        orig(self);

        if (Collision.WetCollision(self.position, 2, 2) && Main.rand.NextFloat(100f) < Main.gfxQuality * 100f) {
            var dust = Dust.NewDustDirect(self.position, 2, 2, DustID.BubbleBurst_White);
            dust.velocity = self.velocity / 2f;
            dust.noGravity = true;
            dust.scale *= Main.rand.NextFloat(1.2f, 1.5f);
            
            self.active = false;
        }
    }
}
