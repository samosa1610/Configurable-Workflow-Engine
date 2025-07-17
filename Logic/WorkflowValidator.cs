using WebApplication1.Models;

namespace WebApplication1.Logic;

/// <summary>
/// Handles all validation logic for workflow definitions and transitions.
/// Keeping this logic separate ensures WorkflowService remains clean and focused.
/// This class is static because it holds only stateless utility methods.
/// </summary>
public static class WorkflowValidator
{
    /// <summary>
    /// Validates the integrity of a workflow definition.
    /// Checks for unique IDs, valid initial state, and other consistency rules.
    /// </summary>
    /// <param name="def">The workflow definition to validate.</param>
    /// <exception cref="Exception">Thrown if definition is invalid.</exception>
    public static void ValidateDefinition(WorkflowDefinition def)
    {
        // Future: Add more rules here like name length, metadata presence, etc.

        // A workflow must have exactly one initial state
        if (def.States.Count(s => s.IsInitial) != 1)
            throw new Exception("Exactly one initial state is required.");

        // Each state must have a unique ID
        if (def.States.GroupBy(s => s.Id).Any(g => g.Count() > 1))
            throw new Exception("Duplicate state IDs detected.");

        // Each action must also have a unique ID
        if (def.Actions.GroupBy(a => a.Id).Any(g => g.Count() > 1))
            throw new Exception("Duplicate action IDs detected.");
    }

    /// <summary>
    /// Validates whether an action can be applied to a given workflow instance.
    /// Ensures that the action exists, is enabled, valid from the current state,
    /// and that the destination state is valid.
    /// </summary>
    /// <param name="inst">The workflow instance being modified.</param>
    /// <param name="def">The definition the instance follows.</param>
    /// <param name="actionId">The ID of the action being executed.</param>
    /// <exception cref="Exception">Thrown if the transition is invalid.</exception>
    public static void ValidateTransition(WorkflowInstance inst, WorkflowDefinition def, string actionId)
    {
        // Future: We could check permissions, time constraints, or roles here.

        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
        if (action == null || !action.Enabled)
            throw new Exception("Action invalid or disabled.");

        // Current state must allow this action
        if (!action.FromStates.Contains(inst.CurrentState))
            throw new Exception("Action not valid from current state.");

        // The next state must exist and be enabled
        var next = def.States.FirstOrDefault(s => s.Id == action.ToState);
        if (next == null || !next.Enabled)
            throw new Exception("Next state invalid or disabled.");

        // Cannot transition from a final state
        if (def.States.First(s => s.Id == inst.CurrentState).IsFinal)
            throw new Exception("Cannot act on a final state.");
    }
}
