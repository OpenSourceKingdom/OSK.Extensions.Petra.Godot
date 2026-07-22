using Godot;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OSK.Extensions.Petra.Godot;

public static class CSharpScriptExtensions
{
    /// <summary>
    /// Attempts to get the script's underlying type. This would return the script's actual class type. 
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item> This call assumes the script is within the <c>res://</c> directory </item>
    /// </list>
    /// </remarks>
    /// <param name="script">The script to get the type for</param>
    /// <returns>The actual C# class type for the script</returns>
    public static Type? GetScriptType(this CSharpScript script)
    {
        ArgumentNullException.ThrowIfNull(script);

        try
        {
            // Get the class name from the file path (e.g., "res://Scripts/MyAction.cs" -> "MyAction")
            // Don't want to use an instance since it may require dependency injections from a dependency container
            var className = Path.GetFileNameWithoutExtension(script.ResourcePath.GetFile());
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name.Equals(className, StringComparison.Ordinal));
        }
        catch (Exception)
        {
            GD.PushWarning($"Unable to parse the class reesource {script.ResourcePath}.");
            return null;
        }
    }
}
