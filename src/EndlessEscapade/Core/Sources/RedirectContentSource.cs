using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReLogic.Content;
using ReLogic.Content.Sources;

namespace EndlessEscapade.Core.Sources;

// Code written and provided by @steviegt6 at GitHub:
// https://github.com/Path-of-Terraria/PathOfTerraria/blob/main/Core/Sources/SmartContentSource.cs
internal sealed class RedirectContentSource(IContentSource source) : IContentSource
{
    private readonly Dictionary<string, string> redirects = [];

    IContentValidator IContentSource.ContentValidator {
        get => source.ContentValidator;
        set => source.ContentValidator = value;
    }

    RejectedAssetCollection IContentSource.Rejections => source.Rejections;

    IEnumerable<string> IContentSource.EnumerateAssets() {
        return source.EnumerateAssets().Select(RewritePath);
    }

    string IContentSource.GetExtension(string assetName) {
        return source.GetExtension(RewritePath(assetName));
    }

    Stream IContentSource.OpenStream(string fullAssetName) {
        return source.OpenStream(RewritePath(fullAssetName));
    }

    public void AddRedirect(string from, string to) {
        redirects.Add(from, to);
    }

    private string RewritePath(string path) {
        foreach (var (from, to) in redirects) {
            if (path.StartsWith(from)) {
                return path.Replace(from, to);
            }
        }

        return path;
    }
}
