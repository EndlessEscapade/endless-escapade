using System.Collections.Generic;
using System.Linq;
using EndlessEscapade.Common.Ambience;

namespace EndlessEscapade.Core.IO;

public abstract class ContentLoader<T> : IContentLoader, ILoadable
{
    /// <summary>
    ///     The file extension associated with the type of content that this loader loads.
    /// </summary>
    public abstract string Extension { get; }

    void ILoadable.Load(Mod mod) {
        foreach (var file in mod.GetFileNames()) {
            if (!file.EndsWith(Extension)) {
                continue;
            }

            ContentSystem.LoadContent<T>(mod, file);
        }
    }

    void ILoadable.Unload() { }
}
