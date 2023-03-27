namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Defines a strategy for executing a translation task from XML files to a JSON database based on the specified goal.
/// </summary>
public interface IGameStrategy {

    /// <summary>
    /// Executes a translation task from XML files to a JSON database based on the specified goal.
    /// </summary>
    /// <param name="goal">The goal object that specifies the translation task.</param>
    void Execute(Goal goal);

}
