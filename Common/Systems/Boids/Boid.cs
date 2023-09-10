using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Boids;

public class BoidSystem : ModSystem
{
    private static BoidFlock flock = new();
    
    public override void PostUpdateEverything() {
        if (Main.keyState.IsKeyDown(Keys.F) && !Main.oldKeyState.IsKeyDown(Keys.F)) {
            flock = new BoidFlock();
        }
        
        flock.Update();
    }

    public override void PostDrawTiles() {
        Main.spriteBatch.Begin();
        flock.Draw();
        Main.spriteBatch.End();
    }
}

public class BoidFlock
{
    public readonly List<Boid> Boids = new();

    public BoidFlock() {
        for (var i = 0; i < 10; i++) {
            Boids.Add(new Boid() {
                Position = Main.MouseWorld + Main.rand.NextVector2Square(8f, 16f) * 16f,
                closeBoids = Boids
            });
        }
    }
    
    internal void Update() {
        foreach (var boid in Boids) {
            boid?.Update();
        }
    }

    internal void Draw() {
        foreach (var boid in Boids) {
            Main.spriteBatch.Draw(
                ModContent.Request<Texture2D>("EndlessEscapade/AquamarineFish").Value, 
                boid.Position - Main.screenPosition, 
                null,
                Color.White,
                boid.Velocity.ToRotation(),
                ModContent.Request<Texture2D>("EndlessEscapade/AquamarineFish").Value.Size() / 2f,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}

public class Boid
{
    public float MaxVision = 32f * 16f;
    public float MaxForce = 0.001f;
    public float MaxVelocity = 2f;
    
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;

    public List<Boid> closeBoids;

    internal void Update() {
        Velocity += Acceleration;
        Position += Velocity;

        ApplyAllignment();
        ApplySeparation();
        ApplyCohesion();
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
