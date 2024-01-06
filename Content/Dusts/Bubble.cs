using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Dusts;

public class Bubble : ModDust
{
    public override void OnSpawn(Dust dust) {
        dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 10, 10);
        
        dust.noLight = true;
        dust.noGravity = true;
        
        dust.alpha = 100;
        
        dust.scale *= Main.rand.NextFloat(0.9f, 1.2f);
        dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
    }
    
    public override bool Update(Dust dust) {
        dust.position += dust.velocity;
        dust.velocity *= 0.99f;
        
        dust.scale -= 0.025f;
        dust.alpha += 2;
        
        dust.rotation += dust.velocity.ToRotation() * 0.1f;

        var hitTile = !WorldGen.TileEmpty((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));
        var invisible = dust.scale <= 0f || dust.alpha >= 255;    
    
        if (hitTile || invisible) {
            dust.active = false;
        }

        return false;
    }
}
