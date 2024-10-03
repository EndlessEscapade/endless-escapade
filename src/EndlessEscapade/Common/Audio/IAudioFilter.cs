using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

[Autoload(Side = ModSide.Client)]
public interface IAudioFilter : ILoadable
{
    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }

    void Apply(SoundEffectInstance instance, in AudioParameters parameters);
}
