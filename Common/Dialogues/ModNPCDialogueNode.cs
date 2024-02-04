using System;
using Terraria.Localization;

namespace EndlessEscapade.Common.Dialogues;

public readonly record struct ModNPCDialogueNode(LocalizedText Text, Action Result = null);
