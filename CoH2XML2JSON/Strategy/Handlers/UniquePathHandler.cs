using System.IO;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// Class defining a simple <see cref="IPathHandler"/> that returns the filename without extension of the given filepath.
/// </summary>
public sealed class UniquePathHandler : IPathHandler {
    
    /// <inheritdoc/>
    public string GetNameFromPath(string path) => Path.GetFileNameWithoutExtension(path);

}
