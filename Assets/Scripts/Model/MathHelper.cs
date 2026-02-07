using UnityEngine;

public static class MathHelper
{
    /// <summary>
    /// Rotates a Vector2 by the given angle
    /// </summary>
    /// <param name="vector">The vector to be rotated</param>
    /// <param name="degrees">The rotation in degrees</param>
    /// <returns>The rotated vector</returns>
    public static Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
}
