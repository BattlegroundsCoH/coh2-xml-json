using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// Class defining a path handler that specified the SCAR name of the blueprint.
/// </summary>
public sealed class ScarPathHandler : IPathHandler {

    /// <inheritdoc/>
    public string GetNameFromPath(string path) {
        
        // Just return if null or empty
        if (string.IsNullOrEmpty(path)) {
            return string.Empty;
        }

        // Split into path elements
        string[] elements = path.Replace("\\", "/").Split('/');
        if (elements.Length < 2) {
            return string.Empty;
        }

        // Prepare stack
        Stack<string> stack = new Stack<string>();
        stack.Push(Path.GetFileNameWithoutExtension(elements[^1]).ToUpperInvariant());

        for (int i = elements.Length - 2; i >= 0; i--) {
            string? pushName = elements[i].ToLowerInvariant() switch {
                "ebps" => "EBP",
                "sbps" => "SBP",
                "abilities" => "ABILITIES",
                "upgrade" => "UPGRADE",
                "weapon" => "WEAPON",
                "gameplay" => "GAMEPLAY",
                "british_africa" => "BRITISH_AFRICA",
                "british" => "BRITISH",
                "german" => "GERMANS",
                "germans" => "GERMANS",
                "americans" => "AMERICANS",
                "afrika_korps" => "AFRIKA_KORPS",
                "common" => "COMMON",
                _ => null
            };
            if (!string.IsNullOrEmpty(pushName)) {
                stack.Push(pushName);
            }
        }

        // Build path
        StringBuilder fullPath = new StringBuilder();
        while (stack.Count > 0) {
            fullPath.Append(stack.Pop());
            if (stack.Count > 0) {
                fullPath.Append('.');
            }
        }

        // Return the newly built path
        return fullPath.ToString();

    }

}
