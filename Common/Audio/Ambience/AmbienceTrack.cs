using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

public abstract class AmbienceTrack : ModType
{
    private SoundEffectInstance soundInstance;

    public abstract string Path { get; }
    public abstract bool Active { get; }

    public virtual float FadeIn { get; } = 0.005f;
    public virtual float FadeOut { get; } = 0.005f;

    public virtual void Setup() {
        soundInstance = ModContent.Request<SoundEffect>(Path, AssetRequestMode.ImmediateLoad).Value.CreateInstance();
        soundInstance.IsLooped = true;
        soundInstance.Volume = 0f;
    }

    public virtual void Update() {
        if (soundInstance == null) {
            return;
        }

        if (Active) {
            soundInstance.Volume += FadeIn;
        }
        else if (!Active) {
            soundInstance.Volume -= FadeOut;
        }

        soundInstance.Volume = MathHelper.Clamp(soundInstance.Volume, 0f, Main.ambientVolume);

        if (Main.hasFocus && !Main.gameMenu) {
            if (soundInstance.State == SoundState.Stopped) {
                soundInstance.Play();
            }
            else if (soundInstance.State == SoundState.Paused) {
                soundInstance.Resume();
            }
        }
        else {
            soundInstance.Pause();
        }
    }

    protected sealed override void Register() {
        ModTypeLookup<AmbienceTrack>.Register(this);
    }
}