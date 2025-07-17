using WebApplication1.Models;

namespace WebApplication1.Logic;

/// <summary>
/// Responsible for storing and retrieving workflow definitions and instances.
/// Uses in-memory dictionaries for simplicity and speed.
/// 
/// In the future, this can be replaced with a file-based or database-backed store
/// without changing much of the application logic.
/// </summary>
public class WorkflowRepository
{
    // For now, we're storing everything in-memory.
    // This keeps things fast and lightweight, and works well for testing/demo purposes.
    // If needed, we can later swap this out for file or database storage easily.

    /// <summary>
    /// In-memory storage for workflow definitions.
    /// Key: Definition ID
    /// </summary>
    public Dictionary<string, WorkflowDefinition> Definitions { get; } = new();

    /// <summary>
    /// In-memory storage for active workflow instances.
    /// Key: Instance ID
    /// </summary>
    public Dictionary<string, WorkflowInstance> Instances { get; } = new();

    /// <summary>
    /// Adds a new workflow definition to the store.
    /// Throws an exception if a definition with the same ID already exists.
    /// </summary>
    /// <param name="def">The workflow definition to store.</param>
    public void AddDefinition(WorkflowDefinition def)
    {
        if (Definitions.ContainsKey(def.Id))
            throw new Exception($"Workflow '{def.Id}' already exists.");

        Definitions[def.Id] = def;
    }

    /// <summary>
    /// Retrieves a workflow definition by its ID.
    /// Returns null if not found.
    /// </summary>
    public WorkflowDefinition? GetDefinition(string id) =>
        Definitions.TryGetValue(id, out var def) ? def : null;

    /// <summary>
    /// Stores a new workflow instance.
    /// </summary>
    /// <param name="inst">The instance to add.</param>
    public void AddInstance(WorkflowInstance inst) =>
        Instances[inst.Id] = inst;

    /// <summary>
    /// Retrieves a workflow instance by ID.
    /// Returns null if not found.
    /// </summary>
    public WorkflowInstance? GetInstance(string id) =>
        Instances.TryGetValue(id, out var inst) ? inst : null;
}
