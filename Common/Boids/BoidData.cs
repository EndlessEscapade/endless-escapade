using Newtonsoft.Json;

namespace EndlessEscapade.Common.Boids;

public readonly struct BoidData
{
    [JsonRequired]
    public readonly string TexturePath;

    [JsonRequired]
    public readonly float MaxVision;

    [JsonRequired]
    public readonly float MaxForce;

    [JsonRequired]
    public readonly float MaxVelocity;
}
