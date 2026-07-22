using Godot;

namespace OSK.Extensions.Petra.Godot;

public static class Vector2Extensions
{

    #region Numerics to Godot

    /// <summary>
    /// Converts the numeric vector2 into the equivalent Godot vector2
    /// </summary>
    /// <param name="vector">The vector2 to convert</param>
    /// <returns>The godot vector2</returns>
    public static Vector2 ToGodot2(this System.Numerics.Vector2 vector)
        => new(vector.X, vector.Y);

    /// <summary>
    /// Converts the numeric vector2 into the equivalent Godot vector3
    /// </summary>
    /// <param name="vector">The vector2 to convert</param>
    /// <param name="y">The y height the vector3 should be placed at</param>
    /// <returns>An equivalenmt vector3</returns>
    public static Vector3 ToGodot3(this System.Numerics.Vector2 vector, float y = 0)
        => new(vector.X, y, vector.Y);

    #endregion

    #region Godot to Numerics

    /// <summary>
    /// Converts the Godot vector2 into the equivalent numeric vector2
    /// </summary>
    /// <param name="vector">The vector2 to convert</param>
    /// <returns>The numeric vector2</returns>
    public static System.Numerics.Vector2 ToNumerics2(this Vector2 vector)
        => new(vector.X, vector.Y);

    /// <summary>
    /// Converts the Godot vector2 into the equivalent numeric vector3
    /// </summary>
    /// <param name="vector">The vector2 to convert</param>
    /// <param name="y">The y height the vector3 should be placed at</param>
    /// <returns>An equivalenmt vector3</returns>
    public static System.Numerics.Vector3 ToNumerics3(this Vector2 vector, float y = 0)
        => new(vector.X, y, vector.Y);

    #endregion
}
