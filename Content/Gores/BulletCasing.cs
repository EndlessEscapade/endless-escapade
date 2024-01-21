using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Gores;

public class BulletCasing : ModGore
{
    public override void SetStaticDefaults() {
        ChildSafety.SafeGore[Type] = true;
    }

    public override void OnSpawn(Gore gore, IEntitySource source) {
        gore.scale = 0.5f;
        gore.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
    }

    public override bool Update(Gore gore) {
        if (gore.alpha >= 255) {
            gore.active = false;
            return true;
        }
        
        gore.alpha += 5;
        
        return true;
    }
}
