using EndlessEscapade.Core.Sources;
using ReLogic.Content.Sources;

namespace EndlessEscapade;

public sealed class EndlessEscapade : Mod
{
    public static EndlessEscapade Instance => ModContent.GetInstance<EndlessEscapade>();

    public override IContentSource CreateDefaultContentSource() {
        var source = new RedirectContentSource(base.CreateDefaultContentSource());

        source.AddRedirect("Content", "Assets/Textures");

        return source;
    }
}
