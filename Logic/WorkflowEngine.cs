using WebApplication1.Models;

namespace WebApplication1.Logic;

/// <summary>
/// Handles the core state machine logic for workflow instances.
/// This class is responsible only for transitions and starting instances,
/// keeping business rules and data storage elsewhere.
/// </summary>
public class WorkflowEngine
{
    /// <summary>
    /// Starts a new instance of the given workflow definition.
    /// Selects the initial state and returns a new WorkflowInstance object.
    /// </summary>
    /// <param name="def">The workflow definition to start from.</param>
    /// <returns>A new workflow instance with its initial state.</returns>
    public WorkflowInstance Start(WorkflowDefinition def)
    {
        var initial = def.States.First(s => s.IsInitial);
        return new WorkflowInstance
        {
            DefinitionId = def.Id,
            CurrentState = initial.Id
        };
    }

    /// <summary>
    /// Applies the specified action to a workflow instance.
    /// Updates the current state and logs the transition history.
    /// </summary>
    /// <param name="inst">The instance to modify.</param>
    /// <param name="def">The definition the instance is based on.</param>
    /// <param name="actionId">The action to apply.</param>
    public void ApplyAction(WorkflowInstance inst, WorkflowDefinition def, string actionId)
    {
        // In the future, this is a great place to trigger side effects:
        // e.g., sending emails, writing logs, or notifying external services.

        var action = def.Actions.First(a => a.Id == actionId);
        inst.CurrentState = action.ToState;
        inst.History.Add((actionId, DateTime.UtcNow));
    }
}
