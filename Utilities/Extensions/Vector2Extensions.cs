using Microsoft.Xna.Framework;
using Terraria;

namespace EndlessEscapade.Utilities.Extensions;

public static class Vector2Extensions
{
    /// <summary>Clamps the specified <paramref name="vector"/> length between 0 and <paramref name="length"/>.</summary>
    /// <param name="vector"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector2 Limit(this Vector2 vector, float length) {
        if (vector.LengthSquared() > length * length) {
            return Vector2.Normalize(vector) * length;
        }
        return vector;
    }
}
