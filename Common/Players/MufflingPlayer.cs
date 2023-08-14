using System;
using EndlessEscapade.Common.Systems.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class MufflingPlayer : ModPlayer
{
    public const float Duration = 180f;

    private float intensity;
    
    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, Player.wet ? 0.25f : 0f, 1f);
    }

    private float timer;

    public float Timer {
        get => timer;
        set => timer = MathHelper.Clamp(value, 0f, Duration);
    }

    public override void PreUpdate() {
        if (Player.wet) {
            Timer++;

            if (Timer < 180) {
                Intensity += 0.025f;
            }
            else {
                Intensity -= 0.025f;
            }
        }
        else {
            Timer--;
            Intensity = Timer / Duration;
        }
        
        if (Intensity <= 0f) {
            return;
        }
        
        AudioSystem.SetModifiers(new AudioModifiers() with {
            LowPass = Intensity
        });
    }
}
