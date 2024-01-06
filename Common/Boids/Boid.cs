using System.Collections.Generic;
using EndlessEscapade.Common.Systems.Boids;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;

namespace EndlessEscapade.Common.Boids;

public sealed class Boid
{
    public readonly float MaxVelocity;
    public readonly float MaxForce;
    public readonly float MaxVision;

    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;

    public Boid(float maxVelocity, float maxForce, float maxVision) {
        MaxVelocity = maxVelocity;
        MaxForce = maxForce;
        MaxVision = maxVision;
    }

    internal void Update() {
        ApplySeparation();
        ApplyAllignment();
        ApplyCohesion();

        Velocity += Acceleration;
        Velocity = Velocity.Limit(MaxVelocity);

        Position += Velocity;

        Acceleration = Vector2.Zero;
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

    private void ApplyAvoidance() {
        var player = Main.LocalPlayer;

        var force = Vector2.Zero;
        var distance = Vector2.DistanceSquared(Position, player.Center);

        if (distance < MaxVision * MaxVision && distance > 0f) {
            var difference = Position - player.Center;

            force += difference.SafeNormalize(Vector2.Zero);
        }

        if (force != Vector2.Zero) {
            force = force.SafeNormalize(Vector2.Zero) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }

    private IEnumerable<Boid> GetCloseBoids() {
        foreach (var boid in BoidSystem.Boids) {
            var distance = Vector2.DistanceSquared(Position, boid.Position);
            var inRange = distance < 100f * 100f && distance > 0f;

            if (!inRange) {
                continue;
            }

            yield return boid;
        }
    }
}
