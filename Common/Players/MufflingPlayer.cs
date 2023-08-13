using EndlessEscapade.Common.Systems.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class MufflingPlayer : ModPlayer
{
    private const int Duration = 180;
    
    private bool hitBack;
    
    private float timer;
    private float intensity;
    
    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, 0.05f, 0.85f);
    }
    
    public override void PreUpdate() {
        if (Player.wet) {
            Intensity += 0.05f;
        }
        else {
            Intensity -= 0.05f;
        }
        
        if (Intensity <= 0f) {
            return;
        }
        
        AudioSystem.SetModifiers(new AudioModifiers() with {
            LowPass = Intensity
        });
    }
}
