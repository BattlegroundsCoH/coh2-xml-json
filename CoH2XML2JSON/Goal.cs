namespace CoH2XML2JSON;

/// <summary>
/// Represents a goal object that is used to instruct an IGameStrategy implementation what to translate from XML files to a Json database.
/// </summary>
public sealed class Goal {
    
    /// <summary>
    /// Gets or sets the output path for the goal object.
    /// </summary>
    /// <value>The output path for the goal object.</value>
    public string OutPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the instance path for the goal object.
    /// </summary>
    /// <value>The instance path for the goal object.</value>
    public string InstancePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mod GUID for the goal object.
    /// </summary>
    /// <value>The mod GUID for the goal object.</value>
    public string ModGuid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mod name for the goal object.
    /// </summary>
    /// <value>The mod name for the goal object.</value>
    public string ModName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the variant to pick when converting a CoH3 blueprint.
    /// </summary>
    /// <value>The name of the variant to use when reading a CoH3 XML blueprint file.</value>
    public string Variant { get; set; } = "default";

}
