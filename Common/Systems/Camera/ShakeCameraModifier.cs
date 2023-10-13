using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.CameraModifiers;

namespace EndlessEscapade.Common.Systems.Camera;

public sealed class ShakeCameraModifier : ICameraModifier
{
    public string UniqueIdentity { get; private set; }
    
    public bool Finished { get; private set; }
    
    public float Intensity { get; private set; }
    public float Decay { get; private set; }

    public ShakeCameraModifier(float intensity, float decay, string identity) {
        Intensity = intensity;
        Decay = decay;
        UniqueIdentity = identity;
    }
    
    public void Update(ref CameraInfo cameraPosition) {
        if (Intensity <= 0f) {
            Finished = true;
            return;
        }
        
        cameraPosition.CameraPosition += new Vector2(Main.rand.NextFloat(-Intensity, Intensity), Main.rand.NextFloat(-Intensity, Intensity));
        Intensity *= Decay;
    }
}
