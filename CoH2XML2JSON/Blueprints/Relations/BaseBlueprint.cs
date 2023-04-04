using CoH2XML2JSON.Blueprints.Constraints;

namespace CoH2XML2JSON.Blueprints.Relations;

/// <summary>
/// Base class for blueprint classes that can be extended by another blueprint.
/// </summary>
/// <typeparam name="T">The type of the blueprint that can be extended.</typeparam>
public abstract class BaseBlueprint<T> : Extendable<BaseBlueprint<T>>, IExtendableBlueprint<T> where T : IBlueprint {

    /// <inheritdoc/>
    public string? ParentFilepath { get; set; }

    /// <inheritdoc/>
    public T? GetParent() => this.Extends is T parent ? parent : default;

    /// <inheritdoc/>
    public virtual void ExtendWith(T parent) {
        if (parent is BaseBlueprint<T> p) {
            this.Extends = p;
            if (this is IBlueprintWithCost selfCost && selfCost.Cost is not null && p is IBlueprintWithCost parentCost) {
                selfCost.Cost.Extends = parentCost.Cost;
            }
            if (this is IBlueprintWithDisplay selfDisplay && selfDisplay.Display is not null && p is IBlueprintWithDisplay parentDisplay) {
                selfDisplay.Display.Extends = parentDisplay.Display;
            }
        }
    }

}
