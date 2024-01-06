using System;
using System.Collections.Generic;
using EndlessEscapade.Common.Systems.Boids;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Boids;

public sealed class Boid
{
    public readonly float MaxVelocity;
    public readonly float MaxForce;
    public readonly float MaxVision;

    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;

    internal void Update() {
        Velocity += Acceleration;
        Position += Velocity;

        ApplyAllignment();
        ApplySeparation();
        ApplyCohesion();
        ApplyCollision();
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
        var force = Vector2.Zero;
        var dist = Vector2.DistanceSquared(Position, Main.LocalPlayer.Center);

        if (dist < MaxVision * MaxVision && dist > 0) {
            var diff = Position - Main.LocalPlayer.Center;
            var norm = diff.SafeNormalize(Vector2.Zero);
            force += norm;
        }

        if (force != Vector2.Zero) {
            force = Vector2.Normalize(force) * MaxVelocity;
            Acceleration += (force - Velocity).Limit(MaxForce);
        }
    }   

    private void ApplyCollision() {
        
    }

    private IEnumerable<Boid> GetCloseBoids() {
        foreach (var boid in BoidSystem.Boids) {
            var distance = Vector2.DistanceSquared(Position, boid.Position);
            var inRange = distance < MaxVision * MaxVision && distance > 0f;

            if (inRange) {
                yield return boid;
            }
        }
    }
}