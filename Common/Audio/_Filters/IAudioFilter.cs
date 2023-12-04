using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

public interface IAudioFilter : ILoadable
{
    void Apply(SoundEffectInstance instance, in AudioModifiers modifiers);
}
