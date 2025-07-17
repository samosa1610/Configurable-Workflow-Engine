using WebApplication1.Models;
using WebApplication1.Logic;

namespace WebApplication1.Services;

/// <summary>
/// This class ties everything together — validation, transition logic, and data storage.
/// Acts as the main interface for the API to interact with workflows.
///
/// The goal is to keep this orchestration layer clean so it's easy to extend later
/// with features like persistence, logging, authentication, etc.
///
/// Think of this as the control tower for all workflow operations.
/// </summary>
public class WorkflowService
{
    // Handles in-memory storage of workflow definitions and instances.
    private readonly WorkflowRepository repo = new();

    // Encapsulates the logic for starting workflows and applying transitions.
    private readonly WorkflowEngine engine = new();

    /// <summary>
    /// Creates and stores a new workflow definition after validating it.
    /// </summary>
    /// <param name="def">The workflow definition to create.</param>
    /// <returns>The ID of the created definition.</returns>
    public string CreateDefinition(WorkflowDefinition def)
    {
        WorkflowValidator.ValidateDefinition(def);
        repo.AddDefinition(def);
        return def.Id;
    }

    /// <summary>
    /// Starts a new workflow instance from a given definition.
    /// </summary>
    /// <param name="defId">The ID of the workflow definition.</param>
    /// <returns>A new instance initialized to the definition's initial state.</returns>
    public WorkflowInstance StartInstance(string defId)
    {
        var def = repo.GetDefinition(defId)
            ?? throw new Exception($"Workflow definition '{defId}' not found.");

        var instance = engine.Start(def);
        repo.AddInstance(instance);
        return instance;
    }

    /// <summary>
    /// Executes a transition action on a given workflow instance.
    /// Validates action applicability and updates the instance state.
    /// </summary>
    /// <param name="instanceId">The ID of the workflow instance.</param>
    /// <param name="actionId">The action to apply.</param>
    public void ExecuteAction(string instanceId, string actionId)
    {
        var inst = repo.GetInstance(instanceId)
            ?? throw new Exception($"Workflow instance '{instanceId}' not found.");

        var def = repo.GetDefinition(inst.DefinitionId)
            ?? throw new Exception("Associated workflow definition not found.");

        WorkflowValidator.ValidateTransition(inst, def, actionId);
        engine.ApplyAction(inst, def, actionId);
    }

    /// <summary>
    /// Retrieves a workflow definition by its ID.
    /// </summary>
    public WorkflowDefinition? GetDefinition(string id) => repo.GetDefinition(id);

    /// <summary>
    /// Retrieves a workflow instance by its ID.
    /// </summary>
    public WorkflowInstance? GetInstance(string id) => repo.GetInstance(id);
}
