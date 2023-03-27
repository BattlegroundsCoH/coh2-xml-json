using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy;

public interface IBlueprintPostHandler<T> where T : IBlueprint {

    public T Handle(T blueprint, string filepath);

}
