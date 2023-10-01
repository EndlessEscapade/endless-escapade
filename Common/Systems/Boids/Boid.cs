using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;

namespace EndlessEscapade.Common.Systems.Boids;

public sealed class Boid
{
    public Vector2 Acceleration;

    public List<Boid> closeBoids;
    public float MaxForce = 0.001f;
    public float MaxVelocity = 2f;
    public float MaxVision = 32f * 16f;

    public Vector2 Position;
    public Vector2 Velocity;

    internal void Update() {
        Velocity += Acceleration;
        Position += Velocity;

        ApplyAllignment();
        ApplySeparation();
        ApplyCohesion();

        ApplyTileAvoidance();
        ApplyPlayerAvoidance();
    }

    private void ApplyAllignment() {
        var count = 0;
        var force = Vector2.Zero;

        foreach (var boid in GetCloseBoids()) {
            force += boid.Velocity;
            count++;
        }

        if (count > 0) {
            force /= count;
        }

        if (force != Vector2.Zero) {
            force = force.SafeNormalize(Vector2.Zero) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }

    private void ApplySeparation() {
        var count = 0;
        var force = Vector2.Zero;

        foreach (var boid in GetCloseBoids()) {
            var distance = Vector2.DistanceSquared(Position, boid.Position);
            var diff = Position - boid.Position;
            var weight = diff.SafeNormalize(Vector2.Zero) / distance;

            force += weight;
            count++;
        }

        if (count > 0) {
            force /= count;
        }

        if (force != Vector2.Zero) {
            force = force.SafeNormalize(Vector2.Zero) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }

    private void ApplyCohesion() {
        var count = 0;
        var force = Vector2.Zero;

        foreach (var boid in GetCloseBoids()) {
            force += boid.Position;
            count++;
        }

        if (count > 0) {
            force /= count;
            force -= Position;
            force = force.SafeNormalize(Vector2.Zero) * MaxVelocity;
        }

        if (force != Vector2.Zero) {
            force = force.SafeNormalize(Vector2.Zero) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }

    private void ApplyPlayerAvoidance() {
        var pdist = Vector2.DistanceSquared(Position, Main.LocalPlayer.Center);
        var force = Vector2.Zero;

        if (pdist < MaxVision * MaxVision && pdist > 0) {
            var d = Position - Main.LocalPlayer.Center;
            var norm = Vector2.Normalize(d);
            var weight = norm;
            force += weight;
        }

        if (force != Vector2.Zero) {
            force = Vector2.Normalize(force) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }

    private void ApplyTileAvoidance() {
        var force = Vector2.Zero;
        var tilePos = Position.ToTileCoordinates();
        
        for (var i = -1; i < 2; i++) {
            for (var j = -1; j < 2; j++) {
                if (!WorldGen.InWorld(tilePos.X + i, tilePos.Y + j, 10)) {
                    continue;
                }
                
                var tile = Framing.GetTileSafely(tilePos.X + i, tilePos.Y + j);
                var pdist = Vector2.DistanceSquared(Position, new Vector2(tilePos.X + i, tilePos.Y + j) * 16);
                    
                if ((pdist < MaxVision * MaxVision && pdist > 0 && tile.HasTile && Main.tileSolid[tile.TileType]) || tile.LiquidAmount < 100) {
                    var distance = Position - new Vector2(tilePos.X + i, tilePos.Y + j) * 16f;
                    force += distance.SafeNormalize(Vector2.Zero);
                }
            }
        }

        if (force != Vector2.Zero) {
            force = Vector2.Normalize(force) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }

    private IEnumerable<Boid> GetCloseBoids() {
        foreach (var boid in closeBoids) {
            var distance = Vector2.DistanceSquared(Position, boid.Position);
            var inRange = distance < MaxVision * MaxVision && distance > 0f;

            if (inRange) {
                yield return boid;
            }
        }
    }
}
