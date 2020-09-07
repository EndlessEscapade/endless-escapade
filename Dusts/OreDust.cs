using Terraria;
using Terraria.ModLoader;

namespace EEMod.Dusts
{
    public abstract class OreDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            dust.scale = 1.05f;
            dust.alpha = 170;
            dust.noGravity = true;
            dust.velocity /= 2.1f;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.96f; //
            float light = 0.35f * dust.scale;
            Lighting.AddLight(dust.position, light, light, light); // dust will doe lieght depending on the scale
            if (dust.scale < 0.5f) // when scale manages to be .5f it will die, big F
            {
                dust.active = false;
            }
            return false;
        }
    }
}