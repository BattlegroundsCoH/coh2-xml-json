using System.Xml;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// Provides a helper method for selecting a specific variant from an XML document.
/// </summary>
public sealed class VariantSelector : IBlueprintHelperHandler {

    /// <summary>
    /// The name of the variant to select.
    /// </summary>
    public string Variant { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariantSelector"/> class with the specified variant name.
    /// </summary>
    /// <param name="variant">The name of the variant to select.</param>
    public VariantSelector(string variant) { 
        this.Variant = variant;
    }

    /// <summary>
    /// Selects the specified variant from the XML document.
    /// </summary>
    /// <param name="document">The XML document.</param>
    /// <returns>The selected variant as an <see cref="XmlElement"/>, or null if the variant is not found.</returns>
    public XmlElement? Select(XmlDocument document) {
        var node = document.SelectSingleNode($@"//variant[@name='{this.Variant}']");
        return node as XmlElement;
    }

}
