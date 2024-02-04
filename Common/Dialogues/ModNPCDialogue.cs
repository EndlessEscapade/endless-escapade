using EndlessEscapade.Content.NPCs.Town;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Dialogues;

public abstract class ModNPCDialogue : ModType, ILocalizedModType
{
    public string LocalizationCategory => "Dialogues";

    protected sealed override void Register() {
        ModTypeLookup<ModNPCDialogue>.Register(this);
    }
}
