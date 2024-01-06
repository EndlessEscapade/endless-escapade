using Microsoft.Xna.Framework;

namespace EndlessEscapade.Utilities.Extensions;

public static class Vector2Extensions
{
    public static Vector2 Limit(this Vector2 vector, float length) {
        if (vector.LengthSquared() > length * length) {
            return Vector2.Normalize(vector) * length;
        }

        return vector;
    }
}
