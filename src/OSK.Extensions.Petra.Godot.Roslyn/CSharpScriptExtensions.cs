using Godot;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Extensions.Petra.Godot.Roslyn;

public static class CSharpScriptExtensions
{
    private static readonly Dictionary<string, Type?> _knwonScriptTypes = [];

    /// <summary>
    /// Attempts to get the script's underlying type. This would return the script's actual class type. 
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item> If the source code is not immediately available for the script, it will be loaded from the file located at the ResourcePath </item>
    /// <item> This method currently only supports class style declarations </item>
    /// </list>
    /// </remarks>
    /// <param name="script">The script to get the type for</param>
    public static Type? GetScriptType(this CSharpScript script)
    {
        ArgumentNullException.ThrowIfNull(script);

        if (string.IsNullOrWhiteSpace(script.SourceCode))
        {
            script.SourceCode = FileAccess.GetFileAsString(script.ResourcePath);
            if (FileAccess.GetOpenError() != Error.Ok)
            {
                return null;
            }
        }

        var classDeclarationSyntax = CSharpSyntaxTree.ParseText(script.SourceCode).GetRoot()
                                               .DescendantNodes()
                                               .OfType<ClassDeclarationSyntax>()
                                               .FirstOrDefault();
        if (classDeclarationSyntax is null)
        {
            GD.PushWarning($"Unable to parse the class reesource source code: {script.ResourcePath}.");
            return null;
        }

        var namespaceNames = classDeclarationSyntax.Ancestors()
                                                    .OfType<BaseNamespaceDeclarationSyntax>()
                                                    .Reverse()
                                                    .Select(syntax => syntax.Name.ToString());

        var scriptTypeName = string.Join(".", namespaceNames.Append(classDeclarationSyntax.Identifier.Text));
        if (_knwonScriptTypes.TryGetValue(scriptTypeName, out var scriptType))
        {
            return scriptType;
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            scriptType = namespaceNames.Any()
                ? assembly.GetType(scriptTypeName)
                : assembly.GetTypes().FirstOrDefault(assemblyType => assemblyType.Name.Equals(scriptTypeName, StringComparison.Ordinal));
            if (scriptType is not null)
            {
                break;
            }
        }

        _knwonScriptTypes[scriptTypeName] = scriptType;
        return null;
    }
}
