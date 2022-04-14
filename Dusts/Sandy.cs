using Terraria;
using Terraria.ModLoader;

namespace EEMod.Dusts
{
    public class Sandy : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.8f;
            dust.velocity.Y *= 0.8f;
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 4.0f;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if (dust.velocity.Y > -8)
            {
                dust.velocity.Y -= .1f;
            }

            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.98f;
            if (dust.scale < 0.2f)
            {
                // dust.active = false;
            }
            return false;
        }
    }
}