using Microsoft.Xna.Framework.Audio;

namespace EndlessEscapade.Core.Audio;

[Autoload(Side = ModSide.Client)]
public interface IAudioFilter : ILoadable
{
    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }

    void Apply(SoundEffectInstance instance, in AudioParameters parameters);
}
