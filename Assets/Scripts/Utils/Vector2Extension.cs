using UnityEngine;

public static class Vector2Extension
{
    /*public static Vector2 Rotate(this Vector2 v, float degrees)
          {
              float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
              float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

              float tx = v.x;
              float ty = v.y;
              v.x = (cos * tx) - (sin * ty);
              v.y = (sin * tx) + (cos * ty);
              return v;
          }*/

    /** @return the angle in degrees of this vector (point) relative to the x-axis. Angles are towards the positive y-axis (typically
    *         counter-clockwise) and between 0 and 360. */

    public static float Angle(this Vector2 v)
    {
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        return angle;
    }

    /** Sets the angle of the vector in degrees relative to the x-axis, towards the positive y-axis (typically counter-clockwise).
    * @param degrees The angle in degrees to set. */
    public static Vector2 SetAngle(this Vector2 v, float degrees)
    {
        return SetAngleRad(v, degrees * Mathf.Deg2Rad);
    }

    /** Sets the angle of the vector in radians relative to the x-axis, towards the positive y-axis (typically counter-clockwise).
    * @param radians The angle in radians to set. */
    public static Vector2 SetAngleRad(this Vector2 v, float radians)
    {
        v.x = v.magnitude;
        v.y = 0f;
        v = v.RotateRad(radians);

        return new Vector2(v.x, v.y);
    }

    /** Rotates the Vector2 by the given angle, counter-clockwise assuming the y-axis points up.
    * @param radians the angle in radians */
    public static Vector2 RotateRad(this Vector2 v, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        float newX = v.x * cos - v.y * sin;
        float newY = v.x * sin + v.y * cos;

        v.x = newX;
        v.y = newY;

        return new Vector2(v.x, v.y);
    }

    public static Vector2 SetLength(this Vector2 v, float len)
    {
        return SetLength2(v, len * len);
    }

    public static Vector2 SetLength2(this Vector2 v, float len2)
    {
        float oldLen2 = v.sqrMagnitude;
        Vector2 output = new Vector2(v.x, v.y);

        if (oldLen2 == 0 || oldLen2 == len2) return output;

        output = output.Scl(Mathf.Sqrt(len2 / oldLen2));
        return output;
    }


    public static Vector2 Scl(this Vector2 v, float scalar)
    {
        Vector2 output = new Vector2(v.x, v.y);

        output.x *= scalar;
        output.y *= scalar;

        return output;
    }

    /// <summary>
    ///   <para>The angle in degrees of this vector (point) relative to the given vector. Angles are towards the positive y-axis.
    /// (typically counter-clockwise.) between -180 and +180</para>
    /// </summary>
    public static float Angle(this Vector2 v, Vector2 reference)
    {
        return Mathf.Atan2(v.Crs(reference), v.Dot(reference)) * Mathf.Rad2Deg;
    }

    /** Calculates the 2D cross product between this and the given vector.
 * @param v the other vector
 * @return the cross product */

    public static float Crs(this Vector2 v, Vector2 other)
    {
        return v.x * other.y - v.y * other.x;
    }

    public static float Dot(this Vector2 v, Vector2 other)
    {
        return v.x * other.x + v.y * other.y;
    }
}