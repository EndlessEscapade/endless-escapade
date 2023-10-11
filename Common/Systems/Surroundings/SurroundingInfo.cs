using Terraria;

namespace EndlessEscapade.Common.Systems.Surroundings;

public record struct SurroundingInfo(Player Player, SceneMetrics Metrics, int[] TileCounts);