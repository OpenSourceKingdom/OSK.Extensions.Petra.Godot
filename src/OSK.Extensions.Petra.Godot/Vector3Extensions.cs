using Godot;

namespace OSK.Extensions.Petra.Godot;

public static class Vector3Extensions
{
    #region Numerics to Godot

    /// <summary>
    /// Converts the numeric vector3 into the equivalent Godot vector3
    /// </summary>
    /// <param name="vector">The vector3 to convert</param>
    /// <returns>The godot vector3</returns>
    public static Vector3 ToGodot3(this System.Numerics.Vector3 vector)
        => new(vector.X, vector.Y, vector.Z);

    /// <summary>
    /// Converts the numeric vector3 into a Godot Vector2
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>The dropped value is the Y height</item>
    /// </list>
    /// </remarks>
    /// <param name="vector">The vector3 to convert</param>
    /// <returns>An equivalenmt vector2</returns>
    public static Vector2 ToGodot2(this System.Numerics.Vector3 vector)
        => new(vector.X, vector.Z);

    #endregion

    #region Godot to Numerics

    /// <summary>
    /// Converts the Godot vector3 into the equivalent numeric vector3
    /// </summary>
    /// <param name="vector">The vector3 to convert</param>
    /// <returns>The numeric vector3</returns>
    public static System.Numerics.Vector3 ToNumerics3(this Vector3 vector)
        => new(vector.X, vector.Y, vector.Z);

    /// <summary>
    /// Converts the Godot vector3 into a Godot Vector2
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>The dropped value is the Y height</item>
    /// </list>
    /// </remarks>
    /// <param name="vector">The vector3 to convert</param>
    /// <returns>An equivalenmt vector2</returns>
    public static System.Numerics.Vector2 ToNumerics2(this Vector3 vector)
        => new(vector.X, vector.Z);

    #endregion
}
