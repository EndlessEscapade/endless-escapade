/*
 * Implementation taken and inspired by
 * https://github.com/Mirsario/TerrariaOverhaul/tree/dev/Common/Ambience
 */

using Terraria;

namespace EndlessEscapade.Common.Surroundings;

public readonly record struct SurroundingsInfo(Player Player, SceneMetrics Metrics);
